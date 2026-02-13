using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
            
            // Enemy Damage
            var enemyDamage = LoomConfig.configClapEnemyDamage.Value;
            if (enemyDamage != 20)
            {
                __instance.hurtColliderScript.enemyDamage = enemyDamage;
                LoomConfig.Debug($"Changed clap enemy damage to {enemyDamage}", __instance);
            }
            
            // TODO Movement speed
            // var speed = LoomConfig.configMovementSpeed.Value;
            // if (!Mathf.Approximately(speed, 4f))
            // {
            //     __instance.enemy.Rigidbody.positionSpeedIdle = speed;
            //     __instance.enemy.Rigidbody.positionSpeedChase = speed;
            //     LoomConfig.Debug($"Changed movement speed to {speed}", __instance);
            // }
        }
        
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadow.UpdatePlayerTarget))]
        public static void UpdatePlayerTargetPrefix([SuppressMessage("ReSharper", "InconsistentNaming")] EnemyShadow __instance)
        {
            if (!SemiFunc.IsMultiplayer()) return;
            
            var playerDamage = LoomConfig.configClapPlayerDamage.Value;
            if (playerDamage == 500) return;
            
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
            
            synchronizer.SetPlayerAttackDamage(playerDamage);
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
            enemyShadow.hurtColliderScript.playerDamage = damage;
            LoomConfig.Debug($"Changed clap player damage to {damage}", enemyShadow);
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
