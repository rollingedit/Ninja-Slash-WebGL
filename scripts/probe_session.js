#!/usr/bin/env node
"use strict";
// Persistent probe session against the WebGL build.
// Commands:
//   node probe_session.js launch <url>     -- start browser (stays open)
//   node probe_session.js status           -- readyState + loading progress + last console lines
//   node probe_session.js shot <name>      -- screenshot to Temp/ui_session/<name>.png
//   node probe_session.js click <fx> <fy>  -- click canvas at fractional coords
//   node probe_session.js eval "<js>"      -- evaluate expression
//   node probe_session.js console          -- dump collected console buffer (browser-side ring)
//   node probe_session.js kill             -- kill the probe browser

const fs = require("node:fs");
const http = require("node:http");
const path = require("node:path");
const { spawn, spawnSync } = require("node:child_process");

const root = path.resolve(__dirname, "..");
const cdpPort = 9557;
const outDir = path.join(root, "Temp", "ui_session");
fs.mkdirSync(outDir, { recursive: true });
const profileDir = path.join(root, "Temp", "ui-session-profile");
const pidFile = path.join(outDir, "browser.pid");

function getJson(url) {
  return new Promise((resolve, reject) => {
    http.get(url, (res) => {
      let b = "";
      res.on("data", (c) => (b += c));
      res.on("end", () => { try { resolve(JSON.parse(b)); } catch (e) { reject(e); } });
    }).on("error", reject);
  });
}

function findBrowser() {
  const candidates = [
    process.env.CHROME_PATH,
    "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
    "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe",
    "C:\\Program Files\\Microsoft\\Edge\\Application\\msedge.exe",
  ].filter(Boolean);
  return candidates.find((c) => fs.existsSync(c));
}

async function attach() {
  const tabs = await getJson(`http://127.0.0.1:${cdpPort}/json/list`);
  const tab = tabs.find((t) => t.url.includes("127.0.0.1:8123") && t.webSocketDebuggerUrl);
  if (!tab) throw new Error("no game tab; tabs: " + tabs.map((t) => t.url).join(", "));
  let nextId = 1;
  const pending = new Map();
  const socket = new WebSocket(tab.webSocketDebuggerUrl);
  socket.addEventListener("message", (event) => {
    const msg = JSON.parse(event.data);
    if (msg.id && pending.has(msg.id)) {
      const { resolve, reject } = pending.get(msg.id);
      pending.delete(msg.id);
      if (msg.error) reject(new Error(JSON.stringify(msg.error)));
      else resolve(msg.result);
    }
  });
  await new Promise((res, rej) => {
    socket.addEventListener("open", res, { once: true });
    socket.addEventListener("error", rej, { once: true });
  });
  function command(method, params = {}, timeout = 120000) {
    const id = nextId++;
    socket.send(JSON.stringify({ id, method, params }));
    return new Promise((resolve, reject) => {
      pending.set(id, { resolve, reject });
      setTimeout(() => {
        if (pending.has(id)) { pending.delete(id); reject(new Error(method + " timed out")); }
      }, timeout);
    });
  }
  return { command, close: () => socket.close() };
}

async function evalJs(cdp, expression) {
  const r = await cdp.command("Runtime.evaluate", { expression, returnByValue: true });
  return r.result ? r.result.value : r;
}

(async () => {
  const cmd = process.argv[2];
  if (cmd === "launch") {
    const url = process.argv[3];
    const browser = spawn(findBrowser(), [
      `--remote-debugging-port=${cdpPort}`,
      "--remote-allow-origins=*",
      "--no-first-run",
      "--window-size=720,860",
      "--window-position=0,0",
      `--user-data-dir=${profileDir}`,
      url,
    ], { cwd: root, stdio: "ignore", windowsHide: false, detached: true });
    browser.unref();
    fs.writeFileSync(pidFile, String(browser.pid));
    // install console collector once page exists
    await new Promise((r) => setTimeout(r, 4000));
    const cdp = await attach();
    await cdp.command("Page.enable");
    await evalJs(cdp, `window.__logs = []; (function(){
      const orig = { log: console.log, warn: console.warn, error: console.error };
      for (const k of Object.keys(orig)) {
        console[k] = function(...a) { try { window.__logs.push(k + "| " + a.map(String).join(" ")); if (window.__logs.length > 3000) window.__logs.shift(); } catch(e){}; return orig[k].apply(console, a); };
      }
      window.addEventListener("error", (e) => window.__logs.push("uncaught| " + e.message));
    })()`);
    console.log("launched pid", browser.pid);
    cdp.close();
  } else if (cmd === "status") {
    const cdp = await attach();
    const v = await evalJs(cdp, `({ ready: document.readyState, logs: (window.__logs||[]).length, lastLogs: (window.__logs||[]).slice(-8) })`);
    console.log(JSON.stringify(v, null, 1));
    cdp.close();
  } else if (cmd === "shot") {
    const cdp = await attach();
    await cdp.command("Page.bringToFront");
    const shot = await cdp.command("Page.captureScreenshot", { format: "png" });
    const file = path.join(outDir, (process.argv[3] || "shot") + ".png");
    fs.writeFileSync(file, Buffer.from(shot.data, "base64"));
    console.log("saved", file);
    cdp.close();
  } else if (cmd === "click") {
    const fx = Number(process.argv[3]), fy = Number(process.argv[4]);
    const cdp = await attach();
    await cdp.command("Page.bringToFront");
    const rect = await evalJs(cdp, `(() => { const c = document.querySelector('#unity-canvas'); const b = c.getBoundingClientRect(); return { x: b.x, y: b.y, w: b.width, h: b.height }; })()`);
    const x = rect.x + rect.w * fx, y = rect.y + rect.h * fy;
    await cdp.command("Input.dispatchMouseEvent", { type: "mouseMoved", x, y, button: "none" });
    await new Promise((r) => setTimeout(r, 150));
    await cdp.command("Input.dispatchMouseEvent", { type: "mousePressed", x, y, button: "left", clickCount: 1 });
    await new Promise((r) => setTimeout(r, 120));
    await cdp.command("Input.dispatchMouseEvent", { type: "mouseReleased", x, y, button: "left", clickCount: 1 });
    console.log("clicked px", Math.round(x), Math.round(y), "canvas", JSON.stringify(rect));
    cdp.close();
  } else if (cmd === "eval") {
    const cdp = await attach();
    console.log(JSON.stringify(await evalJs(cdp, process.argv[3]), null, 1));
    cdp.close();
  } else if (cmd === "console") {
    const cdp = await attach();
    const v = await evalJs(cdp, `(window.__logs||[])`);
    fs.writeFileSync(path.join(outDir, "console.log"), (v || []).join("\n"));
    console.log((v || []).slice(-50).join("\n"));
    cdp.close();
  } else if (cmd === "key") {
    const name = process.argv[3];
    const map = { Up: [38, "ArrowUp"], Down: [40, "ArrowDown"], Left: [37, "ArrowLeft"], Right: [39, "ArrowRight"], Space: [32, " "] };
    const [vk, key] = map[name] || map.Space;
    const cdp = await attach();
    await cdp.command("Page.bringToFront");
    await cdp.command("Input.dispatchKeyEvent", { type: "keyDown", windowsVirtualKeyCode: vk, key, code: name === "Space" ? "Space" : "Arrow" + name });
    await new Promise((r) => setTimeout(r, 90));
    await cdp.command("Input.dispatchKeyEvent", { type: "keyUp", windowsVirtualKeyCode: vk, key, code: name === "Space" ? "Space" : "Arrow" + name });
    console.log("key", name);
    cdp.close();
  } else if (cmd === "reload") {
    const cdp = await attach();
    await cdp.command("Page.enable");
    await cdp.command("Page.addScriptToEvaluateOnNewDocument", { source: `window.__logs = []; (function(){
      const orig = { log: console.log, warn: console.warn, error: console.error };
      for (const k of Object.keys(orig)) {
        console[k] = function(...a) { try { window.__logs.push(k + "| " + a.map(String).join(" ")); if (window.__logs.length > 5000) window.__logs.shift(); } catch(e){}; return orig[k].apply(console, a); };
      }
      window.addEventListener("error", (e) => window.__logs.push("uncaught| " + e.message));
    })();` });
    await cdp.command("Page.reload", { ignoreCache: false });
    // stay attached until the new document exists with the hook installed
    for (let i = 0; i < 60; i++) {
      await new Promise((r) => setTimeout(r, 1000));
      try {
        const v = await evalJs(cdp, "typeof window.__logs");
        if (v === "object") { console.log("hook installed after", i + 1, "s"); break; }
      } catch (e) { /* renderer busy */ }
    }
    cdp.close();
  } else if (cmd === "kill") {
    if (fs.existsSync(pidFile)) {
      spawnSync("taskkill", ["/PID", fs.readFileSync(pidFile, "utf8").trim(), "/T", "/F"], { stdio: "ignore" });
      fs.rmSync(pidFile, { force: true });
    }
    console.log("killed");
  } else {
    console.log("unknown command");
    process.exit(1);
  }
  process.exit(0);
})().catch((e) => { console.error(e.message); process.exit(1); });
