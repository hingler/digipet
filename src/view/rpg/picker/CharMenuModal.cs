using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.rpg;
using digipet.rpg.chardata;
using digipet.rpg.context;
using digipet.util;
using digipet.view.container;

namespace digipet.view.rpg.picker;

public class CharMenuModal : BorderContainer {
  private readonly ICharData data;
  // don't expose - let menu handle it

  private readonly SpriteView char_view;

  private readonly List<Text> stats;
  private readonly Text name;

  public CharMenuModal(IEngine engine) : this(
    new SimpleCharData(engine)
  ) {}
  public CharMenuModal(
    ICharData data
  ) {
    this.data = data;

    char_view = new() {
      Anchor = new(0.0f, 0.5f),
      Sprite = data.SpriteOverride
    };

    AddView(char_view);

    stats = [];

    for (int i = 0; i < 6; i++) {
      Text t = new() {
        Color = DigiColor.BLACK
      };

      AddView(t);
      stats.Add(t);
    }

    name = new() {
      Color = DigiColor.BLACK.WithOpacity(0.3f),
      Font = FontType.SMALL
    };

    name.Alignment = HorizontalAlign.RIGHT;
    name.Content = this.data.Name;

    name.Offset = new(1.0f, 1.0f);

    AddView(name);
  }

  public override void Reflow(ICanvas canvas) {
    base.Reflow(canvas);

    // how do we want to do this?
    // - sprite (center, actual size)
    // text (3x lines = 33px min height)
    // expect min 32px

    float view_height = PixelSizeY;

    float sprite_center = view_height / 2;

    char_view.PixelY = sprite_center;
    char_view.PixelX = 8.0f;

    // where to place text?

    float text_start = char_view.PixelX + char_view.PixelSizeX + 16.0f;
    float text_room = PixelSizeX - text_start;

    float text_max = FontSizeHandler.GetFontSize(FontType.TINY) * 3;
    float text_offset = MathF.Round((view_height / 2) - (text_max / 2) - 1);

    float text_mid = MathF.Round(text_room / 2);
    ICharStatsBase c_stat = data.BaseStats;

    List<long> stats_list = [
      c_stat.Attack, c_stat.Defense, c_stat.Wisdom, 
      c_stat.Weight, c_stat.Vitality, c_stat.Speed
    ];

    // const these
    // ughhh im too tired to give this the care it mandates
    List<string> short_names = [
      "ATK", "DEF", "WIS", "WGT", "VIT", "SPD"
    ];

    float font_ascent = FontSizeHandler.GetFontAscent(FontType.TINY);

    for (int i = 0; i < 6; i++) {
      int font_above = Math.Max(i % 3, 0) * FontSizeHandler.GetFontSize(FontType.TINY);

      float baseline_px = text_offset + font_above + font_ascent;

      stats[i].PixelX = text_start + (text_mid * (i / 3));
      stats[i].PixelY = baseline_px;

      stats[i].Content = short_names[i] + " " + stats_list[i];

      // this is wrong but it should visualize

      // create a little test scene for it
    }
  }
}