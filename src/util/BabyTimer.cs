namespace digipet.util;

public class BabyTimer {
  private readonly Dictionary<string, long> timers = [];

  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<BabyTimer>();

  public BabyTimer() {}

  public void Start(string name) {
    timers.Remove(name);
    timers.Add(name, DateTime.UtcNow.Ticks);
  }

  public double End(string name, bool print = true) {
    if (timers.TryGetValue(name, out long val)) {
      // 100ns ticks to ms
      long tick_count = DateTime.UtcNow.Ticks - val;
      double time_ms = tick_count / 10000.0;

      if (print) {
        logger.Log("runtime for ", name, ": ", time_ms.ToString("#.000"), "ms");
      }

      return time_ms;
    }

    return -1.0;
  }
}