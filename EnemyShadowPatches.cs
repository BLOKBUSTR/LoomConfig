using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HarmonyLib;
using UnityEngine;

#pragma warning disable CS8618
namespace LoomConfig
{
    [HarmonyPatch(typeof(EnemyShadow))]
    public static class EnemyShadowPatches
    {
        
        
        [HarmonyPostfix, HarmonyPatch(nameof(EnemyShadow.Awake))]
        [SuppressMessage("ReSharper", "InvertIf")]
        public static void AwakePostfix([SuppressMessage("ReSharper", "InconsistentNaming")] EnemyShadow __instance)
        {
            // Health
            var health = LoomConfig.configMaxHealth.Value;
            if (health != 500)
            {
                var enemyHealth = __instance.GetComponent<EnemyHealth>();
                if (enemyHealth)
                {
                    enemyHealth.health = health;
                    enemyHealth.healthCurrent = health;
                    LoomConfig.Debug($"Changed health to {health}", __instance);
                }
            }
            
            // Damage
            var playerDamage = LoomConfig.configClapPlayerDamage.Value;
            if (playerDamage != 100)
            {
                __instance.hurtColliderScript.playerDamage = playerDamage;
                LoomConfig.Debug($"Changed clap player damage to {playerDamage}", __instance);
            }
            var enemyDamage = LoomConfig.configClapEnemyDamage.Value;
            if (enemyDamage != 20)
            {
                __instance.hurtColliderScript.enemyDamage = enemyDamage;
                LoomConfig.Debug($"Changed clap enemy damage to {enemyDamage}", __instance);
            }
            
            // Movement speed
            var speed = LoomConfig.configMovementSpeed.Value;
            if (!Mathf.Approximately(speed, 4f))
            {
                __instance.enemy.Rigidbody.positionSpeedIdle = speed;
                __instance.enemy.Rigidbody.positionSpeedChase = speed;
                LoomConfig.Debug($"Changed movement speed to {speed}", __instance);
            }
        }
    }
    
    // [HarmonyPatch(typeof(EnemyShadow))]
    // [SuppressMessage("ReSharper", "InconsistentNaming")]
    // public static class EnemyShadow_Update_Transpiler
    // {
    //     
    //     
    //     [HarmonyPatch(nameof(EnemyShadow.Update))]
    //     public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions,
    //         [SuppressMessage("ReSharper", "InconsistentNaming")] EnemyShadow __instance)
    //     {
    //         
    //         
    //         var codes = new List<CodeInstruction>(instructions);
    //         
    //         
    //         
    //         return codes.AsEnumerable();
    //     }
    // }
}
