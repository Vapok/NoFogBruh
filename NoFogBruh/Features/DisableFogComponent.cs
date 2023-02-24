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
    public static ConfigEntry<bool> EnableFog { get; private set;}
    public static ConfigEntry<bool> EnableAmbientComponent { get; private set;}
    public static ConfigEntry<bool> EnableGroundMist { get; private set;}
    public static ConfigEntry<bool> EnableFogClouds { get; private set;}
    public static ConfigEntry<bool> EnableFogOceanMist { get; private set;}
    public static ConfigEntry<bool> EnableDistantFog { get; private set;}
    public static ConfigEntry<bool> EnableMistEmitter { get; private set;}
    
    static DisableFogComponent()
    {
        ConfigRegistry.Waiter.StatusChanged += (_, _) => RegisterConfigurationFile();
    }

    private static void RegisterConfigurationFile()
    {
        EnableFog = ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Fog Component", false,
            new ConfigDescription("When enabled, will enable fog as the Allfather imagined. When disabled, there will be no fog.",
                null,
                new ConfigurationManagerAttributes { Order = 1 }));
        EnableAmbientComponent = ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Ambient Occlusion Component", false,
            new ConfigDescription("When enabled, will enable fog as the Allfather imagined. When disabled, there will be no fog.",
                null,
                new ConfigurationManagerAttributes { Order = 2 }));
        EnableGroundMist = ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Ground Mist", false,
            new ConfigDescription("When enabled, will enable fog as the Allfather imagined. When disabled, there will be no fog.",
                null,
                new ConfigurationManagerAttributes { Order = 3 }));
        EnableFogClouds = ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Fog Clouds", false,
            new ConfigDescription("When enabled, will enable fog as the Allfather imagined. When disabled, there will be no fog.",
                null,
                new ConfigurationManagerAttributes { Order = 4 }));
        EnableFogOceanMist = ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Ocean Mist", false,
            new ConfigDescription("When enabled, will enable fog as the Allfather imagined. When disabled, there will be no fog.",
                null,
                new ConfigurationManagerAttributes { Order = 5 }));
        EnableDistantFog = ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Distant Fog", false,
            new ConfigDescription("When enabled, will enable fog as the Allfather imagined. When disabled, there will be no fog.",
                null,
                new ConfigurationManagerAttributes { Order = 6 }));
        EnableMistEmitter = ConfigSyncBase.UnsyncedConfig("Fog Settings", "Enable Distant Fog", false,
            new ConfigDescription("When enabled, will enable fog as the Allfather imagined. When disabled, there will be no fog.",
                null,
                new ConfigurationManagerAttributes { Order = 7 }));
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
                env.m_fogDensityNight = 0f;
                env.m_fogDensityMorning = 0f;
                env.m_fogDensityDay = 0f;
                env.m_fogDensityEvening = 0f;
            }

            if (EnvMan.instance == null) return;
            GameObject.Find("_GameMain/_Environment/FollowPlayer/GroundMist").SetActive(EnableGroundMist.Value);
            GameObject.Find("_GameMain/_Environment/FollowPlayer/FogClouds").SetActive(EnableFogClouds.Value);
            GameObject.Find("_GameMain/_Environment/OceanMist").SetActive(EnableFogOceanMist.Value);
            GameObject.Find("_GameMain/_Environment/Distant_fog_planes").SetActive(EnableDistantFog.Value);
        }
    }

    [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.SetParticleArrayEnabled))]
    public static class EnvManSetParticleArrayEnabledPatch
    {
        private static void Postfix(ref MistEmitter __instance, GameObject[] psystems, bool enabled)
        {
            // Disable Mist clouds, does not work on Console Commands (env Misty) but should work in the regular game.
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