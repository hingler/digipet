// number of units affected by an ability
public enum AbilitySpread {
  SINGLE,
  MULTI
}

// whether allies or enemies should be targeted when this ability is used
public enum UnitTeam {
  ALLY,
  ENEMY
}

// the means by which this ability should be used
public enum AbilityFormat {
  // attack on contact - ie a sword slash, a punch, etc.
  Physical,

  // attack via a spawned projectile - ie an arrow, an orb, etc.
  Projectile,

  // aural, no medium of contact - ie a buff/debuff, a heal, etc.
  Augment

  // btw this doesn't matter anymore
}