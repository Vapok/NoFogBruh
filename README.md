# No Fog Bruh! by Vapok

**No Fog Bruh!** is a comprehensive environmental visibility and fog management mod for Valheim. It allows players and server administrators to selectively customize or completely disable all post-processing fog, distant fog planes, particle mist clouds, atmospheric occlusion, and environmental weather overlays.

Whether you're looking for crystal-clear long-distance visibility across mountains and oceans or looking to remove blinding snow glares and dense dungeon dust, **No Fog Bruh!** gives you total control on a per-biome basis.

---

## Features & Biome Breakdown

All settings can be toggled in real-time using the in-game [BepInEx Configuration Manager](https://github.com/BepInEx/BepInEx.ConfigurationManager).

### 🌐 General
* **Enable Fog Component**: Controls Valheim's global depth fog post-processing pipeline. When disabled, global distance fog density is set to zero.
* **Enable Ambient Occlusion Component**: Controls the soft ambient occlusion fog and glow pass.
* **Enable Ground Mist**: Controls atmospheric ground mist planes following the player.
* **Enable Fog Clouds**: Controls volumetric fog cloud clusters.
* **Enable Distant Fog**: Controls distant fog billboard emitters and horizon fog planes across terrain and water.
* **Enable Mist Emitters**: Controls environment-instantiated particle mist emitters.

### 🌊 Ocean
* **Enable Ocean Mist**: Controls sea mist and coastal water vapor particles.

### 🌲 Black Forest
* **Enable Troll Cave Mist**: Controls atmospheric interior dust and mist particles within Troll Caves. *(Server Syncable)*

### 🏔️ Mountain
* **Enable Mountain Cave Mist**: Controls dense mist within Mountain Caves and Frost Caves. *(Server Syncable)*
* **Enable Snow Glint**: Suppresses blinding specular snow surface glint and glare in snowfields.
* **Enable Snowstorm / Blizzard Fog**: Disables dense blizzard and snowstorm particle arrays around the player.

### 🌫️ Mistlands
* **Enable Mistlands Global Mist**: Controls global magical mist clouds in Mistlands locations. *(Server Syncable)*
* **Enable Particle Mist**: Controls dynamic volumetric mist particles interacting with Mister/Demister systems. *(Server Syncable)*

### 🔥 Ashlands
* **Enable Ashlands Global Mist**: Controls smoky mist and ash vapor effects across the Ashlands.
* **Enable Ashlands FaderFX**: Controls dark fader occlusion effects that obscure distant sightlines in Ashlands.

### ❄️ Deep North
* **Enable Deep North Global Mist**: Controls misty environmental effects across the Deep North.
* **Enable Deep North FaderFX**: Controls high-latitude fader occlusion effects causing foggy blindness.

---

## 🔒 Server Synchronization & Admin Enforcement

Certain gameplay-altering mist settings (such as **Mistlands Particle Mist**, **Mountain Cave Mist**, and **Troll Cave Mist**) support server-side synchronization:
* When connected to a dedicated server running **No Fog Bruh!**, server administrators can lock and enforce these settings to maintain gameplay balance.
* Client-side visual preferences (such as general depth fog and snow glint) remain fully customizable per player.

---

## 📦 Installation

### Automatic Installation (Recommended)
Install using your favorite mod manager (e.g., r2modman or Thunderstore Mod Manager).

### Manual Installation
1. Ensure [BepInEx Pack for Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/) and [Jötunn, the Valheim Library](https://valheim.thunderstore.io/package/ValheimModding/Jotunn/) are installed.
2. Download the latest release from [Thunderstore](https://valheim.thunderstore.io/package/Vapok/NoFogBruh/) or [GitHub Releases](https://github.com/Vapok/NoFogBruh/releases).
3. Extract `NoFogBruh.dll` into your `Valheim/BepInEx/plugins/` directory.

---

## 📋 Patch Notes
See the full changelog in [CHANGELOG.md](https://github.com/Vapok/NoFogBruh/blob/main/CHANGELOG.md).

---

## 👥 Credits & Contact

* **Mod Author**: [Vapok](https://github.com/Vapok)
* **Special Thanks**: Azumatt
* **Source Code**: [GitHub Repository](https://github.com/Vapok/NoFogBruh)
* **Discord Community**: [Vapok's Mod Community](https://discord.gg/5YAJkRFBXt)
