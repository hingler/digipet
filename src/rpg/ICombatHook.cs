using digipet.rpg.context;
using digipet.rpg.unit;

namespace digipet.rpg;

#nullable enable

public interface ICombatHook {
  // returns teammates, without self.
  IReadOnlyList<ICharState> GetTeammates(ICharState self);
  IReadOnlyList<ICharState> GetEnemies(ICharState self);

  void EnqueueAttack(IDetailedCharState actor, double damage_fac, double knockback_fac, ICharState? target = null);
  void EnqueueBuff(IDetailedCharState actor, ICharBuff buff, ICharState? target = null);
  void EnqueueTeamBuff(IDetailedCharState actor, ICharBuff buff, UnitTeam team);
  void CreateEntity(IWorldEntity entity);
}