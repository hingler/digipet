// assume all elements are the same size
// or, maintain a "largest element" and compare to it

using System.Numerics;
using digipet;
using digipet.component;
using digipet.util;
using digipet.view;

using static digipet.util.DigiMath;

namespace digipet.view.menu.grid;

#nullable enable

using Kv = Tuple<ViewComponent, Action?>;
public class GridMenu : ViewComponent {
  private readonly IList<Kv> items = [];
  private static readonly Vector2 MIN_MARGIN_PX = new(2.0f, 2.0f);
  private static readonly Vector2 HALF = new(0.5f, 0.5f);

  private readonly Lerper selector_x = new(25.0);
  private readonly Lerper selector_y = new(25.0);

  private readonly Lerper selector_offset = new(12.5);

  private readonly ViewComponent container;

  // one more lerper for menu offset

  private int cursor_;
  public int Cursor {
    get => cursor_;
    set => cursor_ = Math.Clamp(value, 0, items.Count - 1);
  }

  private int margin_ = 4;
  public int MarginY {
    get => margin_;
    set {
      margin_ = value;
      QueueReflow();
    }
  }

  public GridMenu() {
    container = new();
    AddView(container);
  }

  public void AddView(ViewComponent component, Action? callback) {
    items.Add(new(component, callback));
    container.AddView(component);
    QueueReflow();
  }

  public void Clear() {
    items.Clear();
    Cursor = 0;
  }

  // how do we want to handle sliders up top?
  // (not gonna worry about it atm)

  public void CursorLeft() {
    int row_size = GetRowSize();
    Cursor = ((Cursor / row_size) * row_size) + PositiveModulo(Cursor - 1, row_size);
  }

  public void CursorRight() {
    int row_size = GetRowSize();
    Cursor = ((Cursor / row_size) * row_size) + PositiveModulo(Cursor + 1, row_size);
  }

  public void CursorUp() {
    int row_size = GetRowSize();
    Cursor -= row_size;
  }

  public void CursorDown() {
    int row_size = GetRowSize();
    Cursor += row_size;
  }

  public void Select() {
    Kv item = items[Cursor];
    item.Item2?.Invoke();
  }

  public override void Tick(double delta) {
    base.Tick(delta);

    Vector2 target = GetSelectorRectOffset(Cursor, 2);

    Tuple<float, float> scroll_range = GetTargetRange();
    selector_offset.Target = Math.Clamp(selector_offset.Target, scroll_range.Item1, scroll_range.Item2);
    selector_offset.Tick(delta);
    container.PixelY = -(float)selector_offset.Cursor;

    selector_x.Target = target.X;
    selector_y.Target = target.Y;

    selector_x.Tick(delta);
    selector_y.Tick(delta);

  }

  public Tuple<float, float> GetTargetRange() {
    if (items.Count <= 0) {
      return new(0.0f, 0.0f);
    }

    ViewComponent target = items[Cursor].Item1;

    Vector2 elem_size = GetElementSize();

    float height = PixelSizeY;
    // to bottom of view
    float min_offset = Math.Max(target.PixelY - height + elem_size.Y + MarginY, 0.0f);
    // cap is min offset for last element
    // to top of view
    float max_offset = Math.Min(target.PixelY - MarginY, items[items.Count - 1].Item1.PixelY - height + elem_size.Y);

    if (min_offset > max_offset) {
      return new(max_offset, min_offset);
    } else {
      return new(min_offset, max_offset);
    }
  }

  private Vector2 GetSelectorRectOffset(int item, int margin_px) {
    if (item < 0 || item >= items.Count) {
      return Vector2.Zero;
    }

    return items[item].Item1.OffsetPx - new Vector2(margin_px);
  }

  private Vector2 GetSelectorRectSize(int margin_px) {
    return items[Cursor].Item1.SizePx + 2.0f * new Vector2(margin_px);
  }

  private Vector2 GetElementSize() {
    Vector2 elem_size = Vector2.Zero;
    foreach (Kv entry in items) {
      elem_size = Vector2.Max(elem_size, entry.Item1.SizePx + (2.0f * MIN_MARGIN_PX));
    }

    return elem_size;
  }

  public int GetRowSize() {
    Vector2 elem_size = GetElementSize();
    Vector2 dims = SizePx;

    float row_size = MathF.Floor(dims.X / elem_size.X);
    int elements_per_row = (int)row_size;

    return elements_per_row;
  }

  public int GetElementCount() {
    return items.Count;
  }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);
    Vector2 dims = canvas.GetSizePx();
    Vector2 elem_size = GetElementSize();

    float row_size = MathF.Floor(dims.X / elem_size.X);

    int elements_per_row = (int)row_size;
    float net_width = elem_size.X * elements_per_row;
    float margin_width = (dims.X - net_width) / (elements_per_row + 1);

    for (int i = 0; i < items.Count; i++) {
      ViewComponent vc = items[i].Item1;
      int row = i / elements_per_row;
      int col = i % elements_per_row;

      float offset_x = ((col + 1) * margin_width) + (col * elem_size.X);
      float offset_y = ((row + 1) * MarginY) + (row * elem_size.Y);
      vc.OffsetPx = new Vector2(offset_x, offset_y);
    }
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);
    Vector2 rect_offset = canvas.PxToRelative(
      (float)selector_x.Cursor, 
      (float)(selector_y.Cursor - selector_offset.Cursor)
    );
    Vector2 rect_size = canvas.PxToRelative(GetSelectorRectSize(2));

    Vector2 px_size = canvas.GetPixelDims();

    canvas.Rect(rect_offset, rect_offset + rect_size, 0, DigiColor.BLACK);
    canvas.Rect(rect_offset + px_size, (rect_offset + rect_size) - px_size, 0, DigiColor.WHITE);
  }
}