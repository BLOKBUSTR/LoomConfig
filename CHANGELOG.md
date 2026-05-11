# Changelog

### 1.1.0 - R.E.P.O. 0.4 Update

- Updated for R.E.P.O. 0.4, the "Cosmetics Update".
- Added new Audio config options `TargetedVolume` and `NotTargetedVolume`, which are played when Loom begins to target a player or loses interest in them, respectively.
- Removed `FixGlobalClapAudio` config and patch, since the bug is now fixed in vanilla.
- Various code and config tweaks.
- Upgraded BepInEx dependency.

The config has slightly changed, so I suggest deleting/resetting it.

### 1.0.1

- "Fixed" config entry `EnableDebug` somehow being true by default, despite it CLEARLY being declared as false?? I have no clue what happened here...
- Updated Discord URLs in README to link to this mod's dedicated thread now.

### 1.0.0 - Initial release 🧟‍♀️

- Introduced configuration settings for:
  - `MaxHealth`
  - `ClapPlayerDamage` and `ClapEnemyDamage`
  - `MovementSpeed` and `MovementSpeedLeave`
  - `PlayerLookDistance`
  - `ScreenEffectShowHands` and `ScreenEffectShowVeins`
  - `IdleLoopVolume`
  - `FixGlobalClapAudio`
- Network synchronization for ClapPlayerDamage in multiplayer.
