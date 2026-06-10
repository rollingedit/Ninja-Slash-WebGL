# Ninja Slash Port Checklist

## Build identity

Record these in `PORT_LOG.md`:

- Artifact filename and acquisition source.
- SHA-256.
- File size.
- Unity serialized header/magic.
- Unity version detected by AssetRipper.
- AssetRipper log path.
- Unity editor version used for first successful import.
- Unity editor version used for final WebGL export.

## Artifact sources ranked

1. Flashpoint curation of `Ninja Slash` by Doopop / Kongregate.
2. Your own Unity Web Player browser cache from when the original site loaded.
3. Your own archived iOS IPA/app bundle.
4. Third-party hosted `.unity3d` files only as quarantined candidates; compare hash and audit scripts before trusting.

## AssetRipper export settings

- Export Unity project rather than loose assets.
- Preserve GUIDs when available.
- Export shaders/materials/textures/models/audio/animations/scenes.
- Set script output to the most complete C# level available.
- Keep the full AssetRipper log; missing MonoBehaviours become compile/runtime issues later.

## Unity version ladder

Use the lowest editor that opens the recovered project cleanly first, then upgrade in steps.

Recommended ladder for a 2013 Unity project:

1. Original/nearest Unity major reported by AssetRipper.
2. Unity 5.6.7f1 if you need a mature old WebGL-capable bridge.
3. Unity 2017.4.40f1 if you need late UnityScript support or pre-2018 API behavior.
4. Unity 2019.4.40f1 or 2020.3.48f1 for a stable WebGL LTS target if the project is fully C#.
5. Newer Unity only after the game is proven playable.

## Common API repairs

### Application.ExternalCall / ExternalEval

Old Web Player code often uses browser calls directly. Replace with:

```csharp
#if UNITY_WEBGL && !UNITY_EDITOR
[System.Runtime.InteropServices.DllImport("__Internal")]
private static extern void NS_LogEvent(string eventName, string payloadJson);
#endif
```

Then call a no-op fallback in editor and standalone builds.

### Kongregate statistics / badges

Keep the gameplay call sites but make them local no-ops or local storage events. Badges are not required for local play. Do not let a missing API throw.

### Facebook / ads / analytics

Remove hard dependencies. Replace startup initialization with stubs that return success and callbacks that do nothing. This avoids hangs on title screen or post-run screens.

### PlayerPrefs

Use PlayerPrefs for coins, upgrades, selected ninja, missions, settings, and high scores. For WebGL this persists in browser storage when served from the same localhost origin.

### Input

Make one central adapter:

- Left/right arrows and A/D: lane movement.
- Up/W/Space: jump.
- Down/S/Ctrl: slide/duck.
- Mouse drag/touch swipe: original mobile-style slashing/dodging.

Do not rewrite zombie logic. Only feed the original handlers.

## Originality audit hot spots

Search decompiled scripts for these terms:

```
coin money currency upgrade shop purchase cost mission zombie fatso lanky goliath berserker sweeper scroll revive facebook kongregate badge hacked cheat
```

Reject or patch-audit builds where:

- upgrade cost is subtracted with the wrong sign;
- shop purchase adds coins;
- collision/death handlers are bypassed;
- `PlayerPrefs` starts with artificial currency;
- mission thresholds are changed;
- ads/reward hooks mutate currency without user action.

## WebGL build settings

- Compression Format: Disabled for first local test; Brotli/Gzip later if server headers are correct.
- Exceptions: Full with stack traces for first debug build, then None/Explicit for release.
- Code Optimization: Speed for release.
- Data Caching: enable only after build is stable.
- Managed Stripping Level: Low initially; higher only after no missing-reflection issues.
- Run In Background: enabled.
- Decompression Fallback: enabled if using compressed builds and a simple server.

## Final acceptance test

A port is accepted only when all of these pass locally:

1. Loads from `http://127.0.0.1:<port>/` with no external network dependency.
2. Title/menu/shop/missions open without JavaScript errors.
3. New run starts, stage selection/rotation works, camera follows correctly.
4. Lane/jump/slide/slash input works on keyboard and pointer/touch.
5. All zombie classes spawn and can be defeated according to original weak-spot rules.
6. Hazards kill or damage correctly.
7. Coins/scrolls/upgrades persist after refresh.
8. No hacked economy behavior.
9. FPS is stable enough for runner timing.
10. Build folder can be zipped and moved to another machine, then served locally.
