using digipet.rpg.unit;

namespace digipet.rpg.context.simple;

#nullable enable

public class SimpleCharContext : ICharContext {
  private readonly ICombatHook hook;
  private readonly IDetailedCharState self_detailed;
  public ICharState Self => self_detailed;

  public SimpleCharContext(
    ICombatHook hook,
    IDetailedCharState self
  ) {
    this.hook = hook;
    self_detailed = self;
  }

  public float Direction { get; set; }

  public IReadOnlyList<ICharState> GetAllies() {
    return hook.GetTeammates(Self);
  }

  public IReadOnlyList<ICharState> GetEnemies() {
    return hook.GetEnemies(Self);
  }

  public IReadOnlyList<ICharAbility> GetAbilities() {
    return self_detailed.GetAbilities();
  }

  // how do we want to gauge attacks?
  // broadcast raw damage!!
  public void Attack(double damage_fac, double knockback_fac, ICharState? target = null) {
    hook.EnqueueAttack(self_detailed, damage_fac, knockback_fac, target);
  }

  public void Buff(ICharBuff buff, ICharState? target) {
    hook.EnqueueBuff(self_detailed, buff, target);
  }

  public void Buff(ICharBuff buff, UnitTeam team) {
    hook.EnqueueTeamBuff(self_detailed, buff, team);
  }

  public void CreateEntity(IWorldEntity entity) {
    hook.CreateEntity(entity);
  }
}