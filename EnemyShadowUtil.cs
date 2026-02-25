using System.Collections;
using UnityEngine;

#pragma warning disable CS8618
namespace LoomConfig
{
    public class EnemyShadowUtil : MonoBehaviour
    {
        public EnemyShadow enemyShadow;
        
        public IEnumerator SetMovementSpeedCoroutine(float speed)
        {
            yield return null;
            
            while (!enemyShadow.enemy.NavMeshAgent?.Agent)
            {
                LoomConfig.Debug("Delaying movement speed change, NavMeshAgent is not fully set up", enemyShadow);
                yield return new WaitForSeconds(.1f);
            }
            
            enemyShadow.enemy.NavMeshAgent!.Agent.speed = speed;
            enemyShadow.enemy.NavMeshAgent.DefaultSpeed = speed;
            LoomConfig.Debug($"Changed movement speed to {speed}", enemyShadow);
        }
    }
}
