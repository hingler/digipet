using System.Numerics;
using digipet.canvas;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.sprite.attrib;
using digipet.util;

namespace digipet.view.menu;

#nullable enable

public class ComponentMenu : ViewComponent {
  private readonly SubCanvasView sub_canvas;
  private readonly CompoundView container;
  private readonly IList<KeyValuePair<ViewComponent, Action<int>?>> items = [];
  private readonly Lerper lerper_offset;
  private readonly Lerper lerper_selector;

  private readonly SpriteView selector_sprite;

  private float margin_ = 8.0f;

  public float Margin {
    get => margin_;
    set {
      margin_ = value;
      QueueReflow();
    }
  }

  private int item_offset;

  public ComponentMenu(
    IEngine engine
  ) {
    sub_canvas = new(engine);
    container = new();
    lerper_offset = new(12.5);
    lerper_selector = new(25.0);

    sub_canvas.AddView(container);

    item_offset = 0;

    ISprite sprite = engine.GetSpriteFetcher().GetSprite(SpriteID.ICON_SELECTOR);

    {
      selector_sprite = new() {
        sprite = sprite,
        Anchor = new(0.0f, 0.5f),
        OffsetPx = new(margin_ / 2.0f, 0.0f),
        SizePx = sprite.Dims
      };

      sub_canvas.AddView(selector_sprite);
    }

    UpdateLerpers();
  }

  public override IReadOnlyList<ViewComponent> GetChildren() {
    return [ sub_canvas ];
  }

    public void AddItem(ViewComponent element, Action<int>? callback = null) {
    // set anchor point to TL
    double offset = GetOffsetPx(items.Count);
    items.Add(new(element, callback));

    element.Anchor = Vector2.Zero;
    element.SizeX = 1.0f;

    container.AddView(element);

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

  public int GetSelected() {
    return item_offset;
  }

  public void ConfirmSelector() {
    // run associated callback
    items[item_offset].Value?.Invoke(item_offset);
  }

  public override void Tick(double delta) {
    lerper_offset.Tick(delta);
    lerper_selector.Tick(delta);

    float menu_px = (float)lerper_offset.Cursor;
    container.PixelY = -menu_px;
    selector_sprite.PixelY = (float)lerper_selector.Cursor - menu_px;
  }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);
    container.PixelSizeX = canvas.GetSizePx().X - 2.0f * margin_;
    container.PixelX = 2.0f * margin_;

    for (int i = 0; i < items.Count; i++) {
      items[i].Key.PixelY = (float)GetOffsetPx(i);
      items[i].Key.SizeX = 1.0f;
    }

    UpdateLerpers();
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    for (int i = 1; i < items.Count; i++) {
      double line_start = GetOffsetPx(i) - Math.Round(margin_ / 2) - lerper_offset.Cursor;
      Vector2 start = canvas.PxToRelative(3.5f * margin_, (float)line_start);
      Vector2 end = canvas.PxToRelative(canvas.GetSizePx().X - 1.5f * margin_, (float)line_start);
      
      if (start.Y > 0.0f && start.Y < 1.0f) {
        canvas.Line(start, end, 1.0f, DigiColor.BLACK.WithOpacity(0.3f), 0);
      }
    }
  }

    private double GetSelectorOffset(int offset) {
    return GetOffsetPx(offset) + GetItemHeight(offset) / 2;
  }

  private double GetItemHeight(int offset) {
    if (offset < 0 || offset >= items.Count) {
      return 0.0;
    }

    return items[offset].Key.PixelSizeY;
  }

  private double GetOffsetPx(int offset) {
    double net_offset = margin_;
    for (int i = 0; i < offset; i++) {
      net_offset += GetItemHeight(i) + margin_;
    }

    return net_offset;
  }

  private void UpdateLerpers() {
    double lastTarget = lerper_offset.Target;
    double minOffset = GetMinOffsetPx(item_offset);
    double maxOffset = GetMaxOffsetPx(item_offset);

    if (lastTarget < minOffset || lastTarget > maxOffset) {
      double newTarget;
      if (minOffset > maxOffset) {
        // menu offset
        // only applicable in cases where sub canvas size is LT margin
        newTarget = GetMaxOffsetPx(0);
      } else {
        newTarget = Math.Clamp(lastTarget, minOffset, maxOffset) + 0.1;
      }
      lerper_offset.Target = newTarget;
    }

    // make our target the pixel offset of the desired item
    lerper_selector.Target = GetSelectorOffset(item_offset);
  }

  private double GetMinOffsetPx(int index) {
    // offset of bottom of this element
    double pos = GetOffsetPx(index) + GetItemHeight(index) + margin_;
    // return that position, - Y
    // don't go negative
    return Math.Max(pos - sub_canvas.GetSubCanvasSize().Y, 0);
  }

  private double GetMaxOffsetPx(int offset) {
    // returns positional offset for this item
    double abs_max = GetOffsetPx(items.Count - 1) + margin_;
    return Math.Min(GetOffsetPx(offset - 1) - margin_, abs_max);
  }
}