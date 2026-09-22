# 2.0.9 - Internalized Library & Dependency Updates
* **Dependency Updates**:
  * Updated internalized `Vapok.Valheim.Common` to 3.17.1015.
  * Updated `JotunnLib` dependency to 2.30.2.
* **Compatibility Verification**:
  * Audited weather particle hooks and headless dedicated server safety.

# 2.0.8 - Performance Optimization & Frame Rate Fix
* **Eliminated Per-Frame Scene Traversal**:
  * Resolved critical frame rate regression where `EnvManSetEnvPatch.Postfix` executed 20 unconditional `GameObject.Find` calls per physics tick (50Hz / 1,000 traversals per second).
  * Implemented `FogTargetManager` with hierarchical transform resolution (`_environmentRoot.Find`) and direct `LocationList.GetAllLocationLists()` registry lookup.
  * Cached resolved native `GameObject` pointers to replace string path searches with $< 50\text{ ns}$ pointer and boolean state comparisons.
  * Tied state application to `ConfigEntry.SettingChanged` events and weather transition events (`env.m_envObject != _lastEnvObject`), with throttled 3-second background polling during world generation.
* **Snow Glint Property Optimization**:
  * Cached `_snowGlintStrengthId = Shader.PropertyToID("_SnowGlintStrength")` in `EnvManUpdatePatch.Postfix` to eliminate per-frame string property name hashing.
* **Dedicated Server & Headless Runtimes**:
  * Added headless detection (`SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null`) in `NoFogBruh.Awake` to immediately initialize `ConfigRegistry` and load server configuration on dedicated servers without waiting for `FejdStartup`.
  * Added headless bypass guards across `FogTargetManager` and `DisableFogComponent` patches to ensure headless servers never execute client-only visual lookups.
  * Added `FogTargetManager.Reset()` hook in `FejdStartupAwakePatch.Prefix` to clear cached scene references when players return to the main menu.
* **Code Architecture & Cleanup**:
  * Refactored Harmony patch classes and methods to `private static`.
  * Removed lazy `var` keywords and replaced with explicit types across all patched routines.
  * Added `FogTargetManager.Reset()` cleanup hook in `NoFogBruh.OnDestroy` to release references across scene lifecycles.

# 2.0.7 - Valheim 1.0.15 Alignment & Internalized Dependency Updates
* **Valheim 1.0.15 Alignment**:
  * Aligned publicized game assembly and UnityEngine references to Valheim 1.0.15.
  * Updated internalized `Vapok.Valheim.Common` dependency to 3.13.1015.
* **Stability & Localization**:
  * Synchronized all 35 game localizations for splash screen and configuration registry.
  * Audited environment fog suppression and particle mist controllers against game version 1.0.15.

# 2.0.6 - Splash Window Updates & Valheim 1.0.14 Alignment
* **Splash Window Updates**:
  * Updated telemetry default to unchecked on first launch (Opt-In).
  * Added Send Error Logs toggle (Opt-Out) to capture anonymous crash diagnostics and error reports.
  * Added in-game scrollable Privacy Policy overlay with responsive mouse wheel support.
  * Added interactive tooltip data disclaimers on checkbox hover.
* **Valheim 1.0.14 Alignment**:
  * Aligned publicized game assembly and UnityEngine references to Valheim 1.0.14.
  * Updated internalized  dependency to 3.12.1014.

# 2.0.5 - Jewelcrafting Font Compatibility
* **Compatibility Fix**: Fixed issue where Jewelcrafting packages its own font which was overriding part of a vanilla font, causing the Splash screen to appear blank.
* **Vapok.Common Dependency Bump**: Updated internalized dependency to `Vapok.Valheim.Common` 3.11.1012.

# 2.0.4 - Updated README with Telemetry Information
* **Documentation Update**: Updated the README.md with Anonymous Telemetry and Privacy section per request of mod stores.
* **Vapok.Common Dependency Bump**: Updated internalized dependency to `Vapok.Valheim.Common` 3.9.1012.

# 2.0.3 - Unified Splash Screen & Telemetry Controls
* **Unified Startup Splash Screen & Telemetry**:
  * Updated `Vapok.Valheim.Common` dependency reference to `v3.5.1012`.
  * Registered mod metadata with centralized `ModSplashManager`.
  * Added `ShowSplashOnStartup` and `Enable Anonymous Telemetry` configuration bindings to `ConfigRegistry`.

# 2.0.1 - Dependency & Compatibility Maintenance
* **Runtime & Dependency Updates**:
  * Synchronized package manifest and project references with Jotunn `2.30.0` and BepInEx `5.4.2350`.
  * Verified build pipeline and ILRepack bundling with `Vapok.Valheim.Common` `3.2.1012`.
* **Compatibility & Documentation**:
  * Validated scene fog suppression against current Valheim 1.0 builds.
  * Standardized mod documentation, changelog tiers, and Thunderstore release staging.

# 2.0.0 - Valheim 1.0 Release & Biome Categorization
* **Valheim 1.0 Compatibility & Core Updates**:
  * Updated assembly references for Valheim 1.0 (`1.0.12`), BepInEx 5.4.2350, and Jotunn 2.30.0.
  * Rebuilt on .NET Framework 4.8.
  * Bundled `Vapok.Valheim.Common` 3.2.1012 via ILRepack.
* **Biome-Specific Fog & Mist Systems**:
  * Added Deep North support with configuration toggles for Deep North Global Mist and `FaderFX` occlusion layers.
  * Implemented Snow Glint control to suppress specular surface reflections across Mountain and Deep North snowfields.
  * Added Blizzard & Snowstorm mist toggles to suppress dense storm particle emitter arrays during blizzard weather events.
  * Added support for suppressing dynamic volumetric particle mist (`ParticleMist`) and distant atmospheric fog emitters (`DistantFogEmitter`).
* **Harmony Patch & Performance Optimizations**:
  * Fixed a parameter type mismatch bug in the `EnvMan.SetParticleArrayEnabled` Harmony prefix patch.
  * Eliminated fixed-update exception overhead by replacing per-frame scene traversal with event-driven fog component caching.
  * Categorized all configuration bindings neatly by biome (General, Ocean, Black Forest, Mountain, Mistlands, Ashlands, Deep North).

# 1.1.6 - Dependency Maintenance
* Updated all package references and runtime dependencies.

# 1.1.5 - Mistlands Mist GameObject Path Correction
* Corrected GameObject transform path resolution for Mistlands Mist following vanilla Unity hierarchy changes.

# 1.1.4 - Null Reference Exception Trapping
* Added defensive null checks and debug logging around fog emitter lookups to prevent non-critical NREs during scene transitions.

# 1.1.3 - Biome Fog Settings (Troll Cave, Mistlands, Ashlands)
* Added toggles for Troll Cave ground mist, Mistlands cloud layers, and Ashlands atmospheric mist.
* Made select visibility settings ServerSync-controlled for dedicated server administration.

# 1.1.2 - Fog Element Expansion
* Expanded suppression rules to cover newly introduced atmospheric fog elements.

# 1.1.1 - Versioning & Package Updates
* Synchronized Thunderstore package dependencies and manifest versioning.

# 1.1.0 - Jotunn Migration & ServerSync Transition
* Migrated configuration sync from standalone ServerSync to Jotunn JVL framework.
* Updated for Valheim 0.221.4.

# 1.0.6 - Valheim 0.217.28 Maintenance
* Updated for Valheim 0.217.28.

# 1.0.5 - Valheim 0.217.19 Maintenance
* Updated for Valheim 0.217.19.

# 1.0.4 - Valheim 0.217.14 Maintenance
* Updated for Valheim 0.217.14.

# 1.0.3 - Valheim 0.216.9 Maintenance
* Updated for Valheim 0.216.9.

# 1.0.2 - Valheim 0.214.2 & BepInEx Updates
* Updated for Valheim 0.214.2 and BepInEx 5.4.21.

# 1.0.1 - Logging Cleanup
* Removed lingering debug warning logs from release builds.

# 1.0.0 - Initial Release of NoFogBruh
* Initial release of atmospheric fog and mist suppression mod for Valheim.
* Added toggles for Ambient Occlusion, Ground Mist, Fog Clouds, Ocean Mist, Distant Fog, and Mist Emitters.
