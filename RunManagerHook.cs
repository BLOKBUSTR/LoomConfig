using System.Diagnostics.CodeAnalysis;
using HarmonyLib;

#pragma warning disable CS8618
namespace LoomConfig
{
    [HarmonyPatch(typeof(RunManager))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RunManagerHook
    {
        [HarmonyPostfix, HarmonyPatch(nameof(RunManager.ChangeLevel))]
        public static void ChangeLevelPostfix(bool _levelFailed)
        {
            // To avoid getting spammed by the Arena, or by Imperium if "Disable Game Over" is enabled
            if (_levelFailed) return;
            
            LoomConfigRoomProperties.SetLoomProperties();
        }
    }
}
