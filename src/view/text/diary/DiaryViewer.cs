// like the diary editor

// support init offset, and scroll

// add header padding, and footer padding

// - feedback for cursor placement
// - expose flow text, or accept our own flow

using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.util;

namespace digipet.view.text.diary;

public class DiaryViewer : ViewComponent {
  private readonly ITextFlow flow_handler;
  private readonly FlowText text;

  public int HeaderLines = 2;
  public int FooterLines = 2;
  public float PageOffset = 0.0f;

  public string HeaderText = "Nov. 29 2024";

  public FontType Font {
    get => text.Font;
    set {
      text.Font = value;
    }
  }

  public DigiColor Color {
    get => text.Color;
    set => text.Color = value;
  }

  private int margin_px_ = 0;
  public int MarginPx {
    get => margin_px_;
    set {
      margin_px_ = value;
      QueueReflow();
    }
  }

  // get line count
  // limit baselines based on effective line count

  public DiaryViewer(ITextFlow flow_handler) : base() {
    this.flow_handler = flow_handler;
    text = new FlowText(flow_handler) {
      ZIndex = 2
    };

    AddView(text);
  }

  public DiaryViewer(IEngine engine, string content) : this(new TextFlowHandler(engine)) {
    flow_handler.Content = content;
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    float pos_offset = Math.Max(HeaderLines - PageOffset, 0.0f);
    float flow_offset = Math.Max(PageOffset - HeaderLines, 0.0f);

    float line_size = flow_handler.GetLineHeight();

    text.PixelY = pos_offset * line_size;
    text.DisplayedLineOffset = flow_offset;
  }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);

    text.PixelSizeX = canvas.GetSizePx().X - 2 * MarginPx;
    text.PixelX = MarginPx;
  }

  

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    DrawBackgroundRect(canvas);

    float line_offset = PageOffset - HeaderLines;
    float net_offset = line_offset;

    if (net_offset > 0.0f) {
      net_offset %= 1.0f;
    } else {
      line_offset = 0.0f;
    }

    int line_count = flow_handler.GetLines().Count;
    int baseline_offset = (int)Math.Floor(line_offset);
    int max_baselines = line_count + FooterLines;

    float header_baseline = flow_handler.GetTextBaseline(0, net_offset + 1);
    
    if (net_offset <= 0.0f) {
      Vector2 header_start = canvas.PxToRelative(canvas.GetSizePx().X - MarginPx, header_baseline);
      canvas.Text(header_start, HeaderText, 1.0f, FontType.TINY, HorizontalAlign.RIGHT, new DigiColor(0.5f));
    }

    for (int i = 0; i < 20; i++) {
      int abs_line = i + baseline_offset;
      
      if (abs_line > max_baselines) {
        break;
      }

      float baseline = flow_handler.GetTextBaseline(i, net_offset + 1);
      DrawBaseline(canvas, baseline);
    }
  }

  public int GetNetLineCount() {
    return HeaderLines + flow_handler.GetLines().Count + FooterLines;
  }

  public float GetPageStartRel() {
    return Math.Max(-PageOffset * flow_handler.GetLineHeight(), 0.0f);
  }

  public float GetPageEndRel() {
    float net_lines = GetNetLineCount();
    return Math.Min((-PageOffset + net_lines) * flow_handler.GetLineHeight(), PixelSizeY);
  }

  private void DrawBackgroundRect(ICanvas canvas) {
    float page_start = GetPageStartRel();
    float page_end = GetPageEndRel();

    if ((page_end - page_start) > 0.0001f) {
      Vector2 start = new(0.0f, page_start * canvas.GetPixelDims().Y);
      Vector2 end = new(1.0f, page_end * canvas.GetPixelDims().Y);

      canvas.Rect(start, end, 0, DigiColor.WHITE, 0);
    }

  }

  private void DrawBaseline(ICanvas canvas, float base_px) {
    Vector2 start = canvas.PxToRelative(0.0f, base_px);
    Vector2 end = canvas.PxToRelative(canvas.GetSizePx().X, base_px);

    if (base_px >= 0 && base_px <= canvas.GetSizePx().Y) {
      canvas.Line(start, end, 1.0f, DigiColor.BLACK.WithOpacity(0.25f), 1.0f, 1);
    }

  }

}