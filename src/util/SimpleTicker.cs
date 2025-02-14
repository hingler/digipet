namespace digipet.util;

public class SimpleTicker(double tick_delta) {
  double dt = 0.0;
  public int Updates = 0;

  public double TickDelta => tick_delta;

  public bool Update(double delta) {
    dt += delta;
    if (dt > tick_delta) {
      
      int update_count = (int)Math.Floor(dt / TickDelta);
      dt %= tick_delta;
      Updates += update_count;

      return true;
    }

    return false;
  }

  public int GetFrameCount() {
    return Updates;
  }

  public void Reset() {
    dt = 0.0;
    Updates = 0;
  }
}