using System.Numerics;
using digipet.framework;
using digipet.image;
using digipet.util;
using digipet.view.rpg;

namespace digipet.view.world;

public class ParallaxBGBuilder {
  // return IRPGDisplay containing our parallax views

  public List<IPseudoDepthDisplay> displays;

  public ParallaxBGBuilder() {
    displays = [];
  }

  public ParallaxBGBuilder AddDynamic(
    ISprite sprite,
    Vector2 offset,
    float scale,
    float z_dist,
    bool tile
  ) {
    ParallaxBGView view = new() {
      Sprite = sprite,
      BGOffset = offset,
      SpriteScale = scale,
      ZDist = z_dist,
      Tile = tile,
      Offset = Vector2.Zero
    };

    displays.Add(view);

    view.Size = Vector2.One;
    return this;
  }

  public ParallaxBGBuilder AddStatic(
    ISprite sprite,
    Vector2 bg_offset,
    float scale,
    float z_dist,
    bool tile
  ) {
    StaticBGView view = new() {
      Sprite = sprite,
      BGOffset = bg_offset,
      SpriteScale = scale,
      ZDist = z_dist,
      Tile = tile,
      Offset = Vector2.Zero
    };

    displays.Add(view);
    view.Size = Vector2.One;

    return this;
  }

  public ParallaxBGBuilder AddSolid(
    DigiColor color,
    float z_dist
  ) {
    SolidColorBG col = new() {
      Color = color,
      ZDist = z_dist
    };

    displays.Add(col);
    col.Size = Vector2.One;

    return this;
  }

  public RPGDelegateView Build(IEngine engine) {
    RPGDelegateView view = new(engine);

    foreach (IPseudoDepthDisplay display in displays) {
      view.AddDisplay(display);
    }

    return view;
  }
}