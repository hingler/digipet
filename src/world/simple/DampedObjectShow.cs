using System.Numerics;
using digipet.framework;
using digipet.sim;
using digipet.sprite.attrib;
using digipet.util;

namespace digipet.world.simple;

public class DampedObjectShow : IPhysWorld {
  public float FloorHeight => 0.8f;

  private readonly HashSet<ObjPhysObject> objects = [];

  private readonly ILogger logger = LoggerSingleton.GetStaticLogger<DampedObjectShow>();

  private readonly IEngine engine;

  private static readonly float MAX_X = 0.45f;

  public Vector2 Gravity = new(0.0f, -2.1f);
  public float DefaultBounciness = 0.65f;
  public float DefaultLinearDamping = 0.2f;
  public float DefaultFrictionDamping = 3.0f;

  private readonly Random random = new();

  public DampedObjectShow(IEngine engine) {
    this.engine = engine;
  }

  public IPhysObject SpawnObject(
    IWorldItem pickup
  ) => SpawnObject(
    pickup,
    new(random.NextSingle() * 0.9f + 0.45f, 0.6f)
  );

  public IPhysObject SpawnObject(
    IWorldItem Pickup,
    Vector2 Position
  ) => SpawnObject(
    Pickup,
    Position,
    new(1.1f * random.NextSingle() + 0.55f, 0.0f)
  );

  public IPhysObject SpawnObject(
    IWorldItem pickup,
    Vector2 position,
    Vector2 velocity
  ) => SpawnObject(
    pickup,
    position,
    velocity,
    DefaultBounciness,
    DefaultLinearDamping,
    DefaultFrictionDamping
  );

  public IPhysObject SpawnObject(
    IWorldItem pickup,
    Vector2 position,
    Vector2 velocity,
    float bounciness,
    float linearDamping,
    float frictionDamping
  ) {
    ObjPhysObject obj = new(
      position,
      pickup.SpriteOverride ?? engine.GetSpriteFetcher().GetSprite(pickup.RID),
      pickup
    ) {
        Velocity = velocity,
        Position = position,
        Bounciness = bounciness,
        LinearDamping = linearDamping,
        FrictionDamping = frictionDamping
    };

    objects.Add(obj);
    return obj;
  }

  public void Update(float delta) {
    foreach (ObjPhysObject ob in objects) {
      // bounce_threshold
      float px_width = ob.Sprite.Dims.X / 192.0f;
      float max_x = 0.5f - (px_width / 2);
      if (ob.Velocity.LengthSquared() < 0.001 && ob.Position.Y < 0.01) {
        ob.Position = new(ob.Position.X, 0.0f);
        ob.Velocity = Vector2.Zero;
      } else {
        ob.Velocity += Gravity * delta;
        ob.Velocity *= MathF.Exp(delta * -ob.LinearDamping);
        ob.Position += ob.Velocity * delta;

        Vector2 low_bound = new(-max_x, 0.0f);
        Vector2 hi_bound = new(max_x, 50.0f);

        // stores the raw dist we're oob by
        Vector2 drift_factor = Vector2.Clamp(ob.Position, low_bound, hi_bound) - ob.Position;
        // for each bounce:
        // correct for position drift * bounce
        Vector2 drift_factor_abs = Vector2.Abs(drift_factor);

        // true if we're past a wall, else false
        Vector2 has_collide = new(
          drift_factor_abs.X > 0.0 ? 1 : 0,
          drift_factor_abs.Y > 0.0 ? 1 : 0
        );

        // true if we want to bounce off the wall, else false
        Vector2 has_rebound = has_collide * new Vector2(
          MathF.Abs(ob.Velocity.X) > 0.25f ? 1 : 0,
          MathF.Abs(ob.Velocity.Y) > 0.25f ? 1 : 0
        );

        // sliding if colliding and not rebounding
        Vector2 has_slide = has_collide * (Vector2.One - has_rebound);

        if (has_collide.X > 0 || has_collide.Y > 0) {
          ob.Position += drift_factor + (drift_factor * has_rebound * ob.Bounciness);
          Vector2 velocity_correction = ob.Velocity * has_collide + (ob.Velocity * has_rebound) * ob.Bounciness;
          ob.Velocity -= velocity_correction;
        }

        // lastly: implement sliding friction
        // decay further only if sliding - else, do nothing
        ob.Velocity = new(
          has_slide.Y > 0.5 ? ob.Velocity.X * MathF.Exp(delta * -ob.FrictionDamping) : ob.Velocity.X,
          has_slide.X > 0.5 ? ob.Velocity.Y * MathF.Exp(delta * -ob.FrictionDamping) : ob.Velocity.Y
        );
      }
    }
  }

  public IEnumerable<IPhysObject> GetPhysObjects() {
    return objects;
  }

  public void RemovePhysObject(IPhysObject obj) {
    if (obj is ObjPhysObject obp) {
      objects.Remove(obp);
    }
  }
}