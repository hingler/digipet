using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using digipet.component;
using digipet.image;
using digipet.sim;
using digipet.sprite.attrib;
using digipet.view.container;

namespace digipet.view.pet;

public class PetStatList : ViewComponent {
  // margin container
  // each "stat" has height of 1/5 container and width of 1

  private readonly IPetModel model;

  private readonly IList<PetStat> stats = [];

  private readonly MarginContainer container = new();

  public PetStatList(
    ISpriteFetcher sprite_fetcher,
    IPetModel model
  ) {
    this.model = model;
    container.MarginPx = 6;

    stats.Add(CreateStats(sprite_fetcher, SpriteID.STAT_FOOD));
    stats.Add(CreateStats(sprite_fetcher, SpriteID.STAT_WATER));
    stats.Add(CreateStats(sprite_fetcher, SpriteID.STAT_FUN));
    stats.Add(CreateStats(sprite_fetcher, SpriteID.STAT_SOCIAL));
    stats.Add(CreateStats(sprite_fetcher, SpriteID.STAT_ENERGY));

    for (int i = 0; i < stats.Count; i++) {
      stats[i].Offset = new(0.0f, i * 0.2f);
      container.AddView(stats[i]);
    }
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    stats[0].Fill = (float)model.Food;
    stats[1].Fill = (float)model.Water;
    stats[2].Fill = (float)model.Fun;
    stats[3].Fill = (float)model.Social;
    stats[4].Fill = (float)model.Energy;
  }

  public override void Draw(ICanvas canvas) {
    base.Draw(canvas);

    // draw border

    Vector2 pixel_size = canvas.GetPixelDims();

    Vector4 color_gray = new(0.5f, 0.5f, 0.5f, 1.0f);

    canvas.Rect(Vector2.Zero, Vector2.One, 4, color_gray);

    // generify this for other windows

    // (tba: alpha dither?)
    canvas.Rect(pixel_size, Vector2.One - pixel_size, 3, new(0, 0, 0, 1));
    canvas.Rect(2.0f * pixel_size, Vector2.One - 2.0f * pixel_size, 2, color_gray);
    canvas.Rect(3.0f * pixel_size, Vector2.One - 3.0f * pixel_size, 1, Vector4.One);

    container.Draw(canvas);
  }

  private static PetStat CreateStats(ISpriteFetcher fetcher, SpriteID id) {
    ISprite sprite = fetcher.GetSprite(id);

    PetStat stat = new(sprite) {
      Size = new Vector2(1.0f, 0.2f),
      Anchor = Vector2.Zero
    };

    return stat;
  }
}