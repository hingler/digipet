using System.Numerics;
using digipet.component;
using digipet.image;

namespace digipet.view.bg;

public class ScrollingBG : ViewComponent {
  private readonly ISprite sprite;

  public double ScrollSpeed = 0.5;

  private double time = 0.0f;

  private double ScrollDelta => 1.0 / Math.Max(ScrollSpeed, 0.0001);

  public ScrollingBG(ISprite sprite) {
    this.sprite = sprite;
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    time += delta;
    if (time >= ScrollDelta) {
      time -= ScrollDelta;
    }
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    Vector2 size_rel = sprite.Dims / canvas.GetSizePx();

    Vector2 net_offset = size_rel * (float)(time / ScrollDelta);


    canvas.Tex(
      sprite,
      new Vector2(-size_rel.X, 0.0f) + new Vector2(net_offset.X, 0.0f),
      Vector2.One + new Vector2(net_offset.X, 0.0f),
      true
    );
  }
}