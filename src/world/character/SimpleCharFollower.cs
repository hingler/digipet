using System.Numerics;
using digipet.image;
using digipet.transition.animator;

namespace digipet.world.character;

public class SimpleCharFollower {
  private readonly SimpleCharController subcontroller;
  private readonly IPhysObject target;

  public float MaxVelocity = 0.8f;
  public float PaddingFactor = 2.0f;
  public float SafeDist = 0.05f;

  public Vector2 WorldDims {
    get => subcontroller.WorldDims;
    set => subcontroller.WorldDims = value;
  }

  public SimpleCharFollower(
    IPhysWorld world,
    IPhysObject target,
    ISprite sprite
  ) {
    subcontroller = new(world, sprite);
    this.target = target;
  }

  public void PhysTick() {
    float pos_delta = target.Position.X - subcontroller.Position.X;

    float pos_abs = Math.Abs(pos_delta);
    float pos_sign  = Math.Sign(pos_delta);

    // smoothstepped T value
    double speed_t = EasingFunctions.SmoothStep(
      0.0, 
      MaxVelocity * PaddingFactor, 
      pos_abs - SafeDist
    );

    float speed = (float)(speed_t * MaxVelocity);

    subcontroller.Velocity = speed * pos_sign;
  }
}