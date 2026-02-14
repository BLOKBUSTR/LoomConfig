using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

#pragma warning disable CS8618
namespace LoomConfig
{
    [HarmonyPatch(typeof(EnemyShadow))]
    public static class EnemyShadowPatch
    {
        
        
        [HarmonyPostfix, HarmonyPatch(nameof(EnemyShadow.Awake))]
        [SuppressMessage("ReSharper", "InvertIf")]
        public static void AwakePostfix([SuppressMessage("ReSharper", "InconsistentNaming")] EnemyShadow __instance)
        {
            if (SemiFunc.IsMultiplayer())
            {
                var synchronizer = __instance.AddComponent<EnemyShadowSynchronizer>();
                synchronizer.enemyShadow = __instance;
                synchronizer.photonView = __instance.photonView;
            }
            
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
            if (playerDamage != 100 && SemiFunc.IsMasterClientOrSingleplayer())
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
            
            // TODO Move to coroutine
            // Movement speed
            var speed = LoomConfig.configMovementSpeed.Value;
            if (!Mathf.Approximately(speed, 1.2f))
            {
                if (__instance.enemy.NavMeshAgent == null)
                {
                    LoomConfig.Error("EnemyNavMeshAgent not found!");
                    return;
                }
                if (__instance.enemy.NavMeshAgent.Agent == null)
                {
                    LoomConfig.Error("NavmeshAgent not found!");
                    return;
                }
                
                __instance.enemy.NavMeshAgent.Agent.speed = speed;
                LoomConfig.Debug($"Changed movement speed to {speed}", __instance);
            }
        }
        
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadow.UpdatePlayerTarget))]
        public static void UpdatePlayerTargetPrefix([SuppressMessage("ReSharper", "InconsistentNaming")] EnemyShadow __instance)
        {
            if (!SemiFunc.IsMultiplayer()) return;
            if (__instance.hurtColliderScript.playerDamage == 100)
            {
                LoomConfig.Debug("Clap player damage is default, no need to sync", __instance);
                return;
            }
            
            var synchronizer = __instance.GetComponent<EnemyShadowSynchronizer>();
            if (synchronizer == null)
            {
                LoomConfig.Error("EnemyShadowSynchronizer component not found!", __instance);
                return;
            }
            if (synchronizer.synced)
            {
                LoomConfig.Debug("Clap player damage is already synced", __instance);
                return;
            }
            
            synchronizer.SetPlayerAttackDamage(__instance.hurtColliderScript.playerDamage);
        }
        
        [HarmonyPostfix, HarmonyPatch(nameof(EnemyShadow.StateLeave))]
        public static void StateLeavePostfix([SuppressMessage("ReSharper", "InconsistentNaming")] EnemyShadow __instance)
        {
            if (__instance.currentState is not EnemyShadow.State.Leave) return;
            
            var speed = LoomConfig.configMovementSpeedLeave.Value;
            if (!Mathf.Approximately(speed, 2.5f))
            {
                __instance.enemy.NavMeshAgent.OverrideAgent(speed, 30f, .1f);
            }
        }
        
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadow.BendLogic))]
        public static void BendLogicPrefix([SuppressMessage("ReSharper", "InconsistentNaming")] EnemyShadow __instance)
        {
            var distance = LoomConfig.configPlayerLookDistance.Value;
            if (!Mathf.Approximately(distance, 7f))
            {
                __instance.closeEnoughToLook = __instance.distanceFromPlayer < distance;
            }
        }
    }
    
    public class EnemyShadowSynchronizer : MonoBehaviourPun
    {
        public EnemyShadow enemyShadow;
        public new PhotonView photonView;
        
        internal bool synced;
        
        public void SetPlayerAttackDamage(int damage)
        {
            if (SemiFunc.IsNotMasterClient() || synced || !enemyShadow || !photonView) return;
            
            if (SemiFunc.IsMultiplayer())
            {
                photonView.RPC(nameof(SetPlayerAttackDamageRPC), RpcTarget.Others, damage);
                LoomConfig.Debug($"Synced clap player damage ({damage}) to clients", enemyShadow);
            }
            synced = true;
        }
        
        [PunRPC]
        public void SetPlayerAttackDamageRPC(int damage, PhotonMessageInfo info = default)
        {
            enemyShadow.hurtColliderScript.playerDamage = damage;
            LoomConfig.Debug($"Synced clap player damage ({damage}) with host", enemyShadow);
        }
    }
}
