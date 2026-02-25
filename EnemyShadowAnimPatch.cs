using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using UnityEngine;

#pragma warning disable CS8618
namespace LoomConfig
{
    [HarmonyPatch(typeof(EnemyShadowAnim))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class EnemyShadowAnimPatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadowAnim.Update))]
        public static void UpdatePrefix(EnemyShadowAnim __instance)
        {
            var idleLoopVolume = LoomConfig.configIdleLoopVolume.Value;
            if (Mathf.Approximately(idleLoopVolume, .1f) || Mathf.Approximately(idleLoopVolume, __instance.idleLoop.Volume)) return;
            
            __instance.idleLoop.Volume = idleLoopVolume;
        }
        
        // Fix for Loom's global clap audio playing at world origin
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadowAnim.PlayClapSound))]
        public static bool PlayClapSoundPrefix(EnemyShadowAnim __instance)
        {
            if (!LoomConfig.configFixGlobalClapAudio.Value)
            {
                LoomConfig.Debug("Played globalClapSound at vanilla position (Vector3.zero)", __instance);
                return true;
            }
            
            __instance.clapSound.Play(__instance.transform.position);
            __instance.globalClapSound.Play(__instance.transform.position);
            LoomConfig.Debug($"Played globalClapSound at fixed position (transform.position: {__instance.transform.position})", __instance);
            
            return false;
        }
    }
}
