using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.util;
using digipet.view.text;

namespace digipet.view.menu.carousel;

public class CarouselMenu : ViewComponent {
  private readonly Text title;
  private readonly FlowText caption;
  private readonly IList<SpriteView> sprites;

  // 3 is always center
  // 4 -> 3 when spinning left
  // 2 -> 3 when spinning right
  private readonly IList<ICarouselMenuItem> items;
  public double Target {
    get => lerper.Target;
    set {
      lerper.Target = value;
      UpdateCaptions();
      
    }
  }

  private readonly Lerper lerper;

  public double Phi = Math.PI / 8.0;

  // assume 0.5, 0.5 is center
  public double Radius = 0.35;

  private static readonly double DELTA_RADIANS = Math.PI / 4.5;
  private static readonly int SPRITE_COUNT = 7;

  public CarouselMenu(IEngine engine) {
    title = new() {
      Offset = new(0.5f, 0.75f),
      Alignment = HorizontalAlign.CENTER,
      Font = FontType.SMALL,
      Color = DigiColor.BLACK
    };

    caption = new(engine) {
      Alignment = HorizontalAlign.CENTER,
      Offset = new(0.5f, 0.75f),
      Anchor = new(0.5f, 0.0f),
      Size = new(0.8f, 0.2f),
      Font = FontType.TINY
    };

    AddView(title);
    AddView(caption);

    sprites = [];
    for (int i = 0; i < SPRITE_COUNT; i++) {
      SpriteView v = new() {
        Anchor = new(0.5f, 1.0f),
        ZIndex = SPRITE_COUNT - i
      };

      sprites.Add(v);
      AddView(v);
    }

    items = [];

    lerper = new(12.5) {
      Target = 0
    };
  }

  public void AddItem(ICarouselMenuItem item) {
    items.Add(item);
    UpdateCaptions();
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    lerper.Tick(delta);
    List<Tuple<SpriteView, Vector3>> positions = [];
    UpdateSprites();

    for (int i = 0; i < sprites.Count; i++) {
      positions.Add(new(sprites[i], GetPos(i)));
    }

    positions.Sort((a, b) => a.Item2.Z.CompareTo(b.Item2.Z));

    for (int i = 0; i < positions.Count; i++) {
      SpriteView sprite = positions[i].Item1;
      Vector3 pos = positions[i].Item2;

      // largest z val last
      sprite.Offset = new Vector2(pos.X, pos.Y);
      sprite.ZIndex = i;
      sprite.Opacity = (float)Math.Clamp((pos.Z * 1.5) / Radius, 0.0, 1.0);
    }
  }

  private void UpdateSprites() {
    int arr_offset = (int)Math.Floor(lerper.Cursor);
    int sprite_init = arr_offset - (SPRITE_COUNT / 2);

    int net = items.Count;

    for (int c = sprite_init, i = sprites.Count - 1; i >= 0; c++, i--) {
      // positive
      int ind = ((c % net) + net) % net;
      // come up with a way to fix this?
      sprites[i].Sprite = items[ind].Sprite;
      sprites[i].SizePx = new(48, 48);
    }
  }

  private void UpdateCaptions() {
    int item_count = items.Count;
    int highlighted_index = (((int)lerper.Target % item_count) + item_count) % item_count;

    title.Content = items[highlighted_index].Name;
    caption.Content = items[highlighted_index].Description;
  }

  private Vector3 GetPos(int i) {
    double theta = GetSpriteTheta(i);
    
    double x = Math.Sin(theta) * Radius;
    double y = Math.Sin(Phi) * Radius * Math.Cos(theta);
    double z = Math.Cos(theta) * Radius;

    return new Vector3(
      (float)x + 0.5f, (float)y + 0.5f, (float)z
    );
  }

  private double GetSpriteTheta(int i) {
    int offset = i - (SPRITE_COUNT / 2);

    double cursor_abs = ((lerper.Cursor % 1.0) + 1.0) % 1.0;
    return (offset + cursor_abs) * DELTA_RADIANS;
  }
}