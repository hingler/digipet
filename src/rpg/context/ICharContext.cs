using digipet.rpg.unit;

namespace digipet.rpg.context;

#nullable enable

public interface ICharContext {
  ICharState Self { get; }

  // whether or not this char should be advancing or retreating
  public float Direction { get; set; }

  IReadOnlyList<ICharState> GetAllies();
  IReadOnlyList<ICharState> GetEnemies();
  IReadOnlyList<ICharAbility> GetAbilities();

  // performs an attack on the specified target, else all enemies.
  public void Attack(double damage_fac, double knockback_fac, ICharState? target = null);

  // performs a buff on the given target.
  public void Buff(ICharBuff buff, ICharState? target = null);

  // buff all targets on a given team.
  public void Buff(ICharBuff buff, UnitTeam team);

  // spawns a new entity which will be tracked by the engine.
  public void CreateEntity(IWorldEntity entity);
}