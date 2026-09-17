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
