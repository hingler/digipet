using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.input;
using digipet.util;
using digipet.view.gpr;
using digipet.view.rpg;
using digipet.world.character;
using digipet.world.scroll;

namespace digipet.scenes.demo;

public class SideScrollerDemo : Scene {
  private readonly SideScrollerWorld world;
  private readonly ILogger logger;
  private readonly SimpleCharController controller;
  private readonly SimpleCharFollower follower;
  private readonly RPGDelegateView d;

  public SideScrollerDemo(
    IEngine engine
  ) : base(engine) {
    world = new(engine);
    ISprite player = engine.GetSpriteFetcher().GetSprite(sprite.attrib.SpriteID.STAT_FOOD);
    logger = this.GetLogger();
    controller = new(world, player);
    follower = new(world, controller.GetCharObject(), player);
    d = new(engine);
  }

  public override void InitScene() {
    WorldEntityWrap w = new(world);
    world.Gravity = new(0.0f, -40.0f);
    RPGGrid grid = new();

    d.AddDisplay(w); 
    d.AddDisplay(grid);

    PushToStack(d);

    d.WorldScale = 0.08f;
    controller.WorldDims = controller.Sprite.Dims * d.WorldScale;
    follower.WorldDims = controller.Sprite.Dims * d.WorldScale;

    // swag
    follower.SafeDist = 0.1f;
    follower.PaddingFactor = 0.2f;
    follower.MaxVelocity = 4.8f;

    d.SizeY = 0.5f;
    d.Y = 0.25f;
  }

  public override void Tick(double delta) {
    d.WorldOrigin = new(controller.Position.X, 0.0f);
  }

  public override void PhysicsTick(double delta) {
    world.Update(delta);
    IInputManager input = Engine.GetInputManager();

    float dir = 0;
    if (input.IsPressed(InputType.LEFT)) {
      dir -= 1;
    }

    if (input.IsPressed(InputType.RIGHT)) {
      dir += 1;
    }

    controller.Velocity = dir * 4.8f;
    if (input.IsPressed(InputType.CONFIRM)) {
      controller.TryJump(10.0f);
    }

    follower.PhysTick();
  }
}