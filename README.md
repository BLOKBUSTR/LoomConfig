# LoomConfig

This is a simple mod that allows you to configure various parameters of the Loom monster, including mechanics, visuals and audio.

Suggestions are welcome! Tell me what's on your mind in the [Discord thread](https://discord.com/channels/1344557689979670578/1344699091959156787).

❗ **This mod must be installed on all clients!** Some values may be desynced otherwise. Mechanical settings are host-authoritative, while visuals and audio are not.

## 🔧 Configuration

All default config options match the vanilla settings exactly, so this mod should do nothing out of the box.
It is up to you to fine-tune this mod to your liking, as much or as little as you want. \
Configs can be updated in-game with [RepoConfig](https://thunderstore.io/c/repo/p/nickklmao/REPOConfig/). Some options will only apply on the next instantiated Loom (typically on level reload).

<details>
    <summary>Click to expand config list:</summary>

| Category    | ConfigEntry           | Default Value | Description                                                                                                                                                         |
|-------------|-----------------------|:-------------:|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **General** |                       |               |                                                                                                                                                                     |
| &#124;      |                       |               |                                                                                                                                                                     |
| &#124;      | MaxHealth             |      500      | The maximum health of Loom.                                                                                                                                         |
| &#124;      | ClapPlayerDamage      |      100      | The amount of damage dealt to players by the clap attack. This setting will be synced to all clients in multiplayer.                                                |
| &#124;      | ClapEnemyDamage       |      20       | The amount of damage dealt to enemies by the clap attack.                                                                                                           |
| &#124;      | MovementSpeed         |     1.2f      | The base movement speed of Loom.                                                                                                                                    |
| &#124;      | MovementSpeedLeave    |     2.5f      | The movement speed of Loom in her Leave state                                                                                                                       |
| &#124;      |                       |               |                                                                                                                                                                     |
| **Visual**  |                       |               |                                                                                                                                                                     |
| &#124;      | PlayerLookDistance    |      7f       | The distance at which Loom considers herself close enough to look at the player.                                                                                    |
| &#124;      | ScreenEffectShowHands |     true      | Whether to show the hand layer in the screen effect.                                                                                                                |
| &#124;      | ScreenEffectShowVeins |     true      | Whether to show the vein layer in the screen effect.                                                                                                                |
| **Audio**   |                       |               |                                                                                                                                                                     |
| &#124;      | IdleLoopVolume        |      .1f      | The volume of the idleLoop sound                                                                                                                                    |
| &#124;      | FixGlobalClapAudio    |     true      | Patches a vanilla bug where the globalClapSound is played at the world origin rather than at Loom's actual location. The local clapSound is unaffected by this bug. |
| **Debug**   |                       |               |                                                                                                                                                                     |
| &#124;      | EnableDebug           |     false     | Whether to enable debug logging. Keep this disabled for normal gameplay.                                                                                            |
| ↳           |                       |               |                                                                                                                                                                     |

</details>

## ⚠️ Compatibility

There are no known incompatibilities yet, but this mod may potentially conflict with others that extensively patch these methods:
- `EnemyShadow.Awake`
- `EnemyShadow.StateLeave`
- `EnemyShadow.BendLogic`
- `EnemyShadowAnim.Update`
- `EnemyShadowAnim.PlayClapSound` (Prefix skips original method)
- `EnemyShadowScreenVeinEffect.Start`

## ❤️ Acknowledgements

- [OrigamiCoder](https://thunderstore.io/c/repo/p/OrigamiCoder/) for more code guidance, extensive playtesting, and a little joke for a method name ;)

Thank you for using this mod! \
Please report any issues on [GitHub](https://github.com/BLOKBUSTR/LoomConfig) or the [Discord thread](https://discord.com/channels/1344557689979670578/1344699091959156787).
