using digipet.rpg.unit;

namespace digipet.rpg.context;

public interface IDetailedCharState : ICharState {

  // calculations
  void OnHit(double raw_damage, double raw_knockback);

  // calculation opportunity - or just accept raw
  void OnHeal(double raw_heal);

  // raw, just pass down to buff mgr for now
  void OnBuff(ICharBuff buff);
}