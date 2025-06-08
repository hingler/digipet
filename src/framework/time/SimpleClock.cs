namespace digipet.framework.time;

public class SimpleClock : ISystemClock {
  public long GetTimeUsec() {
    return (DateTime.Now.Ticks - 621355968000000000) / 10;
  }
}