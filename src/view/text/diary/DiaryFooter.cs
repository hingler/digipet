using System.Drawing;
using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.util;

namespace digipet.view.text.diary;

public class DiaryFooter : ViewComponent {
  public string Content = "";

  public FontType Font = FontType.TINY;

  public HorizontalAlign Alignment = HorizontalAlign.RIGHT;

  private int margin_px_ = 2;

  public int MarginPx {
    get => margin_px_;
    set {
      margin_px_ = value;
      QueueReflow();
    }
  }

  private readonly IFontHelper helper;

  public DiaryFooter(IEngine engine) {
    helper = engine.GetFontHelper();
  }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);
  }

  public override void Draw(ICanvas canvas) {
    // my idea
    // - text is placed st text descent is `ascent + margin` px below baseline
    // - and also `margin` px from the left of the element
    // - 

    float baseline_px = MarginPx + FontSizeHandler.GetFontAscent(Font);
    float text_origin = Alignment switch {
      HorizontalAlign.CENTER => 0.5f,
      HorizontalAlign.RIGHT => 1.0f,
      _ => 0.0f,
    };

    // right: l coord defined
    // center: l and r coords defined
    // left: r coord defined only

    Vector2 canvas_size = canvas.GetSizePx();

    Vector2 text_size = helper.GetStringSizePx(Content, Font, 1.0f);
    float box_start = (canvas_size.X * text_origin) - (text_origin * text_size.X) - MarginPx;
    box_start = MathF.Floor(box_start);
    float box_end = box_start + text_size.X + 2.0f * MarginPx;
    box_end = MathF.Ceiling(box_end);

    Vector2 text_pos = canvas.PxToRelative(box_start + MarginPx + 0.1f, baseline_px);

    canvas.Rect(
      new Vector2(box_start / canvas_size.X, 0.0f),
      new Vector2(box_end / canvas_size.X, 1.0f),
      3, DigiColor.BLACK
    );

    canvas.Text(text_pos, Content, 1.0f, Font, HorizontalAlign.LEFT, DigiColor.WHITE);
  }
}