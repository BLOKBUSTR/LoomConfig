using HarmonyLib;

#pragma warning disable CS6818
namespace LoomConfig
{
    [HarmonyPatch(typeof(SemiFunc))]
    public class SemiFuncHook
    {
        [HarmonyPostfix, HarmonyPatch(nameof(SemiFunc.OnLevelGenDone))]
        public static void OnLevelGenDonePostfix()
        {
            if (!SemiFunc.IsMasterClientOrSingleplayer()) return;
            
            var damage = LoomConfigRoomProperties.GetClapPlayerDamage();
            if (damage == -1)
            {
                LoomConfig.Logger.LogError("Could not get clap player damage from room (loom) properties!");
                return;
            }
            LoomConfig.syncedClapPlayerDamage = damage;
        }
    }
}
