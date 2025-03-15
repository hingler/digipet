using System.Numerics;
using digipet.framework;
using digipet.rpg;
using digipet.sim;
using digipet.sprite.attrib;
using digipet.util;

namespace digipet.world.simple;

public class DampedObjectShow : IPhysWorld, IWorldManager {
  public float FloorHeight => 0.8f;

  private readonly HashSet<ObjPhysObject> objects = [];

  private readonly IEngine engine;

  public Vector2 Gravity { get; set; } = new(0.0f, -2.1f);
  public float DefaultBounciness = 0.65f;
  public float DefaultLinearDamping = 0.2f;
  public float DefaultFrictionDamping = 3.0f;

  private readonly HashSet<IBoundaryHandler> boundary_handler = [];
  private readonly Dictionary<IPhysObject, List<CollisionData>> active_collisions = [];

  private readonly Random random = new();

  // tag collisions here

  public DampedObjectShow(IEngine engine) {
    this.engine = engine;
  }

  public void AddBoundaryHandler(IBoundaryHandler handler) {
    boundary_handler.Add(handler);
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
    ClearCollisionTable();
    foreach (ObjPhysObject ob in objects) {
      // bounce_threshold
      float px_width = ob.Sprite.Dims.X / 192.0f;
      if (ob.Velocity.LengthSquared() < 0.001 && ob.Position.Y < 0.01) {
        ob.Position = new(ob.Position.X, 0.0f);
        ob.Velocity = Vector2.Zero;
      } else {
        ob.Velocity += Gravity * delta;
        ob.Velocity *= MathF.Exp(delta * -ob.LinearDamping);
        ob.Position += ob.Velocity * delta;

        // boundary handler is here
        foreach (IBoundaryHandler handler in boundary_handler) {
          // hoqw do we want to handle variant phys?
          CollisionData boundary_correction = handler.HandleBoundaries(
            ob, 
            ob.Bounciness,
            ob.LinearDamping,
            ob.FrictionDamping,
            delta
          );

          ob.Position += boundary_correction.DeltaPos;
          ob.Velocity += boundary_correction.Impulse;

          AddCollision(ob, boundary_correction);
        }
      }
    }
  }

  public IReadOnlyList<CollisionData> GetCollisions(IPhysObject ob) {
    if (!active_collisions.TryGetValue(ob, out List<CollisionData> datum)) {
      return [];
    }

    return datum;
  }

  private void ClearCollisionTable() {
    foreach (List<CollisionData> datum in active_collisions.Values) {
      datum.Clear();
    }
  }

  private void AddCollision(IPhysObject target, CollisionData data) {
    if (!active_collisions.TryGetValue(target, out List<CollisionData> datum)) {
      datum = [];
      active_collisions.Add(target, datum);
    }

    datum.Add(data);
  }

  public IEnumerable<IWorldEntity> GetEntities() => objects;

  public IEnumerable<IPhysObject> GetPhysObjects() {
    return objects;
  }

  public void RemovePhysObject(IPhysObject obj) {
    if (obj is ObjPhysObject obp) {
      objects.Remove(obp);
    }
  }
}