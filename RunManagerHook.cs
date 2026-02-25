using HarmonyLib;

#pragma warning disable CS8618
namespace LoomConfig
{
    [HarmonyPatch(typeof(RunManager))]
    public class RunManagerHook
    {
        [HarmonyPostfix, HarmonyPatch(nameof(RunManager.ChangeLevel))]
        public static void ChangeLevelPostfix()
        {
            LoomConfigRoomProperties.SetLoomProperties();
        }
    }
}
