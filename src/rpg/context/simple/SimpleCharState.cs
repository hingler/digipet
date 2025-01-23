using System.Numerics;
using digipet.rpg.model;
using digipet.rpg.model.demo;
using digipet.rpg.unit;
using digipet.rpg.unit.ability;
using digipet.util;

namespace digipet.rpg.context.simple;

public class SimpleCharState : IDetailedCharState {
  private readonly SimpleBuffManager buffs;
  private readonly IMaxHPCalculator hp_calc;
  private readonly IPowerConverter power_conv;
  private readonly ISpeedCalculator speed_calc;
  private readonly List<SimpleCharAbility> abilities;
  private long current_hp;
  public ICharStats Stats => buffs.Stats;

  public IBehaviorModel BehaviorModel = new TrivialBehaviorModel();

  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<SimpleCharState>();
  public long CurrentHP {
    get => current_hp;
    set => current_hp = Math.Clamp(value, 0, MaxHP);
  }
  public long MaxHP {
    get {
      long res = hp_calc.GetMaxHP(Stats);
      TruncateHP(res);

      return res;
    }
  }

  public float Width => buffs.Stats.Width;

  public SimpleCharState(
    ICharStats base_stats,
    UnitTeam team,
    IMaxHPCalculator calculator,
    IPowerConverter converter_power,
    ISpeedCalculator calculator_speed,
    Vector2 init_position
  ) {
    buffs = new(base_stats);
    Team = team;
    Position = init_position;
    hp_calc = calculator;

    current_hp = MaxHP;

    power_conv = converter_power;
    speed_calc = calculator_speed;

    abilities = [];
    foreach (IAbility ability in base_stats.GetAbilities()) {
      abilities.Add(new SimpleCharAbility(ability));
    }
  }

  private void TruncateHP(long max) {
    if (current_hp > max) {
      current_hp = max;
    }
  }

  public void Tick(double delta) {
    foreach (SimpleCharAbility ability in abilities) {
      ability.Tick(delta);
    }
  }

  public UnitTeam Team { get; }
  // double -> vec2

  // tba: need to handle velocity
  public Vector2 Position { get; set; }
  public Vector2 Velocity { get; set; } = Vector2.Zero;

  public IReadOnlyList<ICharAbility> GetAbilities() {
    return abilities;
  }

  // alt: return net damage?
  // don't broadcast knockback - let the manager take care of it (ie: we shouldn't have to know it)
  public void OnHit(double raw_damage) {
    double net_damage = power_conv.ToNetDamage(Stats, raw_damage);
    CurrentHP -= (long)net_damage;
  }

  // alt: return net heal?
  public void OnHeal(double raw_heal) {
    CurrentHP += (long)raw_heal;
  }

  public void OnBuff(ICharBuff buff) {
    buffs.ApplyBuff(buff);
  }
}