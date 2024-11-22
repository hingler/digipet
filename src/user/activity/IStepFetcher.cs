namespace digipet.user.activity;

public interface IStepFetcher {
  // fetch step count from device (ex. from a fitness API)
  // (most likely: make call to android API)
  public int FetchStepCount(DateTime day);
  public int FetchStepCount(DateOnly day);
}