using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    
    // General
    public static ConfigEntry<bool> EnableFog;
    public static ConfigEntry<bool> EnableAmbientComponent;
    public static ConfigEntry<bool> EnableGroundMist;
    public static ConfigEntry<bool> EnableFogClouds;
    public static ConfigEntry<bool> EnableDistantFog;
    public static ConfigEntry<bool> EnableMistEmitter;

    // Ocean
    public static ConfigEntry<bool> EnableFogOceanMist;

    // Black Forest
    public static ConfigEntry<bool> EnableInteriorDust;

    // Mountain
    public static ConfigEntry<bool> EnableMountainCaveMist;
    public static ConfigEntry<bool> EnableSnowGlint;
    public static ConfigEntry<bool> EnableBlizzardMist;

    // Mistlands
    public static ConfigEntry<bool> EnableMistlandsMist;
    public static ConfigEntry<bool> EnableParticleMist;

    // Ashlands
    public static ConfigEntry<bool> EnableAshlandsMist;
    public static ConfigEntry<bool> EnableAshlandsFaderFx;

    // Deep North
    public static ConfigEntry<bool> EnableDeepNorthMist;
    public static ConfigEntry<bool> EnableDeepNorthFaderFx;

    static DisableFogComponent()
    {
        ConfigRegistry.Waiter.StatusChanged += (_, _) =>
        {
            RegisterConfigurationFile();
            FogTargetManager.Initialize();
        };
    }

    private static void RegisterConfigurationFile()
    {
        // General
        ConfigSyncBase.UnsyncedConfig("General", "Enable Fog Component", false,
            new ConfigDescription("When enabled, will enable fog as the Allfather imagined. When disabled, there will be no fog.",
                null,
                new ConfigurationManagerAttributes { Order = 1 }), ref EnableFog);
        
        ConfigSyncBase.UnsyncedConfig("General", "Enable Ambient Occlusion Component", false,
            new ConfigDescription("Enables the overall occlusion fog that gives Valheim it's soft glow",
                null,
                new ConfigurationManagerAttributes { Order = 2 }), ref EnableAmbientComponent);
        
        ConfigSyncBase.UnsyncedConfig("General", "Enable Ground Mist", false,
            new ConfigDescription("Enables Ground mist nearby the player",
                null,
                new ConfigurationManagerAttributes { Order = 3 }), ref EnableGroundMist);
        
        ConfigSyncBase.UnsyncedConfig("General", "Enable Fog Clouds", false,
            new ConfigDescription("Enables Fog Clouds",
                null,
                new ConfigurationManagerAttributes { Order = 4 }), ref EnableFogClouds);

        ConfigSyncBase.UnsyncedConfig("General", "Enable Distant Fog", false,
            new ConfigDescription("Enables Distant Fog visuals",
                null,
                new ConfigurationManagerAttributes { Order = 5 }), ref EnableDistantFog);

        ConfigSyncBase.UnsyncedConfig("General", "Enable Mist Emitters", false,
            new ConfigDescription("Enables Mist Emitters for Particle Mist",
                null,
                new ConfigurationManagerAttributes { Order = 6 }), ref EnableMistEmitter);

        // Ocean
        ConfigSyncBase.UnsyncedConfig("Ocean", "Enable Ocean Mist", false,
            new ConfigDescription("Enables the mist when in Ocean",
                null,
                new ConfigurationManagerAttributes { Order = 1 }), ref EnableFogOceanMist);

        // Black Forest
        ConfigSyncBase.SyncedConfig("Black Forest", "Enable Troll Cave Mist", false,
            new ConfigDescription("Enables mist and fog on the interior parts of Troll Caves",
                null,
                new ConfigurationManagerAttributes { Order = 1 }), ref EnableInteriorDust);

        // Mountain
        ConfigSyncBase.SyncedConfig("Mountain", "Enable Mountain Cave Mist", false,
            new ConfigDescription("Enables mist and fog on the interior parts of Mountains Caves",
                null,
                new ConfigurationManagerAttributes { Order = 1 }), ref EnableMountainCaveMist);

        ConfigSyncBase.UnsyncedConfig("Mountain", "Enable Snow Glint", false,
            new ConfigDescription("Enables blinding snow surface glint and specular reflections in Mountain snowfields",
                null,
                new ConfigurationManagerAttributes { Order = 2 }), ref EnableSnowGlint);

        ConfigSyncBase.UnsyncedConfig("Mountain", "Enable Snowstorm / Blizzard Fog", false,
            new ConfigDescription("Enables snowstorm, blizzard, and snow fog effects",
                null,
                new ConfigurationManagerAttributes { Order = 3 }), ref EnableBlizzardMist);

        // Mistlands
        ConfigSyncBase.SyncedConfig("Mistlands", "Enable Mistlands Global Mist", false,
            new ConfigDescription("Enables magical mist in Mistlands",
                null,
                new ConfigurationManagerAttributes { Order = 1 }), ref EnableMistlandsMist);

        ConfigSyncBase.SyncedConfig("Mistlands", "Enable Particle Mist", false,
            new ConfigDescription("Enables dynamic volumetric particle mist in mist areas",
                null,
                new ConfigurationManagerAttributes { Order = 2 }), ref EnableParticleMist);

        // Ashlands
        ConfigSyncBase.UnsyncedConfig("Ashlands", "Enable Ashlands Global Mist", false,
            new ConfigDescription("Enables smokey mist in Ashlands",
                null,
                new ConfigurationManagerAttributes { Order = 1 }), ref EnableAshlandsMist);

        ConfigSyncBase.UnsyncedConfig("Ashlands", "Enable Ashlands FaderFX", false,
            new ConfigDescription("Enables fader effects that occur in Ashlands causing a foggy occlusion",
                null,
                new ConfigurationManagerAttributes { Order = 2 }), ref EnableAshlandsFaderFx);

        // Deep North
        ConfigSyncBase.UnsyncedConfig("Deep North", "Enable Deep North Global Mist", false,
            new ConfigDescription("Enables mist and fader effects in Deep North",
                null,
                new ConfigurationManagerAttributes { Order = 1 }), ref EnableDeepNorthMist);

        ConfigSyncBase.UnsyncedConfig("Deep North", "Enable Deep North FaderFX", false,
            new ConfigDescription("Enables fader effects that occur in Deep North causing a foggy occlusion",
                null,
                new ConfigurationManagerAttributes { Order = 2 }), ref EnableDeepNorthFaderFx);
    }

    private static void SetFogSetting(PostProcessingBehaviour instance, FogComponent fog)
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

    private static void SetAmbientSetting(PostProcessingBehaviour instance, AmbientOcclusionComponent fog)
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
    private static class EnvManSetEnvPatch
    {
        private static void Postfix(EnvMan __instance, EnvSetup env)
        {
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
            {
                return;
            }

            if (!EnableFog.Value)
            {
                RenderSettings.fogDensity = 0f;
            }

            if (EnvMan.instance == null || env == null)
            {
                return;
            }

            FogTargetManager.OnSetEnv(env.m_envObject);
        }
    }

    [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.Update))]
    private static class EnvManUpdatePatch
    {
        private static readonly int _snowGlintStrengthId = Shader.PropertyToID("_SnowGlintStrength");

        private static void Postfix(EnvMan __instance)
        {
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
            {
                return;
            }

            if (!EnableSnowGlint.Value)
            {
                Shader.SetGlobalFloat(_snowGlintStrengthId, 0f);
            }
        }
    }

    [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.SetParticleArrayEnabled))]
    private static class EnvManSetParticleArrayEnabledPatch
    {
        private static void Postfix(GameObject[] psystems, bool enabled)
        {
            if (psystems == null) return;

            if (!EnableMistEmitter.Value)
            {
                foreach (GameObject gameObject in psystems)
                {
                    if (gameObject == null) continue;
                    MistEmitter componentInChildren = gameObject.GetComponentInChildren<MistEmitter>();
                    if (componentInChildren != null)
                    {
                        componentInChildren.enabled = false;
                    }
                }
            }

            if (!EnableDistantFog.Value)
            {
                foreach (GameObject gameObject in psystems)
                {
                    if (gameObject == null) continue;
                    DistantFogEmitter distantFog = gameObject.GetComponentInChildren<DistantFogEmitter>();
                    if (distantFog != null)
                    {
                        distantFog.SetEmit(false);
                        distantFog.enabled = false;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(ParticleMist), nameof(ParticleMist.Update))]
    private static class ParticleMistUpdatePatch
    {
        private static bool Prefix(ParticleMist __instance)
        {
            if (!EnableParticleMist.Value)
            {
                if (__instance.m_ps != null && __instance.m_ps.particleCount > 0)
                {
                    __instance.m_ps.Clear();
                }
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(DistantFogEmitter), nameof(DistantFogEmitter.Update))]
    private static class DistantFogEmitterUpdatePatch
    {
        private static bool Prefix(DistantFogEmitter __instance)
        {
            if (!EnableDistantFog.Value)
            {
                __instance.SetEmit(false);
                return false;
            }
            return true;
        }
    }
    
    [HarmonyPatch(typeof(PostProcessingBehaviour), nameof(PostProcessingBehaviour.OnPreRender))]
    private static class PostProcessingBehaviourPatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> instrs = instructions.ToList();
            FieldInfo ambientComponentField = AccessTools.DeclaredField(typeof(PostProcessingBehaviour), "m_AmbientOcclusion");
            FieldInfo fogComponentField = AccessTools.DeclaredField(typeof(PostProcessingBehaviour), "m_FogComponent");
            
            int skipstopA = 0;
            bool partADone = false;
            int skipstopB = 0;

            for (int i = 0; i < instrs.Count; ++i)
            {
                if (!partADone)
                {
                    yield return instrs[i];

                    if (i > 6 && instrs[i].opcode == OpCodes.Call && instrs[i + 1].opcode == OpCodes.Ldarg_0 &&
                        instrs[i + 2].opcode == OpCodes.Ldarg_0 && instrs[i + 3].opcode == OpCodes.Ldfld &&
                        instrs[i + 3].operand.Equals(ambientComponentField) && instrs[i + 4].opcode == OpCodes.Call)
                    {
                        CodeInstruction ldlocInstruction = new CodeInstruction(OpCodes.Ldarg_0);
                        if (instrs[i].labels.Count > 0)
                            instrs[i].MoveLabelsTo(ldlocInstruction);

                        yield return ldlocInstruction;
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldfld, ambientComponentField);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(DisableFogComponent), nameof(SetAmbientSetting)));
                        skipstopA = i + 4;
                        partADone = true;
                    }
                }
                else
                {
                    if ((skipstopB == 0 && i > skipstopA) || (i > skipstopB && skipstopB != 0))
                    {
                        yield return instrs[i];
                    }

                    if (i > 6 && instrs[i].opcode == OpCodes.Call && instrs[i + 1].opcode == OpCodes.Ldarg_0 &&
                        instrs[i + 2].opcode == OpCodes.Ldarg_0 && instrs[i + 3].opcode == OpCodes.Ldfld &&
                        instrs[i + 3].operand.Equals(fogComponentField) && instrs[i + 4].opcode == OpCodes.Call)
                    {
                        CodeInstruction ldlocInstruction = new CodeInstruction(OpCodes.Ldarg_0);
                        if (instrs[i].labels.Count > 0)
                            instrs[i].MoveLabelsTo(ldlocInstruction);
          
                        yield return ldlocInstruction;
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldfld, fogComponentField);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(typeof(DisableFogComponent), nameof(SetFogSetting)));
                        skipstopB = i + 4;
                    }
                }
            }
        }
    }
}
