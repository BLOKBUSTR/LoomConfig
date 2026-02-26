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
            if (SemiFunc.IsMasterClientOrSingleplayer()) return;
            
            var damage = LoomConfigRoomProperties.GetClapPlayerDamage();
            if (damage < 0)
            {
                LoomConfig.Logger.LogError("Could not get clap player damage from Custom Room Properties! Falling back to vanilla default value (100)");
                damage = 100;
            }
            LoomConfig.syncedClapPlayerDamage = damage;
        }
    }
}
