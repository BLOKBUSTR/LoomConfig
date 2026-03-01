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
            if (_levelFailed) return; // To avoid getting spammed by Imperium if "Disable Game Over" is enabled
            LoomConfigRoomProperties.SetLoomProperties();
        }
    }
}
