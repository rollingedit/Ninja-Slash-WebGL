"use strict";
const http = require("node:http");
function getJson(url) {
  return new Promise((resolve, reject) => {
    http.get(url, (res) => {
      let b = "";
      res.on("data", (c) => (b += c));
      res.on("end", () => { try { resolve(JSON.parse(b)); } catch (e) { reject(e); } });
    }).on("error", reject);
  });
}
(async () => {
  const tabs = await getJson("http://127.0.0.1:9555/json/list");
  for (const t of tabs) console.log(t.type, "|", t.url.slice(0, 80), "|", t.title.slice(0, 60));
})();
