using System.Numerics;
using digipet.framework;
using digipet.image;
using digipet.world;

namespace digipet.sim.toy.handlers;

public class ToyCar : BaseToyHandler {
  private readonly IEngine engine;
  private readonly IPhysObject car;
  public ToyCar(
    IEngine engine
  ) {
    this.engine = engine;
    ISprite car_sprite = engine.GetDigipetAssetLoader().LoadSprite("sprites/toy/car.png");
    IWorldItem base_item = new BaseToyActivator(this, new EmptyWorldItem(car_sprite));
    car = engine.GetPhysWorld().SpawnObject(base_item, Vector2.Zero, Vector2.Zero, 0.85f, 0.2f, 1.0f);
  }

  public override void Tick(double delta) {
    if (Active && car.Velocity.LengthSquared() < 0.001f) {
      Active = false;
    }

    car.FlipX = car.Velocity.X < 0.0f;
  }

  protected override void Activate() {
    car.ApplyImpulse(Vector2.UnitX * 2.5f);
  }

  protected override void Deactivate() {
    car.Halt();
  }

  public override void Destroy() {
    engine.GetPhysWorld().RemovePhysObject(car);
  }

}