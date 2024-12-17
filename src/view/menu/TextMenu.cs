using System;
using System.Collections.Generic;
using System.Numerics;
using digipet.canvas;
using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.sprite.attrib;
using digipet.util;

namespace digipet.view.menu;

#nullable enable

public class TextMenu : ViewComponent {
  private readonly SubCanvasView subCanvas;
  private readonly CompoundView text_content;
  private readonly IList<KeyValuePair<string, Action<int>?>> items = [];
  private readonly IList<Text> text_nodes = [];
  private readonly Lerper lerper_offset;
  private readonly Lerper lerper_selector;
  private readonly FontType type;

  private readonly SpriteView menu_sprite;

  const float MARGIN_PX = 8.0f;

  private int item_offset;

  public TextMenu(
    IEngine engine,
    FontType type
    ) {
    subCanvas = new(engine);
    lerper_offset = new(12.5);
    lerper_selector = new(25.0);
    
    this.type = type;

    text_content = new();
    subCanvas.AddView(text_content);
    
    item_offset = 0;

    ISprite sprite = engine.GetSpriteFetcher().GetSprite(SpriteID.ICON_SELECTOR);

    {
      menu_sprite = new() {
        sprite = sprite,
        Anchor = new(0.0f, 0.5f),
        OffsetPx = new(MARGIN_PX / 2.0f, 0.0f),
        SizePx = sprite.Dims
      };

      subCanvas.AddView(menu_sprite);
    }
  }

  public void AddItem(string name, Action<int>? callback = null) {
    double offset = GetTextOffsetPx(items.Count);
    items.Add(new(name, callback));

    Text text = new() {
      Font = type,
      Content = name,
      SizePx = new(192, 25),
      Color = Vector4.UnitW
    };

    text_nodes.Add(text);
    text_content.AddView(text);

    QueueReflow();
  }

  public void IncrementSelector() {
    item_offset = Math.Min(items.Count - 1, item_offset + 1);
    UpdateLerpers();
  }

  public void DecrementSelector() {
    item_offset = Math.Max(item_offset - 1, 0);
    UpdateLerpers();
  }

  public void ConfirmSelector() {
    // run associated callback
    items[item_offset].Value?.Invoke(item_offset);
  }

  // need to grab selector

  private void UpdateLerpers() {
    double lastTarget = lerper_offset.Target;
    double minOffset = GetMinOffsetPx(item_offset);
    double maxOffset = GetMaxOffsetPx(item_offset);

    if (lastTarget < minOffset || lastTarget > maxOffset) {
      double newTarget = Math.Clamp(lastTarget, minOffset, maxOffset);
      lerper_offset.Target = newTarget;
    }

    lerper_selector.Target = item_offset;
  }

  public override void Tick(double delta) {
    lerper_offset.Tick(delta);
    lerper_selector.Tick(delta);
  }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);

    double net_height = GetTextOffsetPx(items.Count - 1) + MARGIN_PX;
    double local_height = PixelSizeY;

    double menu_shift = Math.Max((local_height - net_height) / 2.0, 0.0);

    for (int i = 0; i < text_nodes.Count; i++) {
      Text text = text_nodes[i];
      double offset = GetTextOffsetPx(i);
      text.OffsetPx = new(2 * MARGIN_PX, (float)(offset + menu_shift));
    }
  }

  public override void Draw(ICanvas canvas) {
    // do some layout here
    float menu_px = (float)lerper_offset.Cursor;
    text_content.PixelY = -menu_px - 0.1f;
    menu_sprite.PixelY = (float)GetSelectorOffset(lerper_selector.Cursor) - menu_px - 0.6f;
    subCanvas.PreDraw(canvas);
  }

  // implementation
  private double GetSelectorOffset(double offset) {
    double net_height = GetTextOffsetPx(items.Count - 1) + MARGIN_PX;
    double local_height = PixelSizeY;
    double menu_shift = Math.Max((local_height - net_height) / 2.0, 0.0);

    return GetTextOffsetPx(offset) - (int)(GetTextHeightPx() / 2) + menu_shift;
  }

  private double GetTextOffsetPx(double offset) {
    return (GetTextHeightPx() + MARGIN_PX) * (offset + 1);
  }

  private float GetTextHeightPx() {
    return (float)type;
  }

  private double GetMinOffsetPx(double index) {
    // offset of bottom of this element
    double pos = GetTextOffsetPx(index) + MARGIN_PX;
    // return that position, - Y
    // don't go negative
    return Math.Max(pos - subCanvas.GetSubCanvasSize().Y, 0);
    
  }

  private double GetMaxOffsetPx(double offset) {
    // returns positional offset for this item
    double abs_max = GetTextOffsetPx(items.Count - 1) + MARGIN_PX;
    return Math.Min(GetTextOffsetPx(offset - 1), abs_max);
  }
}