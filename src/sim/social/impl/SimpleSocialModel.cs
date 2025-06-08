namespace digipet.sim.social.impl;

public class SimpleSocialModel : ISocialModel {
  private readonly IDiaryScoreModel diary_model;

  private double social_ = 0.5;
  public double Social {
    get => social_;
    set => social_ = Math.Clamp(value, 0.0, 1.0);
  }
  // time conversion, to make this relationship clearer?
  private const int DECAY_RATE = 60 * 60 * 42;
  public SimpleSocialModel(IDiaryScoreModel diary_model) {
    this.diary_model = diary_model;
  }

  public void Tick(int tick_count) {
    Social -= (double)tick_count / DECAY_RATE;
    if (diary_model.Peek() > 0.001) {
      Social += diary_model.Flush();
    }
  }
}