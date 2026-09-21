using HarmonyLib;
using NoFogBruh.Features;

namespace NoFogBruh.Patches;

public class FejdStartupPatches
{

    [HarmonyPatch(typeof(FejdStartup), nameof(FejdStartup.Awake))]
    [HarmonyAfter("org.bepinex.helpers.LocalizationManager")]
    [HarmonyBefore("org.bepinex.helpers.ItemManager")]
    public static class FejdStartupAwakePatch
    {
        static void Prefix()
        {
            FogTargetManager.Reset();
            NoFogBruh.Waiter.ValheimIsAwake(true);
        }
    }

}