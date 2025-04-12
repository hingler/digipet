using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.input;
using digipet.rpg.overworld;
using digipet.util;
using digipet.view.rpg;
using digipet.view.rpg.overworld;
using digipet.view.world;
using graphui.node;

namespace digipet.scenes.demo;

public class OverworldDemo : Scene {
  private readonly OverworldView manager;
  private readonly SpriteAnimator animator;
  private readonly ISpriteSequence sequence;
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
    
    OverworldMap map_data = new();

    map_data.world_name = "thej";
    map_data.graph_nav = builder.Build();
    map_data.backgrounds = [];

    List<string> paths = [];

    string path_prefix = "sprites/rpg/bg/watersequence/";
    for (int i = 0; i < 8; i++) {
      paths.Add(path_prefix + i + ".png");
    }

    // animated sprite sequence
    // workaround for no tiling on animated sprites

    sequence = engine.GetDigipetAssetLoader().ToSpriteSequence(paths);

    animator = new(sequence, 0.5);
    // quick sprite animator?
    ParallaxBGView v = new() {
      Sprite = sequence,
      BGOffset = Vector2.Zero,
      SpriteScale = 0.1f,
      ZDist = 2.0f,
      TileX = true,
      TileY = true,
      Offset = Vector2.Zero,
      Size = Vector2.One,
      LockX = false,
      LockY = false
    };

    v.Size = Vector2.One;

    map_data.backgrounds.Add(v);

    manager = new(map_data, engine);
    logger.Log("manager created");
    logger.Log("constructor called");
  }

  public override void InitScene() {
    logger.Log("initializing scene");

    PushToStack(manager);
  }

  public override bool HandleInput(IKeyEvent @event) {
    base.HandleInput(@event);

    Direction dir = InputToUIDirection.Convert(@event);
    if (dir != Direction.NONE) {
      logger.Log("direction received: ", dir);
      return manager.OnInput(dir);
    }

    return false;
  }

  public override void Tick(double delta) {
    // manager.Tick(delta);
    if (animator.Tick(delta)) {

    }
  }

}