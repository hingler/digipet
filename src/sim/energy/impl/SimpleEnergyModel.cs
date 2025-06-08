namespace digipet.sim.energy.impl;

public class SimpleEnergyModel : IEnergyModel {
  private double energy_ = 0.5;
  public double Energy {
    get => energy_;
    set => energy_ = Math.Clamp(value, 0.0, 1.0);
  }

  private readonly ILightModel light_model;

  private const int DECAY_RATE = 60 * 60 * 27;
  private const int FILL_RATE = 60 * 60 * 6;
  private const int MIN_LOOKBACK = 20;

  public SimpleEnergyModel(ILightModel light_model) {
    this.light_model = light_model;
  }

  public void Tick(int tick_count) {
    // perform peek
    double tick_d = tick_count;
    if (tick_count > MIN_LOOKBACK) {
      int time_lit = light_model.GetTimeSpentLit(tick_count);
      Energy += (double)time_lit / FILL_RATE;
    } else if (light_model.IsLit()) {
      Energy += tick_d / FILL_RATE;
    }

    Energy -= tick_d / DECAY_RATE;

  }
}