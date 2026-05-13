using System.Diagnostics.CodeAnalysis;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

#pragma warning disable CS8618
namespace LoomConfig
{
    [BepInPlugin("BLOKBUSTR.LoomConfig", "LoomConfig", "1.1.0")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LoomConfig : BaseUnityPlugin
    {
        internal static LoomConfig Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger => Instance._logger;
        private ManualLogSource _logger => base.Logger;
        internal Harmony? Harmony { get; set; }
        
        #region ConfigEntries
        
        // Logic
        public static ConfigEntry<int> configMaxHealth;
        public static ConfigEntry<int> configClapPlayerDamage;
        public static ConfigEntry<int> configClapEnemyDamage;
        public static ConfigEntry<float> configMovementSpeed;
        public static ConfigEntry<float> configMovementSpeedLeave;
        
        // Visuals
        public static ConfigEntry<float> configPlayerLookDistance;
        public static ConfigEntry<bool> configScreenEffectShowHands;
        public static ConfigEntry<bool> configScreenEffectShowVeins;
        
        // Audio
        public static ConfigEntry<float> configIdleLoopVolume;
        public static ConfigEntry<float> configTargetedVolume;
        public static ConfigEntry<float> configNotTargetedVolume;
        
        // Fixes
        public static ConfigEntry<bool> configFixNeckClipping;
        
        // Debug
        private static ConfigEntry<bool> configEnableDebug;
        
        #endregion
        
        private void Awake()
        {
            Instance = this;
            
            gameObject.transform.parent = null;
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            
            RegisterConfig();
            Patch();
            
            Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
            Debug("Debug logging is enabled.");
        }
        
        private void RegisterConfig()
        {
            // Logic
            configMaxHealth = Config.Bind("Logic", "MaxHealth", 500,
                new ConfigDescription("The maximum health of Loom.",
                    new AcceptableValueRange<int>(10, 1000)));
            configClapPlayerDamage = Config.Bind("Logic", "ClapPlayerDamage", 100,
                new ConfigDescription("The amount of damage dealt to players by the clap attack. This setting will be synced to all clients in multiplayer.",
                    new AcceptableValueRange<int>(0, 1000)));
            configClapEnemyDamage = Config.Bind("Logic", "ClapEnemyDamage", 20,
                new ConfigDescription("The amount of damage dealt to enemies by the clap attack.",
                    new AcceptableValueRange<int>(0, 1000)));
            configMovementSpeed = Config.Bind("Logic", "MovementSpeed", 1.2f,
                new ConfigDescription("The base movement speed of Loom.",
                    new AcceptableValueRange<float>(1f, 4f)));
            configMovementSpeedLeave = Config.Bind("Logic", "MovementSpeedLeave", 2.5f,
                new ConfigDescription("The movement speed of Loom in her Leave state.",
                    new AcceptableValueRange<float>(1f, 4f)));
            
            // Visuals
            configPlayerLookDistance = Config.Bind("Visuals", "PlayerLookDistance", 7f,
                new ConfigDescription("The maximum distance at which Loom will look at the player.",
                    new AcceptableValueRange<float>(5f, 15f)));
            configScreenEffectShowHands = Config.Bind("Visuals", "ScreenEffectShowHands", true,
                "Whether to show the hand layer in the \"targeted\" screen effect.");
            configScreenEffectShowVeins = Config.Bind("Visuals", "ScreenEffectShowVeins", true,
                "Whether to show the vein layer in the \"targeted\" screen effect.");
            
            // Audio
            configIdleLoopVolume = Config.Bind("Audio", "IdleLoopVolume", .1f,
                new ConfigDescription("The volume of the \"idleLoop\" sound.",
                    new AcceptableValueRange<float>(0f, .2f)));
            configTargetedVolume = Config.Bind("Audio", "TargetedVolume", .5f,
                new ConfigDescription("The volume of the \"targeted\" sound, played when Loom begins to target you.",
                    new AcceptableValueRange<float>(0f, 1f)));
            configNotTargetedVolume = Config.Bind("Audio", "NotTargetedVolume", .5f,
                new ConfigDescription("The volume of the \"notTargeted\" sound, played when Loom loses interest in you.",
                    new AcceptableValueRange<float>(0f, 1f)));
            
            // Fixes
            configFixNeckClipping = Config.Bind("Fixes", "FixNeckClipping", true,
                "An experimental fix for Loom's neck mesh clipping through her mouth in her various bend positions.");
            
            // Debug
            configEnableDebug = Config.Bind("Debug", "EnableDebug", false,
                "Whether to enable debug logging. Keep this disabled for normal gameplay.");
        }
        
        internal void Patch()
        {
            Harmony ??= new Harmony(Info.Metadata.GUID);
            Harmony.PatchAll();
        }
        
        internal static void Debug(string message, MonoBehaviour? instance = null)
        {
            if (configEnableDebug.Value) Logger.LogDebug((bool)instance ? instance + ": " + message : message);
        }
        
        internal static void Error(string message, MonoBehaviour? instance = null)
        {
            Logger.LogDebug((bool)instance ? instance + ": " + message : message);
        }
    }
}
