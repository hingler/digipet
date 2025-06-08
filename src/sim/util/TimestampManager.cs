using digipet.db;
using digipet.framework;
using digipet.framework.time;
using digipet.util;

namespace digipet.sim.util;

#nullable enable

public class TimestampManager {
  // store (on deactivate)
  // connect with a repo key
  // load delta (on activate)

  private readonly IDataStore data;
  private readonly string timestamp_name;
  private readonly ISystemClock clock;
  private long last_timestamp = 0;

  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<TimestampManager>();

  public TimestampManager(
    IEngine engine,
    IDataStore data,
    string timestamp_name
  ) {
    this.data = data;
    this.timestamp_name = timestamp_name;
    clock = engine.GetClock();

    if (data.TryFetchLong(timestamp_name, out long t)) {
      // stores last time stamp at which data was saved
      last_timestamp = t;
    } else {
      // doesn't exist - assume no time passed
      logger.Error("timestamp with key ", timestamp_name, " could not be loaded - using current time...");
      Update();
    }
  }

  public long Update() {
    // store current time
    long timestamp = clock.GetTimeUsec();
    data.StoreLong(timestamp_name, timestamp);
    last_timestamp = timestamp;

    logger.Log("timestamp updated - ", timestamp);

    return last_timestamp;
  }

  public long UpdateAndGetDelta() {
    long stamp_old = last_timestamp;
    long stamp_new = Update();
    return stamp_new - stamp_old;
  }
}