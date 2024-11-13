using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using digipet.component;
using digipet.image;
using digipet.input;
using digipet.sim;
using digipet.sprite.attrib;
using digipet.view.container;

namespace digipet.view.pet;

public class PetStatList : ViewComponent {
  // margin container
  // each "stat" has height of 1/5 container and width of 1

  private readonly IPetModel model;

  private readonly IList<PetStat> stats = [];

  private readonly BorderContainer container = new();

  public PetStatList(
    ISpriteFetcher sprite_fetcher,
    IPetModel model
  ) {
    this.model = model;

    stats.Add(CreateStats(sprite_fetcher, SpriteID.STAT_FOOD));
    stats.Add(CreateStats(sprite_fetcher, SpriteID.STAT_WATER));
    stats.Add(CreateStats(sprite_fetcher, SpriteID.STAT_FUN));
    stats.Add(CreateStats(sprite_fetcher, SpriteID.STAT_SOCIAL));
    stats.Add(CreateStats(sprite_fetcher, SpriteID.STAT_ENERGY));

    MarginContainer container_margin = new() {
      MarginPx = 3
    };

    for (int i = 0; i < stats.Count; i++) {
      stats[i].Offset = new(0.0f, i * 0.2f);
      container_margin.AddView(stats[i]);
    }

    container.AddView(container_margin);
    AddView(container);
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    stats[0].Fill = (float)model.Food;
    stats[1].Fill = (float)model.Water;
    stats[2].Fill = (float)model.Fun;
    stats[3].Fill = (float)model.Social;
    stats[4].Fill = (float)model.Energy;
  }

  public override bool HandleInput(InputType input, InputState state) {
    if (base.HandleInput(input, state)) {
      return true;
    }

    if (state == InputState.PRESS && input == InputType.BACK) {
      PopSelf();
    }

    return true;
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