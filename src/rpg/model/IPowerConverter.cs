using digipet.rpg.context;

namespace digipet.rpg.model;

// converts between local power factor, and global "raw power"

// alt: add some account for magic which does damage based on wisdom instead of atk
public interface IPowerConverter {
  // converts from a power factor, to raw power
  double ToRawDamage(ICharStats stats, double damage_fac);
  double ToRawKnockback(ICharStats stats, double knockback_fac);

  // converts from raw power, to a net damage dealt
  double ToNetDamage(ICharStats stats, double raw_damage);
}

// tba:
// - integrate power converters
// - start doing some damage tests