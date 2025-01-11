# rpg models
- idea: delegate conversions from "stats" to "effects" to certain models

- combatmgr: convert all abilities (projectiles, close range, etc) into net power/knock
- combatmgr: no conversion for buffs - pass them directly along to their targets
- combatmgr: convert heals (probably just leave it 1:1)
- char: convert raw power/knock to effective power/delta-v


## abilities: encoding power info
- 