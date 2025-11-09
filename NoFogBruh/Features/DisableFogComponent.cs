using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BepInEx.Configuration;
using HarmonyLib;
using NoFogBruh.Configuration;
using UnityEngine;
using UnityEngine.PostProcessing;
using Vapok.Common.Managers.Configuration;
using Vapok.Common.Shared;

namespace NoFogBruh.Features;

public class DisableFogComponent
{
    public static bool FeatureInitialized = false;
    public static ConfigEntry<bool> EnableFog;
    public static ConfigEntry<bool> EnableAmbientComponent;
    public static ConfigEntry<bool> EnableGroundMist;
    public static ConfigEntry<bool> EnableFogClouds;
    public static ConfigEntry<bool> EnableFogOceanMist;
    public static ConfigEntry<bool> EnableDistantFog;
    public static ConfigEntry<bool> EnableMistEmitter;
    public static ConfigEntry<bool> EnableInteriorDust;
    public static ConfigEntry<bool> EnableMountainCaveMist;
    public static ConfigEntry<bool> EnableMistlandsMist;
    public static ConfigEntry<bool> EnableAshlandsMist;
    public static ConfigEntry<bool> EnableAshlandsFaderFx;

    static DisableFogComponent()
    {
        ConfigRegistry.Waiter.StatusChanged += (_, _) => RegisterConfigurationFile();
    }

    private static void RegisterConfigurationFile()
    {
        ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Fog Component", false,
            new ConfigDescription("When enabled, will enable fog as the Allfather imagined. When disabled, there will be no fog.",
                null,
                new ConfigurationManagerAttributes { Order = 1 }),ref EnableFog);
        
        ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Ambient Occlusion Component", false,
            new ConfigDescription("Enables the overall occlusion fog that gives Valheim it's soft glow",
                null,
                new ConfigurationManagerAttributes { Order = 2 }),ref EnableAmbientComponent);
        ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Ground Mist", false,
            new ConfigDescription("Enables Ground mist nearby the player",
                null,
                new ConfigurationManagerAttributes { Order = 3 }), ref EnableGroundMist);
        ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Fog Clouds", false,
            new ConfigDescription("Enables Fog Clouds",
                null,
                new ConfigurationManagerAttributes { Order = 4 }), ref EnableFogClouds);
        ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Ocean Mist", false,
            new ConfigDescription("Enables the mist when in Ocean",
                null,
                new ConfigurationManagerAttributes { Order = 5 }),ref EnableFogOceanMist);
        ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Distant Fog", false,
            new ConfigDescription("Enables Distant Fog visuals",
                null,
                new ConfigurationManagerAttributes { Order = 6 }), ref EnableDistantFog);
        ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Mist Emitters", false,
            new ConfigDescription("Enables Mist Emitters for Particle Mist",
                null,
                new ConfigurationManagerAttributes { Order = 7 }), ref EnableMistEmitter);
        ConfigSyncBase.SyncedConfig("Fog Settings", "Enable Troll Cave Mist", false,
            new ConfigDescription("Enables mist and fog on the interior parts of Troll Caves",
                null,
                new ConfigurationManagerAttributes { Order = 8 }), ref EnableInteriorDust);
        ConfigSyncBase.SyncedConfig("Fog Settings", "Enable Mountain Cave Mist", false,
            new ConfigDescription("Enables mist and fog on the interior parts of Mountains Caves",
                null,
                new ConfigurationManagerAttributes { Order = 9 }), ref EnableMountainCaveMist);
        ConfigSyncBase.SyncedConfig("Fog Settings", "Enable Mistlands Global Mist", false,
            new ConfigDescription("Enables magical mist in Mistlands",
                null,
                new ConfigurationManagerAttributes { Order = 10 }), ref EnableMistlandsMist);
        ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Ashlands Global Mist", false,
            new ConfigDescription("Enables smokey mist in Ashlands",
                null,
                new ConfigurationManagerAttributes { Order = 11 }), ref EnableAshlandsMist);
        ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Ashlands FaderFX", false,
            new ConfigDescription("Enables fader effects that occur in Ashlands causing a foggy occlusion",
                null,
                new ConfigurationManagerAttributes { Order = 12 }), ref EnableAshlandsFaderFx);
    }

    private static void  SetFogSetting(PostProcessingBehaviour instance, FogComponent fog)
    {
        if (EnableFog.Value)
        {
            instance.TryExecuteCommandBuffer(fog);
        }
        else
        {
            instance.RemoveCommandBuffer<FogModel>();
        }
    }

    private static void  SetAmbientSetting(PostProcessingBehaviour instance, AmbientOcclusionComponent fog)
    {
        if (EnableAmbientComponent.Value)
        {
            instance.TryExecuteCommandBuffer(fog);
        }
        else
        {
            instance.RemoveCommandBuffer<AmbientOcclusionModel>();
        }
    }

    [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.SetEnv))]
    public static class EnvManSetEnvPatch
    {
        private static void Prefix(ref EnvMan __instance, ref EnvSetup env)
        {
            if (!EnableFog.Value)
            {
                var localEnv = env;
                SafeExecute("Fog",() =>
                {
                    localEnv.m_fogDensityNight = 0f;
                    localEnv.m_fogDensityMorning = 0f;
                    localEnv.m_fogDensityDay = 0f;
                    localEnv.m_fogDensityEvening = 0f;
                });
                env = localEnv;
            }

            if (EnvMan.instance == null) return;
            SafeExecute("GroundMist",() =>GameObject.Find("_GameMain/_Environment/FollowPlayer/GroundMist").SetActive(EnableGroundMist.Value));
            SafeExecute("Mistlands_Globalmist",() =>GameObject.Find("_LocationList_Mistlands(Clone)/environment_effects/FollowPlayer/Mistlands_Globalmist").SetActive(EnableMistlandsMist.Value));
            SafeExecute("Mistlands_Globalmist",() =>GameObject.Find("_LocationList_Mistlands/environment_effects/FollowPlayer/Mistlands_Globalmist").SetActive(EnableMistlandsMist.Value));
            SafeExecute("InteriorDust",() =>GameObject.Find("_GameMain/_Environment/FollowPlayer/InteriorDust").SetActive(EnableInteriorDust.Value));
            SafeExecute("env_mountain_cave",() =>GameObject.Find("_LocationList_MountainCaves(Clone)/environment/followplayer/env_mountain_cave/mist").SetActive(EnableMountainCaveMist.Value));
            SafeExecute("env_mountain_cave_hildir",() =>GameObject.Find("_LocationList_MountainCaves(Clone)/environment/followplayer/env_mountain_cave_hildir/mist").SetActive(EnableMountainCaveMist.Value));
            SafeExecute("Mist",() =>GameObject.Find("_GameMain/_Environment/FollowPlayer/Mist").SetActive(EnableFog.Value));
            SafeExecute("FogClouds",() =>GameObject.Find("_GameMain/_Environment/FollowPlayer/FogClouds").SetActive(EnableFogClouds.Value));
            SafeExecute("OceanMist",() =>GameObject.Find("_GameMain/_Environment/OceanMist").SetActive(EnableFogOceanMist.Value));
            SafeExecute("Distant_fog_planes",() =>GameObject.Find("_GameMain/_Environment/Distant_fog_planes").SetActive(EnableDistantFog.Value));
            SafeExecute("Ashlands_Misty",() =>GameObject.Find("_LocationList_Ashlands(Clone)/environment_effects/FollowPlayer/Ashlands_Misty").SetActive(EnableAshlandsMist.Value));
            SafeExecute("Ashlands_FaderFX",() =>GameObject.Find("_LocationList_Ashlands(Clone)/environment_effects/FollowPlayer/Ashlands_FaderFX/").SetActive(EnableAshlandsFaderFx.Value));
        }
    }

    private static void SafeExecute(string name, Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
            NoFogBruh.Log.Debug($"Safe Execution Error Handled: for {name} - {ex.Message}\r\nStack Trace: {ex.StackTrace}");
        }
    }
    
    [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.SetParticleArrayEnabled))]
    public static class EnvManSetParticleArrayEnabledPatch
    {
        private static void Postfix(ref MistEmitter __instance, GameObject[] psystems, bool enabled)
        {
            if (EnableMistEmitter.Value)
            {
                foreach (GameObject gameObject in psystems)
                {
                    MistEmitter componentInChildren = gameObject.GetComponentInChildren<MistEmitter>();
                    if (componentInChildren)
                    {
                        componentInChildren.enabled = false;
                    }
                }
            }
        }
    }
    
    [HarmonyPatch(typeof(PostProcessingBehaviour), nameof(PostProcessingBehaviour.OnPreRender))]
    static class PostProcessingBehaviourPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var instrs = instructions.ToList();

            var counter = 0;

            CodeInstruction LogMessage(CodeInstruction instruction)
            {
                NoFogBruh.Log.Debug($"IL_{counter}: Opcode: {instruction.opcode} Operand: {instruction.operand}");
                return instruction;
            }

            var ambientComponentField = AccessTools.DeclaredField(typeof(PostProcessingBehaviour), "m_AmbientOcclusion");
            var fogComponentField = AccessTools.DeclaredField(typeof(PostProcessingBehaviour), "m_FogComponent");
            
            var skipstopA = 0;
            var partADone = false;
            var skipstopB = 0;
            

            for (int i = 0; i < instrs.Count; ++i)
            {
                if (!partADone)
                {
                    yield return LogMessage(instrs[i]);
                    counter++;

                    if (i > 6 && instrs[i].opcode == OpCodes.Call && instrs[i + 1].opcode == OpCodes.Ldarg_0 &&
                        instrs[i + 2].opcode == OpCodes.Ldarg_0 && instrs[i + 3].opcode == OpCodes.Ldfld &&
                        instrs[i + 3].operand.Equals(ambientComponentField) && instrs[i + 4].opcode == OpCodes.Call)
                    {
                        var ldlocInstruction = new CodeInstruction(OpCodes.Ldarg_0);
                        //Move Any Labels from the instruction position being patched to new instruction.
                        if (instrs[i].labels.Count > 0)
                            instrs[i].MoveLabelsTo(ldlocInstruction);

                        //Patch the ldloc_0 which is the argument of my method using local variable 0.
                        yield return LogMessage(ldlocInstruction);
                        counter++;

                        //Patch the ldloc_0 which is the argument of my method using local variable 0.
                        yield return LogMessage(new CodeInstruction(OpCodes.Ldarg_0));
                        counter++;
         
                        //Patch the ldloc_0 which is the argument of my method using local variable 0.
                        yield return LogMessage(new CodeInstruction(OpCodes.Ldfld,ambientComponentField));
                        counter++;
         
                        //Patch Calling Method
                        yield return LogMessage(new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(DisableFogComponent), nameof(SetAmbientSetting))));
                        counter++;
                        skipstopA = i + 4;

                        partADone = true;
                    }
                } else {
                    
                    if ((skipstopB == 0 && i > skipstopA ) || (i > skipstopB && skipstopB != 0))
                    {
                        yield return LogMessage(instrs[i]);
                        counter++;
                    }

                    if (i > 6 && instrs[i].opcode == OpCodes.Call && instrs[i+1].opcode == OpCodes.Ldarg_0 && instrs[i+2].opcode == OpCodes.Ldarg_0 && instrs[i+3].opcode == OpCodes.Ldfld && instrs[i+3].operand.Equals(fogComponentField) && instrs[i+4].opcode == OpCodes.Call)
                    {
                        var ldlocInstruction = new CodeInstruction(OpCodes.Ldarg_0);
                        
                        //Move Any Labels from the instruction position being patched to new instruction.
                        if (instrs[i].labels.Count > 0)
                            instrs[i].MoveLabelsTo(ldlocInstruction);
          
                        //Patch the ldloc_0 which is the argument of my method using local variable 0.
                        yield return LogMessage(ldlocInstruction);
                        counter++;
         
                        //Patch the ldloc_0 which is the argument of my method using local variable 0.
                        yield return LogMessage(new CodeInstruction(OpCodes.Ldarg_0));
                        counter++;
         
                        //Patch the ldloc_0 which is the argument of my method using local variable 0.
                        yield return LogMessage(new CodeInstruction(OpCodes.Ldfld,fogComponentField));
                        counter++;
         
                        //Patch Calling Method
                        yield return LogMessage(new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(DisableFogComponent), nameof(SetFogSetting))));
                        counter++;
                        skipstopB = i + 4;
                    }
                }
            }
        }
    }
}