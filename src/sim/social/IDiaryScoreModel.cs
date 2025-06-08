using digipet.diary;
using digipet.diary.store;

namespace digipet.sim.social;

public interface IDiaryScoreModel {
  // computes the score of this entry and adds it to buffer
  // returns computed score
  public double Compute(ITextEditor entry);
  
  // peeks current capacity, does not flush
  public double Peek();
  
  // asyncs lole
  // flushes and sets to 0.
  public double Flush();
}