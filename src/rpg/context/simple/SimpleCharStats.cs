using digipet.rpg.unit;

namespace digipet.rpg.context.simple;

public class SimpleCharStats : ICharStats {
  public long Attack { get; set; } = 0;
  public long Wisdom { get; set; } = 0;
  public long Defense { get; set; } = 0;
  public long Weight { get; set; } = 0;
  public long Vitality { get; set; } = 0;
  public long Speed { get; set; } = 0;

  public float Width { get; set; } = 0.1f;

  public List<IAbility> Abilities = [];

  public IReadOnlyList<IAbility> GetAbilities() => Abilities.AsReadOnly();

  public void Scale(ICharStats fac) {
    Attack *= fac.Attack;
    Wisdom *= fac.Wisdom;
    Defense *= fac.Defense;
    Weight *= fac.Weight;
    Vitality *= fac.Vitality;
    Speed *= fac.Speed;
  }

  public void Add(ICharStats fac) {
    Attack += fac.Attack;
    Wisdom += fac.Wisdom;
    Defense += fac.Defense;
    Weight += fac.Weight;
    Vitality += fac.Vitality;
    Speed += fac.Speed;
  }

  public void Set(ICharStats fac) {
    Attack = fac.Attack;
    Wisdom = fac.Wisdom;
    Defense = fac.Defense;
    Weight = fac.Weight;
    Vitality = fac.Vitality;
    Speed = fac.Speed;

    Width = fac.Width;

    Abilities = [..fac.GetAbilities()];
  }

  public void Set(long value) {
    Attack = value;
    Wisdom = value;
    Defense = value;
    Weight = value;
    Vitality = value;
    Speed = value;
  }
}