using System;

namespace digipet.sim.water;

public class SimpleThirstModel : IThirstModel {
  private double water_content;
  private readonly IWaterSource source;
  public double Quenchiness {
    get => water_content;
  }

  private static readonly int DECAY_TIME = 60 * 60 * 15;
  private static readonly int MIN_IDLE = 60 * 60 * 1;
  // units of water constituting max quenched
  private static readonly double WATER_CAPACITY = 2.5;
  private static readonly double WATER_TARGET = 0.8;
  private static readonly double WATER_MIN = 0.45;

  private static readonly double WATER_CRITICAL = 0.2;

  public SimpleThirstModel(double init_water, IWaterSource source) {
    this.source = source;
    water_content = init_water;
  }

  public void Tick(int tick_seconds) {
    double net_water = water_content - ((double)tick_seconds / DECAY_TIME);
    if (net_water < WATER_CRITICAL && tick_seconds > MIN_IDLE) {
      // amount of water we want to aim for
      double water_deficit = WATER_CRITICAL - net_water;

      // drink until thirst is assuaged, or until we hit target, based on present thirst
      // (using present deficit as an indicator of water consumed overtime)
      double drink_target = Math.Min(water_deficit + WATER_MIN, WATER_TARGET);

      double water_target = WATER_CAPACITY * drink_target;
      // amount of water we'd need to drink to hit that target
      double water_delta = water_target - net_water;
      Drink(water_delta, source);
    }
  }

  public double Drink(double units, IWaterSource source) {
    double init_contents = water_content * WATER_CAPACITY;
    double max_consumable = WATER_CAPACITY - water_content;
    double net_consumed = Math.Min(units, max_consumable);
    double net_contents = source.Drink(Math.Min(units, max_consumable)) + init_contents;
    water_content = Math.Clamp(net_contents, 0.0, WATER_CAPACITY) / WATER_CAPACITY;

    return net_consumed;
  }
}