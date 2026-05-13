# LoomConfig

This mod allows you to configure various parameters of the Loom monster, including logic, visuals and audio.

❗ **This mod must be installed on all clients!** Some values will be desynced otherwise. Logic settings are host-authoritative, while visuals and audio are client-side.

Suggestions are welcome! Tell me what's on your mind in the [Discord thread](https://discord.com/channels/1344557689979670578/1477562871473766572).

## 🔧 Configuration

All default config options match the vanilla settings exactly, so this mod should do nothing out of the box.
It is up to you to fine-tune this mod to your liking, as much or as little as you want. \
Configs can be updated in-game with [RepoConfig](https://thunderstore.io/c/repo/p/nickklmao/REPOConfig/). Some options will only apply on the next instantiated Loom (typically on level reload).

<details>
    <summary>Click to expand config list:</summary>

| Category    | ConfigEntry           | Default Value | Description                                                                                                          |
|-------------|-----------------------|:-------------:|----------------------------------------------------------------------------------------------------------------------|
| **Logic**   |                       |               |                                                                                                                      |
| &#124;      | MaxHealth             |      500      | The maximum health of Loom.                                                                                          |
| &#124;      | ClapPlayerDamage      |      100      | The amount of damage dealt to players by the clap attack. This setting will be synced to all clients in multiplayer. |
| &#124;      | ClapEnemyDamage       |      20       | The amount of damage dealt to enemies by the clap attack.                                                            |
| &#124;      | MovementSpeed         |     1.2f      | The base movement speed of Loom.                                                                                     |
| ↳           | MovementSpeedLeave    |     2.5f      | The movement speed of Loom in her Leave state                                                                        |
| **Visuals** |                       |
| &#124;      | PlayerLookDistance    |      7f       | The maximum distance at which Loom will look at the player.                                                          |
| &#124;      | ScreenEffectShowHands |     true      | Whether to show the hand layer in the "targeted" screen effect.                                                      |
| ↳           | ScreenEffectShowVeins |     true      | Whether to show the vein layer in the "targeted" screen effect.                                                      |
| **Audio**   |                       |
| &#124;      | IdleLoopVolume        |      .1f      | The volume of the "idleLoop" sound.                                                                                  |
| &#124;      | TargetedVolume        |      .5f      | The volume of the "targeted" sound, played when Loom begins to target you                                            |
| ↳           | NotTargetedVolume     |      .5f      | The volume of the "notTargeted" sound, played when Loom loses interest in you.                                       |
| **Fixes**   |                       |
| ↳           | FixNeckClipping       |     true      | An experimental fix for Loom's neck mesh clipping through her mouth in her various bend positions.                   |
| **Debug**   |                       |
| ↳           | EnableDebug           |     false     | Whether to enable debug logging. Keep this disabled for normal gameplay.                                             |

</details>

## ⚠️ Known Issues

- Player damage is quite inconsistent/unreliable on multiplayer clients, however I think this is more of a vanilla quirk than anything else... I plan to investigate this further.

## ⚠️ Compatibility

There are no known incompatibilities yet, but this mod may potentially conflict with others that skip or extensively patch these methods:
- `EnemyShadow.Awake`
- `EnemyShadow.StateLeave`
- `EnemyShadow.BendLogic`
- `EnemyShadowAnim.Awake`
- `EnemyShadowAnim.Update`
- `EnemyShadowAnim.playTargetedSound` (Prefix skips original method if volume is disabled)
- `EnemyShadowAnim.PlayUntargetedSound` (Prefix skips original method if volume is disabled)
- `EnemyShadowScreenVeinEffect.Start`

## ❤️ Acknowledgements

- [OrigamiCoder](https://thunderstore.io/c/repo/p/OrigamiCoder/) for code guidance, extensive playtesting, and a little joke for one of the method names ;)
- My friends [EvryFlare](https://linktr.ee/evryflare) and Erysis for additional playtesting
- "GameObject/Copy Path" editor script taken from this [Unity Discussions thread](https://discussions.unity.com/t/please-include-a-copy-path-when-right-clicking-a-game-object/638839/7).

Thank you for using this mod! \
Please report any issues on [GitHub](https://github.com/BLOKBUSTR/LoomConfig) or the [Discord thread](https://discord.com/channels/1344557689979670578/1477562871473766572).
