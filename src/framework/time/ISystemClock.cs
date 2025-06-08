namespace digipet.framework.time;

public interface ISystemClock {
  // returns long timestamp, let's say number of millis since some arbitrary epoch
  
  long GetTimeUsec();
}