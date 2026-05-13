using System.Diagnostics.CodeAnalysis;
using HarmonyLib;

#pragma warning disable CS8618
namespace LoomConfig
{
    [HarmonyPatch(typeof(RunManager))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class RunManagerHook
    {
        [HarmonyPostfix, HarmonyPatch(nameof(RunManager.ChangeLevel))]
        public static void ChangeLevelPostfix(RunManager __instance, bool _levelFailed)
        {
            // To avoid getting spammed by the Arena, or by Imperium if "Disable Game Over" is enabled
            if (_levelFailed || SemiFunc.IsLevelArena(RunManager.instance.levelCurrent)) return;
            
            EnemyShadowAnimPatch.inLevel = false;
            EnemyShadowAnimPatch.neckRefs.Clear();
            LoomProperties.SetLoomProperties();
        }
    }
}
