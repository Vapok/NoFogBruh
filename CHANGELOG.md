# 2.0.6 - Splash Window Updates & Valheim 1.0.14 Alignment
* **Splash Window Updates**:
  * Telemetry is now unchecked when first loaded (Opt-In visibility)
  * Added Send Error Logs (Opt-Out)
  * Privacy Policy is now available directly in-game
  * Added Data Disclaimers on hover over checkboxes for transparency on what data is sent
* **Valheim 1.0.14 Alignment**: Updated game assembly references and internalized  3.12.1014.

<details>
<summary><b>2.0 Changelog History (Valheim Release)</b> (<i>click to expand</i>)</summary>


### 2.0.5 - Jewelcrafting Font Compatibility
* Fixed: Jewelcrafting packages it's own font which was overriding part of a vanilla font, causing the Splash screen to appear blank.
### 2.0.4 - Updated README with Telemetry Information
* Updated the README.md with Anonymous Telemetry information per request of mod stores.

### 2.0.3 - Unified Splash Screen & Telemetry Controls
* **Unified Startup Splash Screen**: Integrated with a centralized startup splash screen.
  * Added configurable `Show on Game Startup` which can be enabled or disabled in the configuration file.
* **Anonymous Telemetry**: 
  * Added configurable `Enable Anonymous Telemetry` configuration which can be enabled or disabled in the configuration file.
    * Defaults to enabled with auto-opt-in on launch. Uncheck to Opt-Out
    * ANONYMOUS DATA ONLY - I track version number and usage data. No personal data is ever collected. For more information, see the [Privacy Policy](https://vapok.io/privacy-policy/).

### 2.0.1 - Dependency & Compatibility Maintenance
* **Dependency Updates**: Updated Jotunn and BepInEx runtime package bindings.
* **Compatibility Maintenance**: Verified compatibility against the latest Valheim 1.0 release.
* **Documentation Improvements**: Standardized README, user guides, and technical patch documentation.

### 2.0.0 - Valheim 1.0 Release & Biome Categorization
* **Valheim 1.0 Support**: Fully updated and verified for Valheim 1.0.
* **Deep North Support**: Added configuration toggles for Deep North Global Mist and occlusion effects.
* **Snow Glint Control**: Added toggle to suppress blinding snow surface glint and specular reflections across snow biomes.
* **Blizzard & Snowstorm Mist**: Added toggle to remove dense blizzard and snowstorm fog particle arrays.
* **Volumetric Particle Mist**: Added support for suppressing dynamic particle mist and distant fog emitters.
* **Configuration Reorganization**: Categorized all configuration options neatly by Biome (General, Ocean, Black Forest, Mountain, Mistlands, Ashlands, Deep North).
* **Performance Optimizations**: Eliminated fixed-update overhead and optimized fog component lookup routines.
* **Bug Fixes**: Fixed environment particle type handling.

</details>

<details>
<summary><b>1.0 Changelog History (Valheim Early Access)</b> (<i>click to expand</i>)</summary>

### 1.1.6 - Updating Dependencies
* Updated all dependencies to latest versions.

### 1.1.5 - Mistlands Mist Object Path Fix
* Corrected GameObject path for Mistlands Mist following game hierarchy changes.

### 1.1.4 - Error Handling
* Added defensive null checks around fog emitter lookups to prevent non-critical log messages.

### 1.1.3 - Biome Mist Settings
* Added toggles to disable Troll Cave Ground Mist, Mistlands Mist Clouds, and Ashlands atmospheric mist.
* Made select visibility settings ServerSync-controlled for dedicated server admins.

### 1.1.2 - Additional Fog Elements
* Added suppression support for additional fog elements introduced in Valheim updates.

### 1.1.1 - Package Version Maintenance
* Updated Thunderstore dependencies and versioning.

### 1.1.0 - Jotunn Migration
* Transitioned to Jotunn library and updated for Valheim 0.221.4.

### 1.0.6 - Valheim 0.217.28 Maintenance
* Updated for Valheim 0.217.28.

### 1.0.5 - Valheim 0.217.19 Maintenance
* Updated for Valheim 0.217.19.

### 1.0.4 - Valheim 0.217.14 Maintenance
* Updated for Valheim 0.217.14.

### 1.0.3 - Valheim 0.216.9 Maintenance
* Updated for Valheim 0.216.9.

### 1.0.2 - Valheim & BepInEx Updates
* Updated for Valheim 0.214.2 and BepInEx 5.4.21.

### 1.0.1 - Logging Cleanup
* Removed debug warning statements left in release builds.

### 1.0.0 - Initial Release of NoFogBruh
* Initial release of atmospheric fog and mist suppression mod.
* Added toggle controls for Fog, Ambient Occlusion, Ground Mist, Clouds, Ocean Mist, Distant Fog, and Mist Emitters.

</details>
