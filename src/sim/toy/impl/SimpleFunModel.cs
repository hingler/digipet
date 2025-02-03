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

    double consume_capacity = GetConsumeCapacity(tick_seconds);
    List<IToy> toys = [.. toy_model.GetActiveToys()];
    if (toys.Count > 0) {
      double consume_i = consume_capacity / toys.Count;

      double consume_acc = 0.0;
      foreach (IToy toy in toys) {
        double net_consume = toy_model.ConsumeInterest(toy, consume_i);
        if (net_consume <= 0.0) {
          consume_acc -= consume_i / 6;
        } else {
          consume_acc += net_consume;
        }
      }

      Fun += consume_acc;
    }
  }

  private double GetConsumeCapacity(int tick_seconds) {
    double min_capacity = 0.005;
    double extra_capacity = Math.Max((Fun - 0.3) / 50, 0);

    double net_consume = (min_capacity + extra_capacity) * tick_seconds;

    return Math.Min(1.0 - Fun, net_consume);
  }
}