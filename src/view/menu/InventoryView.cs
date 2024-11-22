using System.Drawing;
using digipet.canvas.font;
using digipet.component;
using digipet.util;

namespace digipet.view.menu;


// view which displays tiny text on L and R
public class InventoryView : ViewComponent {
  private readonly FontType font_type;
  private readonly Text text_l;
  private readonly Text text_r;

  public string Name {
    get => text_l.Content;
    set => text_l.Content = value;
  }

  public string Datum {
    get => text_r.Content;
    set => text_r.Content = value;
  }

  public DigiColor TextColor {
    get => text_l.Color;
    set {
      text_l.Color = value;
      text_r.Color = value;
    }
  }


  public InventoryView() : this(FontType.TINY) {}
  public InventoryView(FontType type) : this(type, type) {}
  public InventoryView(FontType type_label, FontType type_datum) {
    font_type = type_label;
    text_l = new() {
      Alignment = HorizontalAlign.LEFT,
      Font = font_type
    };

    text_r = new() {
      Alignment = HorizontalAlign.RIGHT,
      Font = type_datum
    };

    AddView(text_l);
    AddView(text_r);
    // ensure it gets done
    QueueReflow();
  }

  // text should be centered
  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);

    float baseline_px = PixelSizeY - FontSizeHandler.GetFontDescent(font_type);

    text_l.OffsetPx = new(8.0f, baseline_px);
    text_r.OffsetPx = new(canvas.GetSizePx().X - 8.0f, baseline_px);

    // draw will handle the rest!
  }
}