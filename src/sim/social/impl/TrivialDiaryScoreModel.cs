// give everything a 0.5

using digipet.diary;
using digipet.util;

namespace digipet.sim.social;

public class TrivialDiaryScoreModel : IDiaryScoreModel {
  private double acc = 0.0;
  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<TrivialDiaryScoreModel>();

  public double Compute(ITextEditor entry) {
    acc += 0.5;
    return 0.5;
  }

  public double Peek() => acc;

  public double Flush() {
    double temp = acc;
    acc = 0;
    logger.Log("flushed social of ", temp);
    return temp;
  }
}