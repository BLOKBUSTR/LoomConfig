using System.Diagnostics.CodeAnalysis;
using ExitGames.Client.Photon;
using Photon.Pun;

#pragma warning disable CS8618
namespace LoomConfig
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class LoomProperties
    {
        internal const string LOOM_CLAP_PLAYER_DAMAGE = "LoomClapPlayerDamage";
        
        // Method names jokingly suggested by OrigamiCoder lol
        public static void SetLoomProperties()
        {
            if (SemiFunc.IsNotMasterClient() || !SemiFunc.IsMultiplayer()) return;
            
            var playerDamage = LoomConfig.configClapPlayerDamage.Value;
            if (playerDamage == GetClapPlayerDamage())
            {
                LoomConfig.Debug("playerDamage is the same, no need to sync");
                return;
            }
            LoomConfig.Debug($"Setting Custom Room (Loom) Properties | playerDamage: {playerDamage}");
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
            {
                { LOOM_CLAP_PLAYER_DAMAGE, playerDamage }
            });
        }
        
        public static object? GetLoomProperties(string key)
        {
            if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(key)) return null;
            
            var damage = PhotonNetwork.CurrentRoom.CustomProperties[key];
            LoomConfig.Debug($"Getting Custom Room (Loom) Properties | key: {key} | damage: {damage}");
            return damage;
        }
        
        public static int GetClapPlayerDamage()
        {
            var damage = GetLoomProperties(LOOM_CLAP_PLAYER_DAMAGE);
            return damage == null ? -1 : (int)damage;
        }
    }
}
