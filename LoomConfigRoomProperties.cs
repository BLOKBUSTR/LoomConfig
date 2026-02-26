using System.Diagnostics.CodeAnalysis;
using ExitGames.Client.Photon;
using Photon.Pun;

#pragma warning disable CS8618
namespace LoomConfig
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class LoomConfigRoomProperties
    {
        internal const string LOOM_CLAP_PLAYER_DAMAGE = "LoomClapPlayerDamage";
        
        // Method names jokingly suggested by OrigamiCoder lol
        public static void SetLoomProperties()
        {
            if (!PhotonNetwork.IsMasterClient || !PhotonNetwork.InRoom) return;
            
            LoomConfig.Debug("Setting Custom Room Properties");
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
            {
                { LOOM_CLAP_PLAYER_DAMAGE, LoomConfig.configClapPlayerDamage.Value }
            });
        }
        
        public static object? GetLoomProperties(string key)
        {
            if (SemiFunc.IsMasterClientOrSingleplayer() || !PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(key)) return null;
            
            LoomConfig.Debug("Getting Custom Room Properties | key: " + key);
            return PhotonNetwork.CurrentRoom.CustomProperties[key];
        }
        
        public static int GetClapPlayerDamage()
        {
            var damage = GetLoomProperties(LOOM_CLAP_PLAYER_DAMAGE);
            return damage == null ? -1 : (int)damage;
        }
    }
}
