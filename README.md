# LoomConfig

❗ **Client requirement TBD**

This is a "simple" mod that allows you to configure various parameters of the Loom monster, from mechanics to animations.

Suggestions are welcome! Tell me what's on your mind in the [Discord thread]().

## 🔧 Configuration

All default config options match the vanilla settings exactly, so this mod should do nothing out of the box.
It is up to you to fine-tune this mod to your liking, as much or as little as you want. \
Configs can be updated in-game with RepoConfig, but they will only apply on the next spawned Loom instance (typically on level reload).

<details>
    <summary>Click to expand config list:</summary>

| Category    | ConfigEntry        | Default Value | Description                                                                      |
|-------------|--------------------|:-------------:|----------------------------------------------------------------------------------|
| **General** |                    |               |                                                                                  |
| &#124;      |                    |               |                                                                                  |
| &#124;      | MaxHealth          |      500      | The maximum health of Loom.                                                      |
| &#124;      | ClapPlayerDamage   |      100      | The amount of damage dealt to players by the clap attack.                        |
| &#124;      | ClapEnemyDamage    |      20       | The amount of damage dealt to enemies by the clap attack.                        |
| &#124;      | MovementSpeed      |      4f       | The movement speed of Loom.                                                      |
| ↳           |                    |               |                                                                                  |
|             |                    |               |                                                                                  |
| **Visual**  |                    |               |                                                                                  |
|             | PlayerLookDistance |      7f       | The distance at which Loom considers herself close enough to look at the player. |
|             |                    |               |                                                                                  |
| **Debug**   | EnableDebug        |     false     | Whether to enable debug logging.                                                 |

</details>

## ⚠️ Compatibility

There are no known incompatibilities yet, but this mod may potentially conflict with others that extensively patch these methods:
- `EnemyShadow.Update()`

---

Thank you for using this mod! \
Please report any issues on [GitHub]() or the [Discord thread]().
