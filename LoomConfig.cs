using System;
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
        public static ConfigEntry<int> configMaxHealth;
        public static ConfigEntry<int> configClapPlayerDamage;
        public static ConfigEntry<int> configClapEnemyDamage;
        public static ConfigEntry<float> configMovementSpeed;
        
        // Visual
        public static ConfigEntry<float> configPlayerLookDistance;
        public static ConfigEntry<bool> configScreenEffectShowHands;
        public static ConfigEntry<bool> configScreenEffectShowVeins;
        
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
            configMaxHealth = Config.Bind("General", "MaxHealth", 500,
                new ConfigDescription("The maximum health of Loom.",
                    new AcceptableValueRange<int>(10, 1000)));
            configClapPlayerDamage = Config.Bind("General", "ClapPlayerDamage", 100,
                new ConfigDescription("The amount of damage dealt to players by the clap attack. This setting will be synced to all clients.",
                    new AcceptableValueRange<int>(0, 1000)));
            configClapEnemyDamage = Config.Bind("General", "ClapEnemyDamage", 20,
                new ConfigDescription("The amount of damage dealt to enemies by the clap attack.",
                    new AcceptableValueRange<int>(0, 1000)));
            configMovementSpeed = Config.Bind("General", "MovementSpeed", 4f,
                new ConfigDescription("The movement speed of Loom.",
                    new AcceptableValueRange<float>(1f, 6f)));
            
            // Visual
            configPlayerLookDistance = Config.Bind("Visual", "PlayerLookDistance", 7f,
                new ConfigDescription("The distance at which Loom considers herself close enough to look at the player.",
                    new AcceptableValueRange<float>(7f, 15f)));
            configScreenEffectShowHands = Config.Bind("Visual", "ScreenEffectShowHands", true,
                new ConfigDescription("Whether to show the hand layer in the screen effect."));
            configScreenEffectShowVeins = Config.Bind("Visual", "ScreenEffectShowVeins", true,
                new ConfigDescription("Whether to show the vein layer in the screen effect."));
            
            // Debug
            configEnableDebug = Config.Bind("Debug", "EnableDebug", true,
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
            var prefix = instance != null ? $"{instance.GetInstanceID()}: " : string.Empty;
            Logger.LogDebug(prefix + message);
        }
        
        internal static void Error(string message, MonoBehaviour? instance = null)
        {
            var prefix = instance != null ? $"{instance.GetInstanceID()}: " : string.Empty;
            Logger.LogError(prefix + message);
        }
    }
}
