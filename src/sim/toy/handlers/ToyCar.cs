using System.Numerics;
using digipet.framework;
using digipet.image;
using digipet.world;

namespace digipet.sim.toy.handlers;

public class ToyCar : BaseToyHandler {
  private readonly IEngine engine;
  private readonly IPhysObject car;
  private readonly Random random;

  // poll for activator

  public ToyCar(
    IEngine engine,
    IToy toy_item
  ) {
    this.engine = engine;
    ISprite car_sprite = engine.GetDigipetAssetLoader().LoadSprite("sprites/toy/car.png");
    BaseToyActivator base_item = new(this, toy_item) {
      SpriteOverride = car_sprite
    };
    
    car = engine.GetPhysWorld().SpawnObject(base_item, Vector2.Zero, Vector2.Zero, 0.85f, 0.2f, 1.0f);

    random = new();
  }

  public override void Tick(double delta) {
    if (Active && car.Velocity.LengthSquared() < 0.001f) {
      Active = false;
    }

    car.FlipX = car.Velocity.X < 0.0f;
  }

  protected override void Activate() {
    car.ApplyImpulse(Vector2.UnitX * (1.95f + random.NextSingle() * 1.1f));
  }

  protected override void Deactivate() {
    car.Halt();
  }

  public override void Destroy() {
    engine.GetPhysWorld().RemovePhysObject(car);
  }

  // todo:
  // - create toy manager
  // - create toy enjoyment model
  //   - feed into "pet fun"
  //   - feed into "activate task"
  // when a toy is on map
  // - if deactivated, sample an "enjoyment value" for it
  // - if activated, calculate enjoyment value and flip to get "staleness"
  // - based on priorities: deactivate a stale toy, or activate a fun toy
  // - toys should have some "fun decay" rate from type:type
  //   - typ enjoyment should be inversely proportional to decay
  // - manager can update enjoyment/staleness - just check map and update what's out
  //   - over time toys should become "fun" again

  // toy ideas
  // - jukebox (play some music)
  // - ball (launch against walls)
  // - drone (activate via controller - flies around on its own)
  
  // thinking: impl these, then move on

}