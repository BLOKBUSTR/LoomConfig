using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using UnityEngine;

#pragma warning disable CS8618
namespace LoomConfig
{
    [HarmonyPatch(typeof(EnemyShadowAnim))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class EnemyShadowAnimPatch
    {
        // TODO see if Sound.StoreDefault() is actually used in R.E.P.O., if not then call here manually
        
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadowAnim.Update))]
        internal static void UpdatePrefix(EnemyShadowAnim __instance)
        {
            var idleLoopVolume = LoomConfig.configIdleLoopVolume.Value;
            if (Mathf.Approximately(idleLoopVolume, .1f) &&
                Mathf.Approximately(idleLoopVolume, __instance.idleLoop.Volume)) return;
            
            __instance.idleLoop.Volume = idleLoopVolume;
        }
        
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadowAnim.playTargetedSound))]
        internal static bool PlayTargetedSoundPrefix(EnemyShadowAnim __instance)
        {
            var targetedVolume = LoomConfig.configTargetedVolume.Value;
            if (targetedVolume <= 0f) return false;
            if (!Mathf.Approximately(targetedVolume, .5f) ||
                !Mathf.Approximately(targetedVolume, __instance.targeted.Volume))
            {
                __instance.targeted.Volume = targetedVolume;
                if (targetedVolume < .2f)
                {
                    __instance.targeted.VolumeRandom = targetedVolume * .5f;
                }
            }
            return true;
        }
        
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadowAnim.PlayUntargetedSound))]
        internal static bool PlayUntargetedSoundPrefix(EnemyShadowAnim __instance)
        {
            var notTargetedVolume = LoomConfig.configNotTargetedVolume.Value;
            if (notTargetedVolume <= 0f) return false;
            if (!Mathf.Approximately(notTargetedVolume, .5f) ||
                !Mathf.Approximately(notTargetedVolume, __instance.notTargeted.Volume))
            {
                __instance.notTargeted.Volume = notTargetedVolume;
                if (notTargetedVolume < .2f)
                {
                    __instance.notTargeted.VolumeRandom = notTargetedVolume * .5f;
                }
            }
            return true;
        }
    }
}
