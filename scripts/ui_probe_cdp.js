#!/usr/bin/env node
"use strict";
// UI probe for the Unity WebGL build at http://127.0.0.1:8123/
// Usage: node ui_probe_cdp.js <plan.json>
// plan: { url, outDir, steps: [ {wait: ms} | {shot: name} | {click: [fx, fy]} | {key: "Space"} ] }
// click coords are FRACTIONS of the unity canvas (0..1).

const fs = require("node:fs");
const http = require("node:http");
const path = require("node:path");
const { spawn, spawnSync } = require("node:child_process");

const root = path.resolve(__dirname, "..");
const plan = JSON.parse(fs.readFileSync(process.argv[2], "utf8"));
const cdpPort = plan.cdpPort || 9555;
const outDir = path.resolve(root, plan.outDir || "Temp/ui_probe");
fs.mkdirSync(outDir, { recursive: true });
const profileDir = path.join(root, "Temp", "ui-probe-profile");

function requestJson(url) {
  return new Promise((resolve, reject) => {
    const req = http.get(url, (res) => {
      let body = "";
      res.setEncoding("utf8");
      res.on("data", (c) => (body += c));
      res.on("end", () => {
        try { resolve(JSON.parse(body)); } catch (e) { reject(e); }
      });
    });
    req.on("error", reject);
    req.setTimeout(2000, () => req.destroy(new Error("timeout " + url)));
  });
}

async function waitFor(fn, label, timeoutMs = 30000) {
  const start = Date.now();
  let lastErr;
  while (Date.now() - start < timeoutMs) {
    try {
      const v = await fn();
      if (v) return v;
    } catch (e) { lastErr = e; }
    await new Promise((r) => setTimeout(r, 400));
  }
  throw new Error(`${label} not ready${lastErr ? ": " + lastErr.message : ""}`);
}

function findBrowser() {
  const candidates = [
    process.env.CHROME_PATH,
    "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
    "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe",
    "C:\\Program Files\\Microsoft\\Edge\\Application\\msedge.exe",
    "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe",
  ].filter(Boolean);
  const found = candidates.find((c) => fs.existsSync(c));
  if (!found) throw new Error("no browser found");
  return found;
}

function cdpSocket(wsUrl) {
  let nextId = 1;
  const pending = new Map();
  const console_ = [];
  const socket = new WebSocket(wsUrl);
  socket.addEventListener("message", (event) => {
    const msg = JSON.parse(event.data);
    if (msg.id && pending.has(msg.id)) {
      const { resolve, reject } = pending.get(msg.id);
      pending.delete(msg.id);
      if (msg.error) reject(new Error(JSON.stringify(msg.error)));
      else resolve(msg.result);
      return;
    }
    if (msg.method === "Runtime.consoleAPICalled") {
      const args = (msg.params.args || []).map((a) => a.value !== undefined ? String(a.value) : (a.description || a.type)).join(" ");
      console_.push(`[${msg.params.type}] ${args}`);
    } else if (msg.method === "Runtime.exceptionThrown") {
      const d = msg.params.exceptionDetails;
      console_.push(`[exception] ${d.text} ${(d.exception && d.exception.description) || ""}`);
    } else if (msg.method === "Log.entryAdded") {
      const e = msg.params.entry;
      console_.push(`[log:${e.level}] ${e.text}`);
    }
  });
  const opened = new Promise((res, rej) => {
    socket.addEventListener("open", res, { once: true });
    socket.addEventListener("error", rej, { once: true });
  });
  function command(method, params = {}, timeout = 20000) {
    const id = nextId++;
    socket.send(JSON.stringify({ id, method, params }));
    return new Promise((resolve, reject) => {
      pending.set(id, { resolve, reject });
      setTimeout(() => {
        if (pending.has(id)) { pending.delete(id); reject(new Error(method + " timed out")); }
      }, timeout);
    });
  }
  return { opened, command, close: () => socket.close(), console_ };
}

async function main() {
  // kill leftover probe browser
  spawnSync("taskkill", ["/F", "/IM", "chrome.exe", "/FI", `COMMANDLINE eq *ui-probe-profile*`], { stdio: "ignore" });
  fs.rmSync(profileDir, { recursive: true, force: true });

  const browserPath = findBrowser();
  const browser = spawn(browserPath, [
    `--remote-debugging-port=${cdpPort}`,
    "--remote-allow-origins=*",
    "--no-first-run",
    "--disable-default-apps",
    "--disable-background-networking",
    "--disable-background-timer-throttling",
    "--disable-backgrounding-occluded-windows",
    "--disable-renderer-backgrounding",
    "--window-size=720,820",
    "--window-position=0,0",
    `--user-data-dir=${profileDir}`,
    plan.url,
  ], { cwd: root, stdio: "ignore", windowsHide: false });

  let cdp;
  try {
    const target = await waitFor(async () => {
      const tabs = await requestJson(`http://127.0.0.1:${cdpPort}/json/list`);
      return tabs.find((t) => t.url.startsWith(plan.url.split("?")[0]) && t.webSocketDebuggerUrl);
    }, "browser target", 30000);

    cdp = cdpSocket(target.webSocketDebuggerUrl);
    await cdp.opened;
    await cdp.command("Runtime.enable");
    await cdp.command("Log.enable");
    await cdp.command("Page.enable");
    await cdp.command("Page.bringToFront");

    async function canvasRect() {
      const r = await cdp.command("Runtime.evaluate", {
        expression: `(() => { const c = document.querySelector('#unity-canvas'); if (!c) return null; const b = c.getBoundingClientRect(); return { x: b.x, y: b.y, w: b.width, h: b.height }; })()`,
        returnByValue: true,
      });
      return r.result.value;
    }

    for (const step of plan.steps) {
      if (step.wait) {
        await new Promise((r) => setTimeout(r, step.wait));
      } else if (step.shot) {
        const shot = await cdp.command("Page.captureScreenshot", { format: "png" }, 90000);
        fs.writeFileSync(path.join(outDir, step.shot + ".png"), Buffer.from(shot.data, "base64"));
        console.log("saved shot:", step.shot);
      } else if (step.click) {
        const rect = await canvasRect();
        if (!rect) throw new Error("no canvas for click");
        const x = rect.x + rect.w * step.click[0];
        const y = rect.y + rect.h * step.click[1];
        await cdp.command("Input.dispatchMouseEvent", { type: "mouseMoved", x, y, button: "none" });
        await new Promise((r) => setTimeout(r, 150));
        await cdp.command("Input.dispatchMouseEvent", { type: "mousePressed", x, y, button: "left", clickCount: 1 });
        await new Promise((r) => setTimeout(r, 120));
        await cdp.command("Input.dispatchMouseEvent", { type: "mouseReleased", x, y, button: "left", clickCount: 1 });
        console.log(`clicked (${step.click[0]}, ${step.click[1]}) -> px (${Math.round(x)}, ${Math.round(y)}) [canvas ${JSON.stringify(rect)}]`);
      } else if (step.key) {
        const keyMap = { Space: 32 };
        await cdp.command("Input.dispatchKeyEvent", { type: "keyDown", windowsVirtualKeyCode: keyMap[step.key] || 32, key: step.key === "Space" ? " " : step.key, code: step.key });
        await cdp.command("Input.dispatchKeyEvent", { type: "keyUp", windowsVirtualKeyCode: keyMap[step.key] || 32, key: step.key === "Space" ? " " : step.key, code: step.key });
        console.log("key:", step.key);
      }
    }

    fs.writeFileSync(path.join(outDir, "console.log"), cdp.console_.join("\n"));
    console.log("console lines:", cdp.console_.length);
  } finally {
    if (cdp) cdp.close();
    if (browser && browser.pid) spawnSync("taskkill", ["/PID", String(browser.pid), "/T", "/F"], { stdio: "ignore" });
  }
}

main().catch((e) => { console.error(e.stack || e.message); process.exit(1); });
