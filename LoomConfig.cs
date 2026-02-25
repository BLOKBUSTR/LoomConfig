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
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class LoomConfig : BaseUnityPlugin
    {
        internal static LoomConfig Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger => Instance._logger;
        private ManualLogSource _logger => base.Logger;
        internal Harmony? Harmony { get; set; }
        
        // Mechanical
        public static ConfigEntry<int> configMaxHealth;
        public static ConfigEntry<int> configClapPlayerDamage;
        public static int syncedClapPlayerDamage;
        public static ConfigEntry<int> configClapEnemyDamage;
        public static ConfigEntry<float> configMovementSpeed;
        public static ConfigEntry<float> configMovementSpeedLeave;
        
        // Visual
        public static ConfigEntry<float> configPlayerLookDistance;
        public static ConfigEntry<bool> configScreenEffectShowHands;
        public static ConfigEntry<bool> configScreenEffectShowVeins;
        
        // Audio
        public static ConfigEntry<float> configIdleLoopVolume;
        public static ConfigEntry<bool> configFixGlobalClapAudio;
        
        // Debug
        private static ConfigEntry<bool> configEnableDebug;
        
        private void Awake()
        {
            Instance = this;
            
            gameObject.transform.parent = null;
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            
            RegisterConfig();
            syncedClapPlayerDamage = configClapPlayerDamage.Value;
            Patch();
            
            Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} has loaded!");
            Debug("Debug logging is enabled.");
        }
        
        private void RegisterConfig()
        {
            // Mechanical
            configMaxHealth = Config.Bind("Mechanical", "MaxHealth", 500,
                new ConfigDescription("The maximum health of Loom.",
                    new AcceptableValueRange<int>(10, 1000)));
            configClapPlayerDamage = Config.Bind("Mechanical", "ClapPlayerDamage", 100,
                new ConfigDescription("The amount of damage dealt to players by the clap attack. This setting will be synced to all clients in multiplayer.",
                    new AcceptableValueRange<int>(0, 1000)));
            configClapEnemyDamage = Config.Bind("Mechanical", "ClapEnemyDamage", 20,
                new ConfigDescription("The amount of damage dealt to enemies by the clap attack.",
                    new AcceptableValueRange<int>(0, 1000)));
            configMovementSpeed = Config.Bind("Mechanical", "MovementSpeed", 1.2f,
                new ConfigDescription("The base movement speed of Loom.",
                    new AcceptableValueRange<float>(1f, 4f)));
            configMovementSpeedLeave = Config.Bind("Mechanical", "MovementSpeedLeave", 2.5f,
                new ConfigDescription("The movement speed of Loom in her Leave state.",
                    new AcceptableValueRange<float>(1f, 4f)));
            
            // Visual
            configPlayerLookDistance = Config.Bind("Visual", "PlayerLookDistance", 7f,
                new ConfigDescription("The distance at which Loom considers herself close enough to look at the player.",
                    new AcceptableValueRange<float>(5f, 15f)));
            configScreenEffectShowHands = Config.Bind("Visual", "ScreenEffectShowHands", true,
                new ConfigDescription("Whether to show the hand layer in the screen effect."));
            configScreenEffectShowVeins = Config.Bind("Visual", "ScreenEffectShowVeins", true,
                new ConfigDescription("Whether to show the vein layer in the screen effect."));
            
            // Audio
            configIdleLoopVolume = Config.Bind("Audio", "IdleLoopVolume", .1f,
                new ConfigDescription("The volume of the idleLoop sound.",
                    new AcceptableValueRange<float>(0f, .2f)));
            configFixGlobalClapAudio = Config.Bind("Audio", "FixGlobalClapAudio", true,
                new ConfigDescription("If true, fixes a vanilla bug where the globalClapSound is played at the world origin rather than at Loom's actual location. The local clapSound is unaffected by this bug."));
            
            // Debug
            configEnableDebug = Config.Bind("Debug", "EnableDebug", true,
                new ConfigDescription("Whether to enable debug logging. Keep this disabled for normal gameplay."));
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
            var prefix = (bool)instance ? $"{instance!.GetInstanceID()}: " : string.Empty;
            Logger.LogDebug(prefix + message);
        }
    }
}
