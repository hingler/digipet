using System.Numerics;
using digipet.rpg.unit;

namespace digipet.rpg.context;

public interface ICharState {
  ICharStats Stats { get; }
  long CurrentHP { get; }
  long MaxHP { get; }

  UnitTeam Team { get; }
  Vector2 Position { get; }
  Vector2 Velocity { get; }

  IReadOnlyList<ICharAbility> GetAbilities();
}