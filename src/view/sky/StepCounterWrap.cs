using digipet.canvas.font;
using digipet.component;
using digipet.sprite.attrib;

namespace digipet.view.sky;

public class StepCounterWrap : ViewComponent {
  private readonly SkyCounter counter;

  private int step_count_;
  public double StepCount {
    get => step_count_;
    set {
      step_count_ = (int)value; 
      counter.Content = step_count_.ToString();
    }
  }

  public StepCounterWrap(ISpriteFetcher fetcher) {
    counter = new(fetcher.GetSprite(SpriteID.ICON_STEP), FontType.LARGE) {
      ScaleFactor = 2
    };
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    counter.PreDraw(canvas);
  }
}