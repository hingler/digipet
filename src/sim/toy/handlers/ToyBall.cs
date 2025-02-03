using System.Numerics;
using digipet.framework;
using digipet.image;
using digipet.world;

namespace digipet.sim.toy.handlers;

public class ToyBall : BaseToyHandler {
  private readonly IPhysObject ball;

  private readonly IEngine engine;
  private readonly Random random = new();

  public ToyBall(
    IEngine engine,
    IToy toy_base
  ) {
    this.engine = engine;
    ISprite ball_sprite = engine.GetDigipetAssetLoader().LoadSprite("sprites/toy/ball.png");
    IWorldItem base_item = new BaseToyActivator(this, toy_base) {
      SpriteOverride = ball_sprite
    };

    ball = engine.GetPhysWorld().SpawnObject(
      base_item, Vector2.Zero, Vector2.Zero, 0.93f, 0.18f, 1.0f
    );

  }
  public override void Tick(double delta) {
    if (Active && ball.Velocity.LengthSquared() < 0.0001f) {
      Active = false;
    }
  }

  protected override void Activate() {
    float rand_theta = random.NextSingle() * MathF.PI;

    Vector2 rand_dir = new(MathF.Cos(rand_theta), MathF.Sin(rand_theta));
    ball.ApplyImpulse(rand_dir * 1.9f);
  }

  protected override void Deactivate() {
    // no op , just activate the ball again
  }

  public override void Destroy() {
    engine.GetPhysWorld().RemovePhysObject(ball);
  }
}
