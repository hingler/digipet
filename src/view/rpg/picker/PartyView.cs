using System.Runtime.CompilerServices;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.rpg.chardata;
using digipet.util;
using digipet.view;

namespace digipet.rpg.picker;

#nullable enable

// this will be it
public class PartyView : ViewComponent {
  private readonly ISprite stage_sprite;
  private readonly List<ICharData?> active_chars;

  private readonly List<SpriteView> stage_views;
  private readonly List<SpriteView> char_views;

  private readonly SpriteView menu_sprite;
  private readonly SpriteView selected_sprite;
  private readonly Lerper index_lerper;
  
  private static readonly int CHAR_COUNT = 5;

  public int Target {
    get => (int)index_lerper.Target;
    set => index_lerper.Target = Math.Clamp(value, 0, CHAR_COUNT - 1);
  }

  // lame but w/e lole
  // introduce equivalent functionality for picker
  public bool Active {
    get => menu_sprite.Opacity > 0.5f;
    set => menu_sprite.Opacity = value ? 1 : 0;
  }

  public PartyView(
    IEngine engine
  ) : this(
    engine,
    []
  ) {}

  public PartyView(
    IEngine engine,
    List<ICharData?> init_chars
  ) {
    active_chars = [];
    for (int i = 0; i < CHAR_COUNT; i++) {
      if (i >= init_chars.Count) {
        active_chars.Add(null);
      } else {
        active_chars.Add(init_chars[i]);
      }
    }
    
    stage_sprite = engine.GetDigipetAssetLoader().LoadSprite("sprites/rpg/charselect/charstage.png");
    stage_views = [];
    char_views = [];

    // how do we want to do swaps here??
    // - separate pointer from view (fine w that tbh)

    for (int i = 0; i < CHAR_COUNT; i++) {
      SpriteView stage = new SpriteView() {
        Anchor = new(0.0f, 1.0f),
        Sprite = stage_sprite
      };

      AddView(stage);
      stage_views.Add(stage);

      SpriteView sprite = new() {
        Anchor = new(0.5f, 1.0f)
      };

      AddView(sprite);
      char_views.Add(sprite);
    }

    menu_sprite = new() {
      Sprite = engine.GetDigipetAssetLoader().LoadSprite("sprites/menu/menu_selector_v.png"),
      Anchor = new(0.5f, 0.0f),
      PixelY = 16.0f
    };

    selected_sprite = new() {
      Sprite = engine.GetDigipetAssetLoader().LoadSprite("sprites/menu/menu_source_v.png"),
      Anchor = new(0.5f, 0.0f),
      PixelY = 16.0f,
      Opacity = 0.0f
    };

    AddView(menu_sprite);
    AddView(selected_sprite);

    index_lerper = new(10.0f) {
      Target = 0f
    };

    index_lerper.Reset();

    UpdateSprites();
  }



  // do it here lol
  // in action: swap control btwn picker and party

  public ICharData? Swap(ICharData new_char) => Swap(Target, new_char);
  public ICharData? Swap(int index, ICharData new_char) {
    if (index >= 0 && index < CHAR_COUNT) {
      ICharData? temp = active_chars[index];
      active_chars[index] = new_char;
      UpdateSprites();
      return temp;
    }

    return null;
  }

  public ICharData? GetSelectedChar() => active_chars[Target];

  private void UpdateSprites() {
    for (int i = 0; i < CHAR_COUNT; i++) {
      char_views[i].Sprite = active_chars[i]?.ToCharInfo().CharSprite;
    }
  }

  public override void Tick(double delta) {
    index_lerper.Tick(delta);
    menu_sprite.PixelX = GetMenuOffset();
  }

  private float GetMenuOffset() => GetMenuOffset(index_lerper.Cursor);

  private float GetMenuOffset(double index) {
    float index_f = GetStageOffset((int)Math.Floor(index));
    float index_c = GetStageOffset((int)Math.Ceiling(index));

    double t = (index - Math.Floor(index));

    return (float)(t * index_c + (1.0 - t) * index_f);
  }

  public void MarkSourceIndex() {
    int index = Target;
    selected_sprite.PixelX = GetMenuOffset(Target);
    selected_sprite.Opacity = 1.0f;
  }

  public void ResetSelections() {
    selected_sprite.Opacity = 0.0f;
  }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);

    float stage_x = stage_sprite.Dims.X;
    // do space between

    float net_stage_space = stage_x * CHAR_COUNT;
    float net_empty = canvas.GetSizePx().X - net_stage_space;

    float space_size = (net_empty / (CHAR_COUNT + 1));

    for (int i = 0; i < CHAR_COUNT; i++) {
      float spaces = (i + 1) * space_size;
      float stages = i * stage_x;

      stage_views[i].PixelX = spaces + stages;
      char_views[i].PixelX = spaces + stages + stage_x * 0.5f;

      stage_views[i].Y = 1.0f - canvas.GetPixelDims().Y;
      char_views[i].Y = 1.0f - canvas.PxToRelative(0, 8).Y;
    }
  }

  private float GetStageOffset(int index) {
    return char_views[Math.Clamp(index, 0, CHAR_COUNT - 1)].PixelX;
  }
}