// simple model for turning on/off the lights

namespace digipet.sim.energy;

public interface ILightModel {
  // true if lights on, false if lights off
  bool IsLit();

  // should we implement force state here?
  void Toggle(bool state);

  // returns number of seconds for which light was lit (thinking: timers)
  int GetTimeSpentLit(int lookback_sec);

  void RegisterToggleListener(Action callback);
}