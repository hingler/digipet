using System.Numerics;
using digipet.framework;
using digipet.image;
using digipet.world;

namespace digipet.sim.toy.handlers;

public class ToyDrone : BaseToyHandler {
  private readonly IPhysObject drone_control;

  private readonly DroneController drone_controller;
  private readonly IAnimatedSprite drone_sprite;

  private readonly IEngine engine;

  private double frame_timer;

  public ToyDrone(
    IEngine engine,
    IToy toy_base
  ) {
    this.engine = engine;
    ISprite controller_sprite = engine.GetDigipetAssetLoader().LoadSprite("sprites/toy/drone_controller.png");
    drone_sprite = engine.GetDigipetAssetLoader().LoadAnimatedSprite("sprites/toy/drone_sprites.png");
    drone_sprite.HFrames = 3;
    drone_sprite.VFrames = 1;
    drone_sprite.Frame = 0;

    IWorldItem controller = new BaseToyActivator(this, toy_base) {
      SpriteOverride = controller_sprite
    };

    IWorldItem drone = new EmptyWorldItem(drone_sprite);

    IPhysWorld world = engine.GetPhysWorld();
    drone_control = world.SpawnObject(controller, Vector2.UnitX * -0.25f, Vector2.Zero);
    IPhysObject drone_body = world.SpawnObject(drone, Vector2.UnitX * 0.25f, Vector2.Zero);

    drone_controller = new(world, drone_body);
  }

  public override void Tick(double delta) {
    drone_controller.Tick(delta);
    frame_timer += (delta * drone_controller.Thrust);

    if (drone_controller.Thrust > 0.1f) {
      drone_sprite.Frame = 1 + (int)((frame_timer * 20) % 2);
    } else {
      drone_sprite.Frame = 0;
    }

  }

  protected override void Activate() {
    drone_controller.Active = true;
  }

  protected override void Deactivate() {
    drone_controller.Active = false;
  }

  public override void Destroy() {
    engine.GetPhysWorld().RemovePhysObject(drone_control);
    drone_controller.Destroy();
  }
}