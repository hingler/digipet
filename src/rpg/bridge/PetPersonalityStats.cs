using System.Runtime.CompilerServices;
using digipet.pet;
using digipet.rpg.context;
using digipet.sim;
using Godot;

namespace digipet.rpg.bridge;

struct StatTracker {
  // scales gt zero
  public double Buffs;

  // scales lt zero
  public double Debuffs;

  public double NetValue;

  public StatTracker() {
    Buffs = 0;
    Debuffs = 0;
    NetValue = 0;
  }

  public double AsDouble() {
    // center value
    double mean = (Buffs + Debuffs) / 2;
    // double magnitude = Buffs - Debuffs;

    return (NetValue - mean);
  }
}
static class StatHelper {
  public static StatTracker Add(double attrib, double scale) {
    StatTracker res = new();
    return res.Add(attrib, scale);
  }
  public static StatTracker Add(this StatTracker a, double attrib, double scale) {
    // weight around 0.5

    if (scale > 0) {
      a.Buffs += scale;
    } else {
      a.Debuffs += scale;
    }



    a.NetValue += (attrib * 2.0 - 1.0) * scale;
    return a;
  }

  public static StatTracker Sub(this StatTracker a, double attrib, double scale) => a.Add(attrib, -scale);
}

// base stats
public class PetPersonalityStats : ICharStatsBase {
  private readonly IPetPersonality pers;
  private readonly double scale_factor;
  private readonly ICharStatsBase base_stats;

  public long Attack => (long)(Attack_D * scale_factor) + base_stats.Attack;
  public long Wisdom => (long)(Wisdom_D * scale_factor) + base_stats.Wisdom;
  public long Defense => (long)(Defense_D * scale_factor) + base_stats.Defense;
  public long Weight => (long)(Weight_D * scale_factor) + base_stats.Weight;
  public long Vitality => (long)(Vitality_D * scale_factor) + base_stats.Vitality;
  public long Speed => (long)(Speed_D * scale_factor) + base_stats.Speed;
  
  public PetPersonalityStats(
    IPetData pet,
    double scale_factor
  ) {
    pers = pet.Personality;
    this.scale_factor = scale_factor;
  }

  private double Attack_D => 
    StatHelper.Add(pers.Brave, 0.6)
    .Add(pers.Weird, 0.35)
    .Add(pers.Tough, 0.2)
    .Sub(pers.Charm, 0.2)
    .Sub(pers.Smart, 0.3)
    .Sub(pers.Quick, 0.1)
    .AsDouble()
  ;

  private double Wisdom_D =>
    StatHelper.Add(pers.Smart, 0.7)
    .Add(pers.Quick, 0.4)
    .Add(pers.Charm, 0.1)
    .Sub(pers.Tough, 0.6)
    .Sub(pers.Brave, 0.4)
    .Sub(pers.Weird, 0.3)
    .AsDouble()
  ;

  private double Defense_D =>
    StatHelper.Add(pers.Tough, 0.4)
    .Add(pers.Smart, 0.2)
    .Sub(pers.Brave, 0.4)
    .Sub(pers.Quick, 0.3)
    .Sub(pers.Weird, 0.2)
    .Sub(pers.Charm, 0.1)
    .AsDouble()
  ;

  private double Weight_D =>
    StatHelper.Add(pers.Tough, 0.45)
    .Add(pers.Brave, 0.25)
    .Add(pers.Weird, 0.1)
    .Add(pers.Charm, 0.05)
    .Sub(pers.Quick, 0.5)
    .Sub(pers.Smart, 0.3)
    .AsDouble()
  ;

  private double Vitality_D =>
    StatHelper.Add(pers.Brave, 0.35)
    .Add(pers.Charm, 0.35)
    .Add(pers.Tough, 0.2)
    .Sub(pers.Smart, 0.25)
    .Sub(pers.Quick, 0.25)
    .Sub(pers.Weird, 0.15)
    .AsDouble()
  ;

  private double Speed_D =>
    StatHelper.Add(pers.Quick, 0.8)
      .Add(pers.Weird, 0.3)
      .Sub(pers.Tough, 0.7)
      .Sub(pers.Brave, 0.3)
      .Sub(pers.Charm, 0.2)
      .Sub(pers.Smart, 0.05)
      .AsDouble()
    ;
}

