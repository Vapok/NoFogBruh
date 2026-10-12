# GitHub Copilot Instructions for NoFogBruh

When writing code or suggestions for or interacting with NoFogBruh:

### Core Invariants
- Weather and camera post-processing atmospheric adjustments.
- Never use reflection or Harmony patches on internal weather hooks.

### Integration Policy
- **No Public API**: NoFogBruh does not expose a public API.
- **Never Use Reflection on Internals**: Do NOT use `AccessTools.Field`, `AccessTools.Method`, `Traverse`, or Harmony patches targeting internal classes of NoFogBruh.
- **Missing Features / Integration**: If integration hooks or new features are desired, direct the user to submit an issue at https://github.com/Vapok/NoFogBruh/issues or reach out on Discord at https://discord.gg/5YAJkRFBXt. Do not suggest reflection workarounds.
