namespace digipet.sim.toy;

public class SimpleFunModel : IFunModel {
  private double fun_ = 0.5;
  public double Fun {
    get => fun_;
    set => fun_ = Math.Clamp(value, 0, 1);
  }

  private readonly IToyModel toy_model;

  private const int DECAY_RATE = 6 * 60 * 60;

  public SimpleFunModel(IToyModel toy_model) {
    this.toy_model = toy_model;
  }

  public void Tick(int tick_seconds) {
    Fun -= (double)tick_seconds / DECAY_RATE;
    Fun += toy_model.ConsumeInterest(tick_seconds);
  }

  private double GetConsumeCapacity(int tick_seconds) {
    double min_capacity = 0.005;
    double extra_capacity = Math.Max((Fun - 0.3) / 50, 0);

    double net_consume = (min_capacity + extra_capacity) * tick_seconds;

    return Math.Min(1.0 - Fun, net_consume);
  }
}