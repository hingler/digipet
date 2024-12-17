namespace digipet.transition.helper;

// pauses for some number of secs
public class PauseTransition : ITransition {
  private readonly double duration;
  private double dt;

  public PauseTransition(double duration) {
    this.duration = duration;
    dt = 0;
  }

  public void Tick(double delta) {
    dt += delta;
  }

  public void Advance() {
    dt = duration + 0.0001;
  }

  public bool Complete() {
    return dt > duration;
  }

  public bool BlockInput => false;
}