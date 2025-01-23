using System;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.sim;
using digipet.sprite;
using digipet.sprite.attrib;
using digipet.view;
using digipet.view.pet;
using digipet.view.sky;

namespace digipet.demo;

public class AnimatedTextScene : Scene {

  private readonly Text t;
  private readonly DigiProgressBar bar;

  private readonly ISpriteFetcher fetcher;
  private readonly IPetModel model;

  private double dt;

  public AnimatedTextScene(
    IEngine engine,
    ISpriteFetcher fetcher,
    IPetModel model
  ) : base(engine) {
    this.fetcher = fetcher;
    this.model = model;

    dt = 0.0;
    t = new() {
      Content = "test_text",
      Scale = 2.0f,
      Alignment = HorizontalAlign.CENTER,
      Offset = new(0.5f, 0.5f)
    };

    bar = new() {
      Size = new(1.0f, 0.08f),
      Offset = new(0.0f, 0.7f)
    };
  }
  public override void InitScene() {
    SkyView v = new(fetcher);
    v.Offset = System.Numerics.Vector2.Zero;
    v.Size = System.Numerics.Vector2.One;

    PushToStack(v);

    PushToStack(t);


    // shouldnt be here
    CompoundView sv = new();

    sv.Offset = new(0.0f, 0.2f);
    sv.Size = new(0.5f, 0.5f);

    sv.AddView(bar);
    PushToStack(sv);
    PushToStack(bar);
    SimplePetSprite anime = new(Engine);

    // no longer square :/
    anime.Size = new(1.0f, 1.0f);
    anime.Offset = new(0.0f, 0.0f);
    anime.Anchor = new(0.0f, 0.0f);
    PushToStack(anime);

    PetStatList l = new(Engine, model);
    l.Offset = new(0.0f, 0.0f);
    l.Size = new(0.38f, 1.0f);

    PushToStack(l);
  }

  public override void Tick(double delta) {
    dt += delta;

    // offset for this text is screwy :/
    t.Offset = new System.Numerics.Vector2(
      (float)(Math.Sin(dt) * 0.4 + 0.5),
      0.1f
    );
    bar.Fill = (float)Math.Cos(dt) * 0.5f + 0.5f;

    
  }
}