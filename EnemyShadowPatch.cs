using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Unity.VisualScripting;
using UnityEngine;

#pragma warning disable CS8618
namespace LoomConfig
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [HarmonyPatch(typeof(EnemyShadow))]
    internal static class EnemyShadowPatch
    {
        [HarmonyPostfix, HarmonyPatch(nameof(EnemyShadow.Awake))]
        [SuppressMessage("ReSharper", "InvertIf")]
        internal static void AwakePostfix(EnemyShadow __instance)
        {
            LoomConfig.Debug("Applying custom properties to new Loom, if any", __instance);
            
            var util = __instance.AddComponent<EnemyShadowUtil>();
            util.enemyShadow = __instance;
            
            // Delayed Properties
            util.StartCoroutine(util.SetDelayedProperties());
            
            if (SemiFunc.IsNotMasterClient()) return;
            
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
                else
                {
                    LoomConfig.Error("EnemyHealth component not found!", __instance);
                }
            }
            
            var enemyDamage = LoomConfig.configClapEnemyDamage.Value;
            if (enemyDamage != 20)
            {
                __instance.hurtColliderScript.enemyDamage = enemyDamage;
                LoomConfig.Debug($"Changed clap enemy damage to {enemyDamage}", __instance);
            }
        }
        
        [HarmonyPostfix, HarmonyPatch(nameof(EnemyShadow.StateLeave))]
        internal static void StateLeavePostfix(EnemyShadow __instance)
        {
            if (__instance.currentState is not EnemyShadow.State.Leave) return;
            
            var speed = LoomConfig.configMovementSpeedLeave.Value;
            if (!Mathf.Approximately(speed, 2.5f))
            {
                __instance.enemy.NavMeshAgent.OverrideAgent(speed, 30f, .1f);
            }
        }
        
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadow.BendLogic))]
        internal static void BendLogicPrefix(EnemyShadow __instance)
        {
            var distance = LoomConfig.configPlayerLookDistance.Value;
            if (!Mathf.Approximately(distance, 7f) && !Mathf.Approximately(distance, __instance.distanceFromPlayer))
            {
                __instance.closeEnoughToLook = __instance.distanceFromPlayer < distance;
            }
        }
    }
}
