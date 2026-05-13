using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;

#pragma warning disable CS8618
namespace LoomConfig
{
    [HarmonyPatch(typeof(EnemyShadowAnim))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class EnemyShadowAnimPatch
    {
        private static readonly Vector3 neckOriginalPosition = new(-0.00324382959f, 0.60799998f, -0.0120000001f);
        private static readonly List<Vector3> neckFixedPositions = new()
        {
            new Vector3(-0.00324380398f, 0.500300169f, -0.0120002478f),
            new Vector3(-0.00324380398f, 0.435680211f, -0.0120004788f),
            new Vector3(-0.00324380398f, 0.399781346f, -0.0120002031f)
        };
        
        private static readonly Vector3 neckOriginalScale = new(0.41f, 0.41f, 0.41f);
        private static readonly List<Vector3> neckFixedScales = new()
        {
            new Vector3(0.41f, 0.3484999f, 0.41f),
            new Vector3(0.41f, 0.3115999f, 0.41f),
            new Vector3(0.41f, 0.2911f, 0.41f)
        };
        
        internal static readonly ConditionalWeakTable<EnemyShadowAnim, Transform> neckRefs = new();
        
        internal static bool inLevel;
        
        [HarmonyPostfix, HarmonyPatch(nameof(EnemyShadowAnim.Awake))]
        internal static void EnemyShadowAnimAwakePostfix(EnemyShadowAnim __instance)
        {
            Transform mesh = __instance.transform.Find("Whole mesh CODE ✢/Whole Mesh ANIM ⟺/____________ (Torso)/Torso CODE ✢/Torso ANIM ⟺/____________ (Neck)/Neck CODE ✢/Neck ANIM ⟺/Neck (mesh)");
            if (!mesh)
            {
                LoomConfig.Error("Neck mesh transform not found! Has the GameObject hierarchy changed?", __instance);
                return;
            }
            LoomConfig.Debug($"Found neck mesh transform:\n{mesh}", __instance);
            neckRefs.Add(__instance, mesh);
            inLevel = true;
        }
        
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadowAnim.Update))]
        internal static void UpdatePrefix(EnemyShadowAnim __instance)
        {
            var idleLoopVolume = LoomConfig.configIdleLoopVolume.Value;
            if (Mathf.Approximately(idleLoopVolume, .1f) &&
                Mathf.Approximately(idleLoopVolume, __instance.idleLoop.Volume)) return;
            
            __instance.idleLoop.Volume = idleLoopVolume;
        }
        
        [HarmonyPostfix, HarmonyPatch(nameof(EnemyShadowAnim.Update))]
        internal static void UpdatePostfix(EnemyShadowAnim __instance)
        {
            if (!inLevel ||
                __instance.enemyShadow.currentState is EnemyShadow.State.Despawn or EnemyShadow.State.CoolDown) return;
            
            neckRefs.TryGetValue(__instance, out Transform neck);
            var flag = !LoomConfig.configFixNeckClipping.Value;
            
            if (!neck && SemiFunc.PerSecond(.033f, __instance))
            {
                LoomConfig.Error("Missing transform to neck mesh transform!", __instance);
                flag = true;
            }
            if (flag)
            {
                neck.localPosition = neckOriginalPosition;
                neck.localScale = neckOriginalScale;
                return;
            }
            
            var bendState = __instance.enemyShadow.bendState;
            var delta = Time.deltaTime * __instance.enemyShadow.headTurnSpeed;
            
            if (bendState > 0 &&
                neck.localPosition != neckFixedPositions[bendState] && neck.localScale != neckFixedScales[bendState])
            {
                neck.localPosition = Vector3.Slerp(neck.localPosition, neckFixedPositions[bendState], delta);
                neck.localScale = Vector3.Slerp(neck.localScale, neckFixedScales[bendState], delta);
            }
            else if (neck.localPosition != neckOriginalPosition && neck.localScale != neckOriginalScale)
            {
                neck.localPosition = Vector3.Slerp(neck.localPosition, neckOriginalPosition, delta);
                neck.localScale = Vector3.Slerp(neck.localScale, neckOriginalScale, delta);
            }
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
