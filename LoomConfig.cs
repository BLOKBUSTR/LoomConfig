using System.Diagnostics.CodeAnalysis;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

#pragma warning disable CS8618
namespace LoomConfig
{
    [BepInPlugin("BLOKBUSTR.LoomConfig", "LoomConfig", "0.0.1")]
    public class LoomConfig : BaseUnityPlugin
    {
        internal static LoomConfig Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger => Instance._logger;
        [SuppressMessage("ReSharper", "InconsistentNaming")] private ManualLogSource _logger => base.Logger;
        internal Harmony? Harmony { get; set; }
        
        // General
        public static ConfigEntry<int> configClapDamage;
        
        // Visual
        public static ConfigEntry<float> configPlayerLookDistance;
        
        // Debug
        private static ConfigEntry<bool> configEnableDebug;
        
        private void Awake()
        {
            Instance = this;
            
            gameObject.transform.parent = null;
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            
            InitConfig();
            Patch();
            
            Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
            Debug("Debug logging is enabled.");
        }
        
        private void InitConfig()
        {
            // General
            configClapDamage = Config.Bind("General", "ClapDamage", 100,
                new ConfigDescription("The amount of damage dealt by the clap attack.",
                    new AcceptableValueRange<int>(0, 500)));
            
            // Visual
            configPlayerLookDistance = Config.Bind("Visual", "PlayerLookDistance", 7f,
                new ConfigDescription("The distance at which the Loom considers itself close enough to look at the player.",
                    new AcceptableValueRange<float>(7f, 15f)));
            
            // Debug
            configEnableDebug = Config.Bind("Debug", "EnableDebug", false,
                new ConfigDescription("Whether to enable debug logging."));
        }
        
        internal void Patch()
        {
            Harmony ??= new Harmony(Info.Metadata.GUID);
            Harmony.PatchAll();
        }
        
        internal void Unpatch()
        {
            Harmony?.UnpatchSelf();
        }
        
        internal static void Debug(string message, MonoBehaviour? instance = null)
        {
            if (!configEnableDebug.Value) return;
            var prefix = instance != null ? $"{instance.GetInstanceID()}: " : "";
            Logger.LogDebug(prefix + message);
        }
    }
}
