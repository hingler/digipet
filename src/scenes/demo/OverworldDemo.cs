using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.rpg.overworld;
using digipet.util;
using digipet.view.rpg;

using graphui.node;

namespace digipet.scenes.demo;

public class OverworldDemo : Scene {
  private readonly OverworldManager manager;
  private readonly RPGDelegateView rpg_view;
  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<OverworldDemo>();

  public OverworldDemo(
    IEngine engine
  ) : base(engine) {
    logger.Log("pre-constructor");
    OverworldBuilder builder = new();
    OverworldNode a = new() {
      Position = new(-10.0f, 0.0f),
      Identifier = "1.1"
    };

    OverworldNode b = new() {
      Position = new(-5.0f, 0.0f),
      Identifier = "1.2"
    };

    OverworldNode ba = new() {
      Position = new(-5.0f, 5.0f),
      Identifier = "1.21"
    };

    OverworldNode c = new() {
      Position = new(5.0f, 0.0f),
      Identifier = "1.B"
    };

    logger.Log("pre-builder");

    builder.Connect(a, Direction.RIGHT, b, Direction.LEFT)
      .Connect(b, Direction.UP, ba, Direction.DOWN)
      .Connect(b, Direction.RIGHT, c, Direction.LEFT)
      .SetInitialNode(a);

    logger.Log("post-builder");
    manager = new(builder, Engine);
    logger.Log("manager created");
    rpg_view = new(Engine);

    logger.Log("constructor called");
  }

  public override void InitScene() {
    logger.Log("initializing scene");
    rpg_view.AddView(
      new WorldEntityWrap(manager)
    );

    PushToStack(rpg_view);

    rpg_view.WorldScale = 0.1f;
    manager.SetSpriteScale(Vector2.One * 0.1f);

    rpg_view.SizePx = new Vector2(192.0f);
  }

  public override bool HandleInput(IKeyEvent @event) {
    base.HandleInput(@event);

    Direction dir = InputToUIDirection.Convert(@event);
    if (dir != Direction.NONE) {
      return manager.ProvideInput(dir);
    }

    return false;
  }

  public override void Tick(double delta) {
    manager.Tick(delta);
    rpg_view.WorldOrigin = manager.GetSelectorPosition();
  }

}