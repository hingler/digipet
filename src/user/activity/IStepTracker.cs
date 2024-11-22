using digipet.file.stream;

namespace digipet.user.activity;

public interface IStepTracker : IStreamable {
  // fetches stap count according to save data
  public int GetStepCount(DateTime day);
  public int GetStepCount(DateOnly day);
  public int GetStepCount(int dayNumber);
}