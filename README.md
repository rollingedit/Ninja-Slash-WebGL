# Ninja Slash WebGL Port

A modern WebGL port of **Ninja Slash**, the Unity endless runner originally released on Kongregate by **doopop**.

Playable demo: https://ninjaslash.pages.dev/

This project takes an original Unity Web Player-era game build and ports it to a browser-native WebGL deployment. It is not a wrapper around an old plugin build: the game was recovered into Unity, repaired for a modern editor/runtime, rebuilt for WebGL, and packaged as a static browser game with custom loading behavior.

## Original Game

**Ninja Slash** is a cartoon-style Unity3D endless runner where the player controls a ninja fighting zombies, collecting coins, completing missions, buying upgrades, and moving through randomized stage patterns.

The Kongregate Wiki lists the game as:

- Title: **Ninja Slash**
- Author: **doopop**
- Genre: **Action**
- Gameplay style: **Endless Runner**
- Published: **2013-04-09**
- Technology: **Unity3D**

Reference: https://kongregate.fandom.com/wiki/Ninja_Slash

## Technical Porting Work

This port required a full recovery and compatibility pass across project data, runtime code, UI behavior, rendering, browser integration, and the final static deployment package.

- Recovered the original Unity Web Player artifact into a Unity project with [AssetRipper](https://github.com/AssetRipper/AssetRipper).
- Migrated the recovered project into a modern Unity editor capable of WebGL output.
- Repaired recovered C# scripts for modern Unity API compatibility.
- Replaced discontinued Kongregate, browser-plugin, ad, mobile-store, and platform service calls with WebGL-safe behavior.
- Patched keyboard, mouse, and touch input paths for browser play.
- Repaired menu and UI flow across title, restart, shop, upgrades, dojo, roster, missions, career, options, and pause/settings screens.
- Preserved recovered scenes, prefabs, stage data, character panels, enemy behavior, effects, missions, upgrades, and UI assets where possible.
- Repaired recovered asset-data corruption that prevented key prefabs and tags from importing or resolving correctly in modern Unity.
- Restored restart/session cleanup so old generated world blocks and enemies do not persist into later runs.
- Adjusted recovered collision/trigger data where modern Unity physics no longer matched the original Unity Web Player runtime.
- Fixed WebGL rendering regressions from the recovery process, including material/shader issues and missing visual effects.
- Restored shader behavior needed for original trail/effect tinting and fading.
- Replaced dead app-store links with a WebGL port banner and repository link.
- Replaced visible Unity template splash/loading branding with RollingEdit WebGL Port branding.
- Built a local static WebGL package in `final/`.
- Built a static deployment package in `NinjaSlash/` with split `.data` and `.wasm` loading for static host file-size limits.
- Removed compressed Unity artifact handling that caused incorrect WebAssembly MIME/content loading on static hosts.

Much of the work was compatibility repair rather than feature recreation: recovered prefabs, tags, serialized UI references, object pools, trigger volumes, shaders, and WebGL startup files all needed targeted fixes before the game behaved like the original build in a current browser.

## Intentional Gameplay Changes

The current public build includes two intentional gameplay modifications:

- All playable characters are unlocked.
- Coin purchase behavior is modified so purchases can add coins back to the inventory instead of subtracting them.

These are port modifications and are not original Ninja Slash behavior.

## Repository Layout

```text
NinjaSlash/                 Static deploy package.
final/                      Local static WebGL package.
build_webgl/                Unity WebGL build output.
recovered_project/          Recovered Unity project and compatibility patches.
originals/                  Source artifacts and manifests, when present.
scripts/                    Verification, serving, and helper scripts.
docs/                       Port notes and supporting assets.
unity_stubs/                Compatibility stubs for recovered Unity scripts.
```

## Run Locally

Serve the local package over HTTP:

```bash
python -m http.server 8123 --bind 127.0.0.1 --directory final
```

Open:

```text
http://127.0.0.1:8123/
```

Unity WebGL should be served over HTTP. Opening `index.html` directly through `file://` is not a reliable test.

## Static Deployment

Deploy the contents of:

```text
NinjaSlash/
```

The deploy package is static and does not require a backend server. Large Unity WebGL files are split and reassembled in the browser so the build can fit static host file-size limits.

Expected deploy properties:

- no `.gz` files required
- no single file over common static host file-size limits
- `Build/NinjaSlash_AssetRipper.wasm.part0` begins with WebAssembly magic bytes `00 61 73 6d`

## License And Rights

Original Ninja Slash game content remains owned by its original rights holders. This includes original names, characters, artwork, models, textures, animations, audio, scenes, prefabs, trademarks, and other recovered Unity-exported content.

The GPLv3 license for this repository applies only to original work created for this port, including:

- recovery scripts
- compatibility stubs
- WebGL loader/template changes
- documentation
- build and deployment glue
- source patches written for the port
- replacement WebGL port branding assets
- other new code or assets created for this repository

The GPLv3 license does not grant rights to original Ninja Slash content or other third-party intellectual property.

## Attribution

Ninja Slash was created by **doopop** and published on Kongregate in 2013. This repository is an independent WebGL preservation and compatibility port.
