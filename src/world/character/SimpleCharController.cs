// wrap around phys world obvject
// based on key input, attempt to move
// add jump (impulse in V direction)

// need to "port" animation handler from pet i think??
// (do it later lolol)

// - project grid over


// tbas for backgrounds
// - add parallax scrolling (ie layers w different offset intensities)
// - add interactors (more dialogue handlers?? not yet - don't overdevelop)


// - figure out how to handle autoscroll?
// - figure out how to implement camera follow?

// - once those are done im not *too* worried

using System.Numerics;
using digipet.image;
using digipet.sim;

namespace digipet.world.character;

// how do we prevent this controller from sliding on sloped surfaces??
// - ground -> ignore gravity
// - determine if grounded based on local slopes
// - if grounded, then treat as "static"
// - if not grounded, then subject to physics
// - (what about rolling off a slope? i think we could toggle grounding based on state)

public class SimpleCharController {
  private readonly IPhysWorld world;
  private readonly IPhysObject target;

  private double max_slope_rads = Math.PI / 4;

  public double MaxSlopeDegrees {
    get => max_slope_rads * (180.0 / Math.PI);
    set => max_slope_rads = value * (Math.PI / 180.0);
  }

  public Vector2 Position => target.Position;

  public float Velocity {
    get => target.Velocity.X;
    set => target.SetVelocity(new(value, target.Velocity.Y));
  }

  public Vector2 WorldDims {
    get => target.WorldDims;
    set => target.SetWorldDims(value);
  }

  public ISprite Sprite => target.Sprite;

  public SimpleCharController(
    IPhysWorld phys_world,
    ISprite sprite
  ) {
    world = phys_world;
    target = world.SpawnObject(
      new EmptyWorldItem(sprite),
      Vector2.UnitY * 0.4f,
      Vector2.Zero,
      0.05f,
      0.1f,
      5.0f
    );

    WorldDims = target.Sprite.Dims * 0.02f;
  }

  public void TryJump(float delta_v) {
    if (IsGrounded()) {
      target.ApplyImpulse(Vector2.UnitY * delta_v);
    }
  }

  public IPhysObject GetCharObject() => target;

  public bool IsGrounded() {
    IReadOnlyList<CollisionData> collision_points = world.GetCollisions(target);

    double max_slope_cos = Math.Cos(max_slope_rads);
    foreach (CollisionData datum in collision_points) {
      if (datum.Normal.Y >= max_slope_cos) {
        return true;
      }
    }

    return false;
  }
}