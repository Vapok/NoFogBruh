using System.Collections.Generic;
using BepInEx.Configuration;
using UnityEngine;

namespace NoFogBruh.Features
{
    public static class FogTargetManager
    {
        private const float CheckIntervalSeconds = 3.0f;
        private const string EnvironmentRootPath = "_GameMain/_Environment";

        private static readonly List<FogTarget> _targets = new List<FogTarget>();
        private static Transform _environmentRoot;
        private static GameObject _lastEnvObject;
        private static float _nextCheckTime;
        private static bool _allTargetsResolved;
        private static bool _initialized;

        private class FogTarget
        {
            public ConfigEntry<bool> Config;
            public GameObject Target;
            public string RelativePath;
            public string LocationListPrefix;
        }

        public static void Initialize()
        {
            if (_initialized || SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
            {
                return;
            }

            _initialized = true;
            _targets.Clear();

            RegisterTarget(DisableFogComponent.EnableGroundMist, "FollowPlayer/GroundMist");
            RegisterTarget(DisableFogComponent.EnableFog, "FollowPlayer/Mist");
            RegisterTarget(DisableFogComponent.EnableFogClouds, "FollowPlayer/FogClouds");
            RegisterTarget(DisableFogComponent.EnableInteriorDust, "FollowPlayer/InteriorDust");
            RegisterTarget(DisableFogComponent.EnableBlizzardMist, "FollowPlayer/SnowStorm");
            RegisterTarget(DisableFogComponent.EnableBlizzardMist, "FollowPlayer/Blizzard");
            RegisterTarget(DisableFogComponent.EnableFogOceanMist, "OceanMist");
            RegisterTarget(DisableFogComponent.EnableDistantFog, "Distant_fog_planes");

            RegisterLocationTarget(DisableFogComponent.EnableMistlandsMist, "_LocationList_Mistlands", "environment_effects/FollowPlayer/Mistlands_Globalmist");
            RegisterLocationTarget(DisableFogComponent.EnableMountainCaveMist, "_LocationList_MountainCaves", "environment/followplayer/env_mountain_cave/mist");
            RegisterLocationTarget(DisableFogComponent.EnableMountainCaveMist, "_LocationList_MountainCaves", "environment/followplayer/env_mountain_cave_hildir/mist");
            RegisterLocationTarget(DisableFogComponent.EnableAshlandsMist, "_LocationList_Ashlands", "environment_effects/FollowPlayer/Ashlands_Misty");
            RegisterLocationTarget(DisableFogComponent.EnableAshlandsFaderFx, "_LocationList_Ashlands", "environment_effects/FollowPlayer/Ashlands_FaderFX");
            RegisterLocationTarget(DisableFogComponent.EnableDeepNorthMist, "_LocationList_DeepNorth", "environment_effects/FollowPlayer/DeepNorth_Misty");
            RegisterLocationTarget(DisableFogComponent.EnableDeepNorthFaderFx, "_LocationList_DeepNorth", "environment_effects/FollowPlayer/DeepNorth_FaderFX");

            ResolveTargets();
            ApplyAll();
        }

        private static void RegisterTarget(ConfigEntry<bool> config, string relativePath)
        {
            FogTarget target = new FogTarget
            {
                Config = config,
                RelativePath = relativePath,
                LocationListPrefix = null
            };

            if (config != null)
            {
                config.SettingChanged += (_, _) => ApplyTarget(target);
            }

            _targets.Add(target);
        }

        private static void RegisterLocationTarget(ConfigEntry<bool> config, string locationListPrefix, string relativePath)
        {
            FogTarget target = new FogTarget
            {
                Config = config,
                RelativePath = relativePath,
                LocationListPrefix = locationListPrefix
            };

            if (config != null)
            {
                config.SettingChanged += (_, _) => ApplyTarget(target);
            }

            _targets.Add(target);
        }

        public static void OnSetEnv(GameObject currentEnvObject)
        {
            if (!_initialized || SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
            {
                return;
            }

            if (currentEnvObject != _lastEnvObject)
            {
                _lastEnvObject = currentEnvObject;
                ApplyAll();
            }

            if (!_allTargetsResolved && Time.time >= _nextCheckTime)
            {
                _nextCheckTime = Time.time + CheckIntervalSeconds;
                ResolveTargets();
                ApplyAll();
            }
        }

        public static void ResolveTargets()
        {
            if (_environmentRoot == null)
            {
                GameObject envObj = GameObject.Find(EnvironmentRootPath);
                if (envObj != null)
                {
                    _environmentRoot = envObj.transform;
                }
            }

            List<LocationList> locationLists = LocationList.GetAllLocationLists();
            bool hasLocationLists = locationLists != null && locationLists.Count > 0;

            bool allResolved = true;

            for (int i = 0; i < _targets.Count; i++)
            {
                FogTarget target = _targets[i];

                if (target.Target != null)
                {
                    continue;
                }

                if (target.LocationListPrefix == null)
                {
                    if (_environmentRoot != null)
                    {
                        Transform found = _environmentRoot.Find(target.RelativePath);
                        if (found != null)
                        {
                            target.Target = found.gameObject;
                        }
                    }
                }
                else if (hasLocationLists)
                {
                    for (int j = 0; j < locationLists.Count; j++)
                    {
                        LocationList locList = locationLists[j];
                        if (locList == null || locList.gameObject == null)
                        {
                            continue;
                        }

                        if (locList.gameObject.name.StartsWith(target.LocationListPrefix))
                        {
                            Transform found = locList.transform.Find(target.RelativePath);
                            if (found != null)
                            {
                                target.Target = found.gameObject;
                                break;
                            }
                        }
                    }
                }

                if (target.Target == null)
                {
                    allResolved = false;
                }
            }

            _allTargetsResolved = allResolved;
        }

        public static void ApplyAll()
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                ApplyTarget(_targets[i]);
            }
        }

        private static void ApplyTarget(FogTarget target)
        {
            if (target.Target != null && target.Config != null)
            {
                bool desired = target.Config.Value;
                if (target.Target.activeSelf != desired)
                {
                    target.Target.SetActive(desired);
                }
            }
        }

        public static void Reset()
        {
            _environmentRoot = null;
            _lastEnvObject = null;
            _allTargetsResolved = false;
            _nextCheckTime = 0f;

            for (int i = 0; i < _targets.Count; i++)
            {
                _targets[i].Target = null;
            }
        }
    }
}
