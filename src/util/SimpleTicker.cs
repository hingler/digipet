namespace digipet.util;

public class SimpleTicker(double tick_delta) {
  double dt = 0.0;
  int updates = 0;

  public double TickDelta => tick_delta;

  public bool Update(double delta) {
    dt += delta;
    if (dt > tick_delta) {
      dt -= tick_delta;
      ++updates;
      return true;
    }

    return false;
  }

  public int GetFrameCount() {
    return updates;
  }

  public void Reset() {
    dt = 0.0;
    updates = 0;
  }
}