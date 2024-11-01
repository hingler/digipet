using System.Numerics;
using digipet.component;
using digipet.image;
using digipet.util;
using digipet.view;

namespace digipet.sprite;

public class SimplePetSprite : ViewComponent {
  public double frame_time = 0.5;
  private double dt_acc = 0.0;
  private readonly IAnimatedSprite sprite;

  // thinking: maintain a pixel scale??
  // (ex. 2 sprite-px per screenpx)

  private readonly ILogger logger = LoggerSingleton.GetLogger();

  public SimplePetSprite(IAnimatedSprite sprite) {
    this.sprite = sprite;
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    dt_acc += delta;
    if (dt_acc >= frame_time) {
      dt_acc -= frame_time;
      sprite.Increment();
      logger.Log("view sprite: ", sprite.Dims.X, ", ", sprite.Dims.Y);
    }
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    // sprite centered in rect with 2 sprite-px per screen-px
    Vector2 pixel_size = canvas.GetPixelDims();
    Vector2 screen_size = pixel_size * 2.0f * sprite.Dims;

    Vector2 screen_start = new(0.5f - (screen_size.X / 2.0f), 0.8f - screen_size.Y);

    canvas.Tex(sprite, screen_start, screen_start + screen_size, false);
  }
}