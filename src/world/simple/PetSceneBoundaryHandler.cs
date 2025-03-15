// using System.Numerics;

// namespace digipet.world.simple;

// using CorrectionResult = Tuple<Vector2, Vector2>;

// public class PetSceneBoundaryHandler : IBoundaryHandler {
//   private static readonly Vector2 LOW_BOUND = new(-0.5f, 0.0f);
//   private static readonly Vector2 HI_BOUND = new(0.5f, 50.0f);
//   public Vector2 Gravity = new(0.0f, 0.0f);

//   // generify into (origin, direction), like m64
//   public CorrectionResult HandleBoundaries(
//     IPhysObject ob, 
//     float bounce,
//     float damping,
//     float friction_coeff,
//     double delta
//   ) {
//     // boundary handler is here
//     float px_width = ob.Sprite.Dims.X / 192.0f;
//     float max_x = HI_BOUND.X - (px_width / 2);
    
//     Vector2 low_bound = new(-max_x, LOW_BOUND.Y);
//     Vector2 hi_bound = new(max_x, HI_BOUND.Y);

//     // stores the raw dist we're oob by
//     Vector2 drift_factor = Vector2.Clamp(ob.Position, low_bound, hi_bound) - ob.Position;
//     // for each bounce:
//     // correct for position drift * bounce
//     Vector2 drift_factor_abs = Vector2.Abs(drift_factor);

//     // true if we're past a wall, else false
//     Vector2 has_collide = new(
//       drift_factor_abs.X > 0.0 ? 1 : 0,
//       drift_factor_abs.Y > 0.0 ? 1 : 0
//     );

//     // true if we want to bounce off the wall, else false
//     Vector2 has_rebound = has_collide * new Vector2(
//       MathF.Abs(ob.Velocity.X) > 0.01f ? 1 : 0,
//       MathF.Abs(ob.Velocity.Y) > 0.25f ? 1 : 0
//     );

//     // sliding if colliding and not rebounding
//     Vector2 has_slide = has_collide * (Vector2.One - has_rebound);
//     Vector2 pos_correction = Vector2.Zero;
//     Vector2 vel_correction = Vector2.Zero;
//     if (has_collide.X > 0 || has_collide.Y > 0) {
//       pos_correction = drift_factor + (drift_factor * has_rebound * bounce);
//       Vector2 velocity_correction = ob.Velocity * has_collide + (ob.Velocity * has_rebound) * bounce;
//       vel_correction -= velocity_correction;
//     }

//     Vector2 slide_delta = ob.Velocity * ((float)Math.Exp(delta * -friction_coeff) - 1.0f) * has_slide;

//     vel_correction += slide_delta;

//     return new(pos_correction, vel_correction);
//   }
// }

// dead code :3