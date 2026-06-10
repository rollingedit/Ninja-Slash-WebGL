#!/usr/bin/env node
"use strict";

const fs = require("node:fs");
const http = require("node:http");
const os = require("node:os");
const path = require("node:path");
const { spawn, spawnSync } = require("node:child_process");

const root = path.resolve(__dirname, "..");
const port = Number(process.env.NINJA_SLASH_SMOKE_PORT || (8090 + Math.floor(Math.random() * 300)));
const cdpPort = Number(process.env.NINJA_SLASH_CDP_PORT || (9333 + Math.floor(Math.random() * 300)));
const gameUrl = `http://127.0.0.1:${port}/`;
const buildDir = path.join(root, "build_webgl", "NinjaSlash");
const serverScript = path.join(root, "scripts", "local_server.py");
const profileDir = path.join(root, "Temp", `browser-smoke-${Date.now()}`);

function requestJson(url) {
  return new Promise((resolve, reject) => {
    const req = http.get(url, (res) => {
      let body = "";
      res.setEncoding("utf8");
      res.on("data", (chunk) => { body += chunk; });
      res.on("end", () => {
        if (res.statusCode < 200 || res.statusCode >= 300) {
          reject(new Error(`${url} returned HTTP ${res.statusCode}: ${body.slice(0, 200)}`));
          return;
        }
        try {
          resolve(JSON.parse(body));
        } catch (err) {
          reject(err);
        }
      });
    });
    req.on("error", reject);
    req.setTimeout(1000, () => req.destroy(new Error(`Timed out requesting ${url}`)));
  });
}

function requestText(url) {
  return new Promise((resolve, reject) => {
    const req = http.get(url, (res) => {
      let body = "";
      res.setEncoding("utf8");
      res.on("data", (chunk) => { body += chunk; });
      res.on("end", () => {
        if (res.statusCode < 200 || res.statusCode >= 300) {
          reject(new Error(`${url} returned HTTP ${res.statusCode}`));
          return;
        }
        resolve(body);
      });
    });
    req.on("error", reject);
    req.setTimeout(1000, () => req.destroy(new Error(`Timed out requesting ${url}`)));
  });
}

async function waitFor(fn, label, timeoutMs = 15000) {
  const start = Date.now();
  let lastError;
  while (Date.now() - start < timeoutMs) {
    try {
      const value = await fn();
      if (value) return value;
    } catch (err) {
      lastError = err;
    }
    await new Promise((resolve) => setTimeout(resolve, 250));
  }
  throw new Error(`${label} did not become ready${lastError ? `: ${lastError.message}` : ""}`);
}

async function removeWithRetry(target) {
  for (let i = 0; i < 20; i++) {
    try {
      fs.rmSync(target, { recursive: true, force: true });
      return true;
    } catch {
      await new Promise((resolve) => setTimeout(resolve, 250));
    }
  }
  return false;
}

function killTree(child) {
  if (!child || child.killed || !child.pid) return;
  if (process.platform === "win32") {
    spawnSync("taskkill", ["/PID", String(child.pid), "/T", "/F"], { stdio: "ignore" });
  } else {
    child.kill("SIGTERM");
  }
}

function findBrowser() {
  const candidates = [
    process.env.CHROME_PATH,
    "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
    "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe",
    "C:\\Program Files\\Microsoft\\Edge\\Application\\msedge.exe",
    "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe"
  ].filter(Boolean);
  const found = candidates.find((candidate) => fs.existsSync(candidate));
  if (!found) throw new Error("No Chrome or Edge executable found.");
  return found;
}

function cdpSocket(wsUrl) {
  let nextId = 1;
  const pending = new Map();
  const events = [];
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
    if (msg.method) events.push(msg);
  });

  const opened = new Promise((resolve, reject) => {
    socket.addEventListener("open", resolve, { once: true });
    socket.addEventListener("error", reject, { once: true });
  });

  function command(method, params = {}) {
    const id = nextId++;
    socket.send(JSON.stringify({ id, method, params }));
    return new Promise((resolve, reject) => {
      pending.set(id, { resolve, reject });
      setTimeout(() => {
        if (pending.has(id)) {
          pending.delete(id);
          reject(new Error(`${method} timed out`));
        }
      }, 5000);
    });
  }

  return { opened, command, close: () => socket.close(), events };
}

async function main() {
  if (!fs.existsSync(path.join(buildDir, "index.html"))) {
    throw new Error(`Missing build output at ${buildDir}`);
  }

  fs.mkdirSync(profileDir, { recursive: true });
  const server = spawn("python", [serverScript, buildDir, String(port)], {
    cwd: root,
    stdio: ["ignore", "pipe", "pipe"],
    windowsHide: true
  });
  const serverLogs = [];
  server.stdout.on("data", (chunk) => serverLogs.push(chunk.toString()));
  server.stderr.on("data", (chunk) => serverLogs.push(chunk.toString()));

  let browser;
  let cdp;
  try {
    await waitFor(() => requestText(`http://127.0.0.1:${port}/`).then(Boolean), "local server");

    const browserPath = findBrowser();
    browser = spawn(browserPath, [
      `--remote-debugging-port=${cdpPort}`,
      "--remote-allow-origins=*",
      "--no-first-run",
      "--disable-default-apps",
      "--disable-background-networking",
      "--disable-background-timer-throttling",
      "--disable-backgrounding-occluded-windows",
      "--disable-renderer-backgrounding",
      "--window-size=1280,720",
      `--user-data-dir=${profileDir}`,
      gameUrl
    ], {
      cwd: root,
      stdio: ["ignore", "pipe", "pipe"],
      windowsHide: false
    });

    const browserLogs = [];
    browser.stdout.on("data", (chunk) => browserLogs.push(chunk.toString()));
    browser.stderr.on("data", (chunk) => browserLogs.push(chunk.toString()));

    const target = await waitFor(async () => {
      const tabs = await requestJson(`http://127.0.0.1:${cdpPort}/json/list`);
      return tabs.find((tab) => tab.url.startsWith(`http://127.0.0.1:${port}/`) && tab.webSocketDebuggerUrl);
    }, "browser target", 20000);

    cdp = cdpSocket(target.webSocketDebuggerUrl);
    await cdp.opened;
    await cdp.command("Runtime.enable");
    await cdp.command("Page.enable");
    await cdp.command("Page.bringToFront");

    let lastObserved = null;
    const menuState = await waitFor(async () => {
      const evalResult = await cdp.command("Runtime.evaluate", {
        expression: `(() => ({
          webgl: document.body.dataset.webgl,
          mode: document.body.dataset.mode,
          originalAssets: Number(document.body.dataset.originalAssets || 0),
          canvasPixels: (() => {
            const c = document.getElementById("game");
            return c ? [c.width, c.height] : [0, 0];
          })(),
          visiblePanel: [...document.querySelectorAll(".panel:not(.hidden)")].map((el) => el.id),
          errors: window.__ninjaSlashErrors || []
        }))()`,
        returnByValue: true
      });
      const value = evalResult.result.value;
      lastObserved = value;
      if (value.webgl === "ready" && value.mode === "menu" && value.originalAssets >= 8 && value.canvasPixels[0] > 0 && value.canvasPixels[1] > 0 && value.visiblePanel.includes("menu")) {
        return value;
      }
      return null;
    }, "menu", 10000).catch((err) => {
      err.message = `${err.message}; last observed state: ${JSON.stringify(lastObserved)}`;
      throw err;
    });

    await cdp.command("Runtime.evaluate", { expression: `document.getElementById("shopBtn").click()` });
    const shopState = await cdp.command("Runtime.evaluate", {
      expression: `(() => [...document.querySelectorAll(".panel:not(.hidden)")].map((el) => el.id))()`,
      returnByValue: true
    });
    if (!shopState.result.value.includes("shop")) throw new Error(`Shop did not open: ${JSON.stringify(shopState.result.value)}`);

    await cdp.command("Runtime.evaluate", { expression: `document.getElementById("shopBack").click(); document.getElementById("missionsBtn").click()` });
    const missionState = await cdp.command("Runtime.evaluate", {
      expression: `(() => [...document.querySelectorAll(".panel:not(.hidden)")].map((el) => el.id))()`,
      returnByValue: true
    });
    if (!missionState.result.value.includes("missions")) throw new Error(`Missions did not open: ${JSON.stringify(missionState.result.value)}`);

    await cdp.command("Runtime.evaluate", { expression: `document.getElementById("missionsBack").click(); document.getElementById("playBtn").click()` });
    const result = await waitFor(async () => {
      const evalResult = await cdp.command("Runtime.evaluate", {
        expression: `(() => ({
          webgl: document.body.dataset.webgl,
          mode: document.body.dataset.mode,
          originalAssets: Number(document.body.dataset.originalAssets || 0),
          score: Number(document.body.dataset.score || 0),
          entities: Number(document.body.dataset.entities || 0),
          visiblePanel: [...document.querySelectorAll(".panel:not(.hidden)")].map((el) => el.id),
          errors: window.__ninjaSlashErrors || []
        }))()`,
        returnByValue: true
      });
      const value = evalResult.result.value;
      lastObserved = value;
      if (value.webgl === "ready" && value.mode === "running" && value.originalAssets >= 8 && value.score > 0 && value.visiblePanel.length === 0 && value.errors.length === 0) {
        return value;
      }
      return null;
    }, "game loop", 10000).catch((err) => {
      err.message = `${err.message}; last observed state: ${JSON.stringify(lastObserved)}`;
      throw err;
    });

    await cdp.command("Input.dispatchKeyEvent", { type: "keyDown", windowsVirtualKeyCode: 39, key: "ArrowRight", code: "ArrowRight" });
    await cdp.command("Input.dispatchKeyEvent", { type: "keyUp", windowsVirtualKeyCode: 39, key: "ArrowRight", code: "ArrowRight" });
    await cdp.command("Input.dispatchKeyEvent", { type: "keyDown", windowsVirtualKeyCode: 32, key: " ", code: "Space" });
    await cdp.command("Input.dispatchKeyEvent", { type: "keyUp", windowsVirtualKeyCode: 32, key: " ", code: "Space" });

    const afterInput = await cdp.command("Runtime.evaluate", {
      expression: `(() => ({ mode: document.body.dataset.mode, score: Number(document.body.dataset.score || 0), webgl: document.body.dataset.webgl }))()`,
      returnByValue: true
    });
    const screenshot = await cdp.command("Page.captureScreenshot", { format: "png", captureBeyondViewport: false });
    const screenshotPath = path.join(root, "build_webgl", "NinjaSlash", "browser_smoke.png");
    fs.writeFileSync(screenshotPath, Buffer.from(screenshot.data, "base64"));

    console.log(JSON.stringify({
      ok: true,
      browserPath,
      url: gameUrl,
      menu: menuState,
      run: result,
      afterInput: afterInput.result.value,
      screenshot: path.relative(root, screenshotPath)
    }, null, 2));
  } catch (err) {
    const logs = serverLogs.join("").trim();
    throw new Error(`${err.message}${logs ? `\nServer logs:\n${logs}` : ""}`);
  } finally {
    if (cdp) cdp.close();
    killTree(browser);
    if (!server.killed) server.kill();
    await new Promise((resolve) => setTimeout(resolve, 800));
    const removed = await removeWithRetry(profileDir);
    if (!removed) {
      console.error(`Warning: could not remove temporary browser profile ${profileDir}`);
    }
  }
}

main().catch((err) => {
  console.error(err.stack || err.message);
  process.exit(1);
});
