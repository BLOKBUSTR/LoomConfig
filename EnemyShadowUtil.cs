using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

#pragma warning disable CS8618
namespace LoomConfig
{
    public class EnemyShadowUtil : MonoBehaviour
    {
        public EnemyShadow enemyShadow;
        
        [SuppressMessage("ReSharper", "InvertIf")]
        public IEnumerator SetDelayedProperties()
        {
            yield return null; // Skip for one update tick
            // Delay further in case the NavMeshAgent still isn't set up for some reason
            while (!enemyShadow.enemy.NavMeshAgent?.Agent)
            {
                LoomConfig.Debug("Delaying other properties, NavMeshAgent is not fully set up", enemyShadow);
                yield return new WaitForSeconds(.1f);
            }
            
            var playerDamage = LoomConfig.configClapPlayerDamage.Value;
            if (SemiFunc.IsNotMasterClient())
            {
                playerDamage = LoomConfigRoomProperties.GetClapPlayerDamage();
            }
            if (playerDamage != 100)
            {
                enemyShadow.hurtColliderScript.playerDamage = playerDamage;
                LoomConfig.Debug($"Changed clap player damage to {playerDamage}", enemyShadow);
            }
            
            if (SemiFunc.IsNotMasterClient()) yield break;
            
            var speed = LoomConfig.configMovementSpeed.Value;
            if (!Mathf.Approximately(speed, 1.2f))
            {
                enemyShadow.enemy.NavMeshAgent!.Agent.speed = speed;
                enemyShadow.enemy.NavMeshAgent.DefaultSpeed = speed;
                LoomConfig.Debug($"Changed movement speed to {speed}", enemyShadow);
            }
        }
    }
}
