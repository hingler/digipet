using System.Numerics;
using System.Reflection;

namespace digipet.world.simple;
using CorrectionResult = Tuple<Vector2, Vector2>;
public class WallBoundaryHandler : IBoundaryHandler {

  public Vector2 WorldOrigin = Vector2.Zero;

  private Vector2 normal_ = Vector2.UnitY;
  public Vector2 WorldNormal {
    get => normal_;
    set => normal_ = Vector2.Normalize(value);
  }

  public Vector2 WorldTangent {
    get => new(WorldNormal.Y, -WorldNormal.X);
  }

  // m64 - dist into wall to correct
  public float CorrectionDist = 0.5f;
  // min speed to handle as bounce
  public float BounceThreshold = 0.15f;

  public CollisionData HandleBoundaries(
    IPhysObject ob,
    float bounce,
    float damping,
    float friction_coeff,
    double delta
  ) {

    // find object's world dims
    // apply to correction factors

    Vector2 position_local = ob.Position - WorldOrigin;

    float velocity_normal = Vector2.Dot(ob.Velocity, WorldNormal);
    float collide_dist = Vector2.Dot(position_local, WorldNormal);

    if ((collide_dist > 0.0f) || (collide_dist < -CorrectionDist)) {
      // above wall, or beyond correction dist
      return new();
    }

    // we're somewhere in the correction zone
    bool has_rebound = velocity_normal < -BounceThreshold;
    bool has_slide = velocity_normal < 0.0f && !has_rebound;

    float rebound_f = (has_rebound ? 1f : 0f);
    float slide_f = (has_slide ? 1f : 0f);

    float pos_correction_fac = -collide_dist;

    Vector2 pos_correction = (pos_correction_fac + (pos_correction_fac * rebound_f * bounce)) * WorldNormal;
    Vector2 vel_correction = Vector2.Zero;
    if (velocity_normal < 0.0f) {
      // push velocity away from surface
      Vector2 vel_component = WorldNormal * velocity_normal;
      Vector2 vel_tangent = ob.Velocity - vel_component;

      vel_correction -= vel_component * (1.0f + rebound_f * bounce);
      vel_correction += vel_tangent * ((float)Math.Exp(delta * -friction_coeff) - 1.0f) * slide_f;
      // normal force?
    }

    CollisionData res = new() {
      DeltaPos = pos_correction,
      Impulse = vel_correction,
      Normal = WorldNormal,
      Point = ob.Position + pos_correction
    };

    return res;
  }
}