using System;
using digidev.impl;
using digipet.component;
using digipet.framework;
using digipet.sim;
using digipet.sprite;
using digipet.sprite.attrib;
using digipet.view;
using digipet.view.pet;
using digipet.view.sky;
using Godot;

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

    CompoundView sv = new();
    Texture2D img = ResourceLoader.Load<Texture2D>("res://res/img/fh_test.jpg");
    Texture2D sa_sprite = ResourceLoader.Load<Texture2D>("res://res/img/pet/pixilart-sprite20px.png");

    GodotSprite sprite = new(img);
    AnimatedGodotSprite sa = new(sa_sprite);

    sa.HFrames = 2;
    sa.VFrames = 1;
    
    sa.Frame = 0;

    sv.Offset = new(0.0f, 0.2f);
    sv.Size = new(0.5f, 0.5f);

    sv.AddView(bar);
    PushToStack(sv);
    PushToStack(bar);

    SpriteView sprite_view = new(sprite);
    SimplePetSprite anime = new(sa);

    sprite_view.Size = new(0.6f, 0.15f);
    sprite_view.Offset = new(0.1f, 0.4f);

    // no longer square :/
    anime.Size = new(1.0f, 1.0f);
    anime.Offset = new(0.0f, 0.0f);
    anime.Anchor = new(0.0f, 0.0f);

    PushToStack(sprite_view);
    PushToStack(anime);


    PetStat ps = new(new GodotSprite("res://res/img/stat/meat.png")) {
      Fill = 0.6f,
      Offset = new(0.2f, 0.2f),
      Size = new(0.6f, 0.25f),
      Anchor = new(0.0f, 0.5f)
    };

    PushToStack(ps);

    PetStatList l = new(fetcher, model);
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