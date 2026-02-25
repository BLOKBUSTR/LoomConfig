using System.Diagnostics.CodeAnalysis;
using HarmonyLib;

#pragma warning disable CS8618
namespace LoomConfig
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [HarmonyPatch(typeof(EnemyShadowScreenVeinEffect))]
    public class EnemyShadowScreenVeinEffectPatch
    {
        [HarmonyPrefix, HarmonyPatch(nameof(EnemyShadowScreenVeinEffect.Start))]
        public static void StartPrefix(EnemyShadowScreenVeinEffect __instance)
        {
            if (!LoomConfig.configScreenEffectShowHands.Value)
            {
                __instance.handSpriteRenderer.enabled = false;
            }
            if (!LoomConfig.configScreenEffectShowVeins.Value)
            {
                __instance.veinSpriteRenderer.enabled = false;
            }
        }
    }
}
