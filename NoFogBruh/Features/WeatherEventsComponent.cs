using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;
using NoFogBruh.Configuration;
using UnityEngine;
using Vapok.Common.Managers.Configuration;
using Vapok.Common.Shared;

namespace NoFogBruh.Features
{
    public class WeatherEventsComponent
    {
        public static bool FeatureInitialized = false;

        private const string SectionWorld = "Weather Events - World";
        private const string SectionBoss = "Weather Events - Boss";
        private const string SectionDungeon = "Weather Events - Dungeon";
        private const string SectionRaid = "Weather Events - Raid";

        public static ConfigEntry<bool> EnableMisty;
        public static ConfigEntry<bool> EnableDeepForestMist;
        public static ConfigEntry<bool> EnableRain;
        public static ConfigEntry<bool> EnableLightRain;
        public static ConfigEntry<bool> EnableThunderStorm;
        public static ConfigEntry<bool> EnableSwampRain;
        public static ConfigEntry<bool> EnableSnow;
        public static ConfigEntry<bool> EnableSnowStorm;
        public static ConfigEntry<bool> EnableTwilightSnow;
        public static ConfigEntry<bool> EnableTwilightSnowStorm;
        public static ConfigEntry<bool> EnableMistlandsRain;
        public static ConfigEntry<bool> EnableMistlandsThunder;
        public static ConfigEntry<bool> EnableAshlandsMisty;
        public static ConfigEntry<bool> EnableAshlandsAshrain;
        public static ConfigEntry<bool> EnableAshlandsCinderrain;
        public static ConfigEntry<bool> EnableAshlandsStorm;
        public static ConfigEntry<bool> EnableAshlandsMeteorshower;
        public static ConfigEntry<bool> EnableAshlandsSeastorm;
        public static ConfigEntry<bool> EnableAshrain;
        public static ConfigEntry<bool> EnableDarklandsDark;
        public static ConfigEntry<bool> EnableNofogts;

        public static ConfigEntry<bool> EnableEikthyr;
        public static ConfigEntry<bool> EnableEikthyrNoLightning;
        public static ConfigEntry<bool> EnableGDKing;
        public static ConfigEntry<bool> EnableBonemass;
        public static ConfigEntry<bool> EnableModer;
        public static ConfigEntry<bool> EnableGoblinKing;
        public static ConfigEntry<bool> EnableQueen;
        public static ConfigEntry<bool> EnableFader;
        public static ConfigEntry<bool> EnableDNBossroom;

        public static ConfigEntry<bool> EnableCrypt;
        public static ConfigEntry<bool> EnableSunkenCrypt;
        public static ConfigEntry<bool> EnableCaves;
        public static ConfigEntry<bool> EnableCavesHildir;
        public static ConfigEntry<bool> EnableCryptHildir;
        public static ConfigEntry<bool> EnableInfectedMine;

        public static ConfigEntry<bool> EnableGhosts;
        public static ConfigEntry<bool> EnableJotunInvasionMeadows;
        public static ConfigEntry<bool> EnableJotunInvasionBlackforest;
        public static ConfigEntry<bool> EnableJotunInvasionSwamp;
        public static ConfigEntry<bool> EnableJotunInvasionMountain;
        public static ConfigEntry<bool> EnableJotunInvasionPlains;
        public static ConfigEntry<bool> EnableJotunInvasionMistlands;
        public static ConfigEntry<bool> EnableMorkhalla;
        public static ConfigEntry<bool> EnableTheHollow;

        private static readonly Dictionary<string, ConfigEntry<bool>> _weatherConfigs = new Dictionary<string, ConfigEntry<bool>>(StringComparer.OrdinalIgnoreCase);
        private static bool _initialized;

        static WeatherEventsComponent()
        {
            ConfigRegistry.Waiter.StatusChanged += (_, _) =>
            {
                RegisterConfigurationFile();
            };
        }

        private static void RegisterConfigurationFile()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;

            // World / Biome Ambient Weather
            RegisterWeatherConfig(SectionWorld, "Misty", "Enable Misty Weather",
                "Enables dense fog weather in Meadows, Black Forest, Plains, and Ocean.", 1, ref EnableMisty);

            RegisterWeatherConfig(SectionWorld, "DeepForest Mist", "Enable Deep Forest Mist Weather",
                "Enables mist and fog weather in Black Forest.", 2, ref EnableDeepForestMist);

            RegisterWeatherConfig(SectionWorld, "Rain", "Enable Rain Weather",
                "Enables standard rain in Meadows, Black Forest, Plains, and Ocean.", 3, ref EnableRain);

            RegisterWeatherConfig(SectionWorld, "LightRain", "Enable Light Rain Weather",
                "Enables light rain and drizzle in Meadows, Plains, and Ocean.", 4, ref EnableLightRain);

            RegisterWeatherConfig(SectionWorld, "ThunderStorm", "Enable Thunderstorm Weather",
                "Enables heavy thunderstorms and sea storms in Meadows, Plains, and Ocean.", 5, ref EnableThunderStorm);

            RegisterWeatherConfig(SectionWorld, "SwampRain", "Enable Swamp Rain Weather",
                "Enables continuous rain and gloom in Swamp.", 6, ref EnableSwampRain);

            RegisterWeatherConfig(SectionWorld, "Snow", "Enable Snow Weather",
                "Enables snowfall in Mountain and Deep North.", 7, ref EnableSnow);

            RegisterWeatherConfig(SectionWorld, "SnowStorm", "Enable Snowstorm Weather",
                "Enables snowstorms and blizzards in Mountain and Deep North.", 8, ref EnableSnowStorm);

            RegisterWeatherConfig(SectionWorld, "Twilight_Snow", "Enable Twilight Snow Weather",
                "Enables twilight snowfall in Mountain.", 9, ref EnableTwilightSnow);

            RegisterWeatherConfig(SectionWorld, "Twilight_SnowStorm", "Enable Twilight Snowstorm Weather",
                "Enables twilight snowstorms in Mountain.", 10, ref EnableTwilightSnowStorm);

            RegisterWeatherConfig(SectionWorld, "Mistlands_rain", "Enable Mistlands Rain Weather",
                "Enables rainfall in Mistlands.", 11, ref EnableMistlandsRain);

            RegisterWeatherConfig(SectionWorld, "Mistlands_thunder", "Enable Mistlands Thunder Weather",
                "Enables thunderstorms in Mistlands.", 12, ref EnableMistlandsThunder);

            RegisterWeatherConfig(SectionWorld, "Ashlands_Misty", "Enable Ashlands Mist Weather",
                "Enables foggy mist weather in Ashlands.", 13, ref EnableAshlandsMisty);

            RegisterWeatherConfig(SectionWorld, "Ashlands_Ashrain", "Enable Ashlands Ash Rain Weather",
                "Enables ash rain in Ashlands.", 14, ref EnableAshlandsAshrain);

            RegisterWeatherConfig(SectionWorld, "Ashlands_Cinderrain", "Enable Ashlands Cinder Rain Weather",
                "Enables cinder rain in Ashlands.", 15, ref EnableAshlandsCinderrain);

            RegisterWeatherConfig(SectionWorld, "Ashlands_Storm", "Enable Ashlands Storm Weather",
                "Enables heavy storms in Ashlands.", 16, ref EnableAshlandsStorm);

            RegisterWeatherConfig(SectionWorld, "Ashlands_Meteorshower", "Enable Ashlands Meteor Shower Weather",
                "Enables meteor showers in Ashlands.", 17, ref EnableAshlandsMeteorshower);

            RegisterWeatherConfig(SectionWorld, "Ashlands_Seastorm", "Enable Ashlands Sea Storm Weather",
                "Enables boiling sea storms in Ashlands.", 18, ref EnableAshlandsSeastorm);

            RegisterWeatherConfig(SectionWorld, "Ashrain", "Enable Legacy Ash Rain Weather",
                "Enables legacy ash rain in Ashlands.", 19, ref EnableAshrain);

            RegisterWeatherConfig(SectionWorld, "Darklands_dark", "Enable Darklands Dark Weather",
                "Enables darklands environmental darkness.", 20, ref EnableDarklandsDark);

            RegisterWeatherConfig(SectionWorld, "nofogts", "Enable Internal NoFog Weather",
                "Enables internal nofog thunderstorm weather.", 21, ref EnableNofogts);

            // Boss Environments
            RegisterWeatherConfig(SectionBoss, "Eikthyr", "Enable Eikthyr Weather",
                "Enables storm and lightning weather during Eikthyr boss fight.", 1, ref EnableEikthyr);

            RegisterWeatherConfig(SectionBoss, "Eikthyr_no_lightning", "Enable Eikthyr No Lightning Weather",
                "Enables storm without lightning during Eikthyr boss fight.", 2, ref EnableEikthyrNoLightning);

            RegisterWeatherConfig(SectionBoss, "GDKing", "Enable The Elder Weather",
                "Enables overcast root weather during The Elder boss fight.", 3, ref EnableGDKing);

            RegisterWeatherConfig(SectionBoss, "Bonemass", "Enable Bonemass Weather",
                "Enables murky swamp weather during Bonemass boss fight.", 4, ref EnableBonemass);

            RegisterWeatherConfig(SectionBoss, "Moder", "Enable Moder Weather",
                "Enables freezing blizzard weather during Moder boss fight.", 5, ref EnableModer);

            RegisterWeatherConfig(SectionBoss, "GoblinKing", "Enable Yagluth Weather",
                "Enables fiery atmospheric weather during Yagluth boss fight.", 6, ref EnableGoblinKing);

            RegisterWeatherConfig(SectionBoss, "Queen", "Enable The Queen Weather",
                "Enables infested chamber weather during The Queen boss fight.", 7, ref EnableQueen);

            RegisterWeatherConfig(SectionBoss, "Fader", "Enable Fader Weather",
                "Enables ash vortex weather during Fader boss fight.", 8, ref EnableFader);

            RegisterWeatherConfig(SectionBoss, "DN_Bossroom", "Enable Deep North Boss Room Weather",
                "Enables Deep North boss arena weather.", 9, ref EnableDNBossroom);

            // Dungeon Environments
            RegisterWeatherConfig(SectionDungeon, "Crypt", "Enable Burial Chambers Weather",
                "Enables interior crypt gloom in Black Forest Burial Chambers.", 1, ref EnableCrypt);

            RegisterWeatherConfig(SectionDungeon, "SunkenCrypt", "Enable Sunken Crypt Weather",
                "Enables interior wet gloom in Swamp Sunken Crypts.", 2, ref EnableSunkenCrypt);

            RegisterWeatherConfig(SectionDungeon, "Caves", "Enable Mountain Caves Weather",
                "Enables interior mist and cold in Mountain Frost Caves.", 3, ref EnableCaves);

            RegisterWeatherConfig(SectionDungeon, "CavesHildir", "Enable Hildir Frost Caves Weather",
                "Enables interior mist in Hildir Mountain Caves.", 4, ref EnableCavesHildir);

            RegisterWeatherConfig(SectionDungeon, "CryptHildir", "Enable Hildir Sunken Crypt Weather",
                "Enables interior mist in Hildir Sunken Crypts.", 5, ref EnableCryptHildir);

            RegisterWeatherConfig(SectionDungeon, "InfectedMine", "Enable Infected Mine Weather",
                "Enables interior mist and spores in Mistlands Infected Mines.", 6, ref EnableInfectedMine);

            // Raid Environments
            RegisterWeatherConfig(SectionRaid, "Ghosts", "Enable Ghosts Raid Weather",
                "Enables ghostly mist during ghost raid encounters.", 1, ref EnableGhosts);

            RegisterWeatherConfig(SectionRaid, "JotunInvasion_meadows", "Enable Meadows Invasion Raid Weather",
                "Enables atmospheric distortion during Meadows Jotun invasions.", 2, ref EnableJotunInvasionMeadows);

            RegisterWeatherConfig(SectionRaid, "JotunInvasion_blackforest", "Enable Black Forest Invasion Raid Weather",
                "Enables atmospheric distortion during Black Forest Jotun invasions.", 3, ref EnableJotunInvasionBlackforest);

            RegisterWeatherConfig(SectionRaid, "JotunInvasion_swamp", "Enable Swamp Invasion Raid Weather",
                "Enables atmospheric distortion during Swamp Jotun invasions.", 4, ref EnableJotunInvasionSwamp);

            RegisterWeatherConfig(SectionRaid, "JotunInvasion_mountain", "Enable Mountain Invasion Raid Weather",
                "Enables atmospheric distortion during Mountain Jotun invasions.", 5, ref EnableJotunInvasionMountain);

            RegisterWeatherConfig(SectionRaid, "JotunInvasion_plains", "Enable Plains Invasion Raid Weather",
                "Enables atmospheric distortion during Plains Jotun invasions.", 6, ref EnableJotunInvasionPlains);

            RegisterWeatherConfig(SectionRaid, "JotunInvasion_mistlands", "Enable Mistlands Invasion Raid Weather",
                "Enables atmospheric distortion during Mistlands Jotun invasions.", 7, ref EnableJotunInvasionMistlands);

            RegisterWeatherConfig(SectionRaid, "Morkhalla", "Enable Morkhalla Raid Weather",
                "Enables Morkhalla event weather.", 8, ref EnableMorkhalla);

            RegisterWeatherConfig(SectionRaid, "TheHollow", "Enable The Hollow Raid Weather",
                "Enables The Hollow event weather.", 9, ref EnableTheHollow);
        }

        private static void RegisterWeatherConfig(string section, string envName, string configKey, string description, int order, ref ConfigEntry<bool> configEntry)
        {
            ConfigSyncBase.SyncedConfig(section, configKey, true,
                new ConfigDescription(description, null, new ConfigurationManagerAttributes { Order = order }),
                ref configEntry);

            if (configEntry != null)
            {
                configEntry.SettingChanged += (_, _) => OnWeatherSettingChanged();
                _weatherConfigs[envName] = configEntry;
            }
        }

        public static void RegisterDynamicEnvironment(string envName)
        {
            if (!_initialized || string.IsNullOrEmpty(envName))
            {
                return;
            }

            if (envName.IndexOf("clear", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return;
            }

            if (_weatherConfigs.ContainsKey(envName))
            {
                return;
            }

            string targetSection = DetermineSection(envName);

            ConfigEntry<bool> dynamicEntry = null;
            ConfigSyncBase.SyncedConfig(targetSection, $"Enable {envName} Weather", true,
                new ConfigDescription($"Enables {envName} weather event.", null, new ConfigurationManagerAttributes { Order = 50 }),
                ref dynamicEntry);

            if (dynamicEntry != null)
            {
                dynamicEntry.SettingChanged += (_, _) => OnWeatherSettingChanged();
                _weatherConfigs[envName] = dynamicEntry;
            }
        }

        private static string DetermineSection(string envName)
        {
            if (envName.IndexOf("boss", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return SectionBoss;
            }

            if (envName.IndexOf("crypt", StringComparison.OrdinalIgnoreCase) >= 0 ||
                envName.IndexOf("cave", StringComparison.OrdinalIgnoreCase) >= 0 ||
                envName.IndexOf("mine", StringComparison.OrdinalIgnoreCase) >= 0 ||
                envName.IndexOf("dungeon", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return SectionDungeon;
            }

            if (envName.IndexOf("invasion", StringComparison.OrdinalIgnoreCase) >= 0 ||
                envName.IndexOf("raid", StringComparison.OrdinalIgnoreCase) >= 0 ||
                IsRaidEnvironment(envName))
            {
                return SectionRaid;
            }

            return SectionWorld;
        }

        private static bool IsRaidEnvironment(string envName)
        {
            RandEventSystem randSystem = RandEventSystem.instance;
            if (randSystem == null || randSystem.m_events == null)
            {
                return false;
            }

            foreach (RandomEvent ev in randSystem.m_events)
            {
                if (ev != null && string.Equals(ev.m_forceEnvironment, envName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsEnvironmentEnabled(string envName)
        {
            if (string.IsNullOrEmpty(envName))
            {
                return true;
            }

            if (envName.IndexOf("clear", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            if (_weatherConfigs.TryGetValue(envName, out ConfigEntry<bool> config))
            {
                return config != null ? config.Value : true;
            }

            return true;
        }

        private static void OnWeatherSettingChanged()
        {
            if (EnvMan.instance == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(EnvMan.instance.m_debugEnv) && !IsEnvironmentEnabled(EnvMan.instance.m_debugEnv))
            {
                EnvMan.instance.m_debugEnv = "";
            }

            FieldInfo forceEnvField = AccessTools.DeclaredField(typeof(EnvMan), "m_forceEnv");
            if (forceEnvField != null)
            {
                string forceEnv = (string)forceEnvField.GetValue(EnvMan.instance);
                if (!string.IsNullOrEmpty(forceEnv) && !IsEnvironmentEnabled(forceEnv))
                {
                    forceEnvField.SetValue(EnvMan.instance, "");
                }
            }

            FieldInfo periodField = AccessTools.DeclaredField(typeof(EnvMan), "m_environmentPeriod");
            if (periodField != null)
            {
                periodField.SetValue(EnvMan.instance, -1L);
            }

            EnvSetup fallback = GetFallbackEnvironment(EnvMan.instance);
            if (fallback == null)
            {
                return;
            }

            FieldInfo nextEnvField = AccessTools.DeclaredField(typeof(EnvMan), "m_nextEnv");
            if (nextEnvField != null)
            {
                EnvSetup nextEnv = (EnvSetup)nextEnvField.GetValue(EnvMan.instance);
                if (nextEnv != null && !IsEnvironmentEnabled(nextEnv.m_name))
                {
                    nextEnvField.SetValue(EnvMan.instance, fallback);
                }
            }

            EnvSetup currentEnv = EnvMan.instance.GetCurrentEnvironment();
            if (currentEnv != null && !IsEnvironmentEnabled(currentEnv.m_name))
            {
                FieldInfo instantField = AccessTools.DeclaredField(typeof(EnvMan), "m_forceInstantEnvSwitchTimer");
                if (instantField != null)
                {
                    instantField.SetValue(EnvMan.instance, 2f);
                }

                FieldInfo currentEnvField = AccessTools.DeclaredField(typeof(EnvMan), "m_currentEnv");
                if (currentEnvField != null)
                {
                    currentEnvField.SetValue(EnvMan.instance, fallback);
                }

                MethodInfo queueMethod = AccessTools.DeclaredMethod(typeof(EnvMan), "QueueEnvironment", new[] { typeof(EnvSetup) });
                if (queueMethod != null)
                {
                    queueMethod.Invoke(EnvMan.instance, new object[] { fallback });
                }
            }
        }

        public static EnvSetup GetFallbackEnvironment(EnvMan envMan)
        {
            if (envMan == null)
            {
                return null;
            }

            EnvSetup clearEnv = envMan.GetEnv("Clear");
            if (clearEnv != null)
            {
                return clearEnv;
            }

            MethodInfo getDefaultEnvMethod = AccessTools.DeclaredMethod(typeof(EnvMan), "GetDefaultEnv");
            if (getDefaultEnvMethod != null)
            {
                return (EnvSetup)getDefaultEnvMethod.Invoke(envMan, null);
            }

            return null;
        }

        public static EnvSetup GetFallbackEnvironment(EnvMan envMan, BiomeSector biome)
        {
            if (envMan == null)
            {
                return null;
            }

            MethodInfo getBiomeEnvSetup = AccessTools.DeclaredMethod(typeof(EnvMan), "GetBiomeEnvSetup", new[] { typeof(Heightmap.Biome) });
            if (getBiomeEnvSetup != null)
            {
                BiomeEnvSetup biomeSetup = (BiomeEnvSetup)getBiomeEnvSetup.Invoke(envMan, new object[] { biome.Biome });
                if (biomeSetup != null && biomeSetup.m_environments != null)
                {
                    foreach (EnvEntry entry in biomeSetup.m_environments)
                    {
                        if (entry != null && entry.m_environment != null && entry.m_environment.IndexOf("clear", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            EnvSetup env = envMan.GetEnv(entry.m_environment);
                            if (env != null)
                            {
                                return env;
                            }
                        }
                    }
                }
            }

            return GetFallbackEnvironment(envMan);
        }

        [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.GetAvailableEnvironments))]
        private static class EnvManGetAvailableEnvironmentsPatch
        {
            private static void Postfix(EnvMan __instance, BiomeSector biome, ref List<EnvEntry> __result)
            {
                if (__result == null || __result.Count == 0)
                {
                    return;
                }

                __result.RemoveAll(entry => entry != null && !IsEnvironmentEnabled(entry.m_environment));

                if (__result.Count == 0)
                {
                    EnvSetup fallback = GetFallbackEnvironment(__instance, biome);
                    if (fallback != null)
                    {
                        __result.Add(new EnvEntry
                        {
                            m_environment = fallback.m_name,
                            m_weight = 1f,
                            m_env = fallback
                        });
                    }
                }
            }
        }

        [HarmonyPatch(typeof(EnvMan), "GetEnvironmentOverride")]
        private static class EnvManGetEnvironmentOverridePatch
        {
            private static void Postfix(EnvMan __instance, ref string __result)
            {
                if (!string.IsNullOrEmpty(__result) && !IsEnvironmentEnabled(__result))
                {
                    if (!string.IsNullOrEmpty(__instance.m_debugEnv) && !IsEnvironmentEnabled(__instance.m_debugEnv))
                    {
                        __instance.m_debugEnv = "";
                    }
                    __result = null;
                }
            }
        }

        [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.QueueEnvironment), new[] { typeof(string) })]
        private static class EnvManQueueEnvironmentStringPatch
        {
            private static void Prefix(EnvMan __instance, ref string name)
            {
                if (!string.IsNullOrEmpty(name) && !IsEnvironmentEnabled(name))
                {
                    EnvSetup fallback = GetFallbackEnvironment(__instance);
                    if (fallback != null)
                    {
                        name = fallback.m_name;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.QueueEnvironment), new[] { typeof(EnvSetup) })]
        private static class EnvManQueueEnvironmentPatch
        {
            private static void Prefix(EnvMan __instance, ref EnvSetup env)
            {
                if (env != null && !IsEnvironmentEnabled(env.m_name))
                {
                    EnvSetup fallback = GetFallbackEnvironment(__instance);
                    if (fallback != null)
                    {
                        env = fallback;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.SetForceEnvironment))]
        private static class EnvManSetForceEnvironmentPatch
        {
            private static void Prefix(EnvMan __instance, ref string env)
            {
                if (!string.IsNullOrEmpty(env) && !IsEnvironmentEnabled(env))
                {
                    EnvSetup fallback = GetFallbackEnvironment(__instance);
                    if (fallback != null)
                    {
                        env = fallback.m_name;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(EnvMan), "SetEnv", new[] { typeof(EnvSetup), typeof(float), typeof(float), typeof(float), typeof(float), typeof(float) })]
        private static class EnvManSetEnvPatch
        {
            private static void Prefix(EnvMan __instance, ref EnvSetup env)
            {
                if (env != null && !IsEnvironmentEnabled(env.m_name))
                {
                    EnvSetup fallback = GetFallbackEnvironment(__instance);
                    if (fallback != null)
                    {
                        env = fallback;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.Awake))]
        private static class EnvManAwakePatch
        {
            private static void Postfix(EnvMan __instance)
            {
                if (__instance == null || __instance.m_environments == null)
                {
                    return;
                }

                foreach (EnvSetup env in __instance.m_environments)
                {
                    if (env != null)
                    {
                        RegisterDynamicEnvironment(env.m_name);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(EnvMan), nameof(EnvMan.AppendEnvironment))]
        private static class EnvManAppendEnvironmentPatch
        {
            private static void Postfix(EnvSetup env)
            {
                if (env != null)
                {
                    RegisterDynamicEnvironment(env.m_name);
                }
            }
        }
    }
}
