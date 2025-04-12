using digipet.image;
using digipet.sprite;

namespace digipet.view.rpg.overworld;

public class SpriteAnimator {
  private readonly IAnimatable animatable;
  private readonly FrameTicker ticker;

  public SpriteAnimator(
    IAnimatable sprite,
    double frame_time
  ) {
    animatable = sprite;
    ticker = new(frame_time, sprite.GetFrameCount());
  }

  public bool Tick(double delta) {
    bool ticked = ticker.Tick(delta);
    if (ticked) {
      animatable.Frame = ticker.Frame;
    }

    return ticked;
  }
}