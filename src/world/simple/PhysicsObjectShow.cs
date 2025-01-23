using System;
using System.Collections.Generic;
using digipet.framework;
using digipet.sim;
using digipet.sprite.attrib;
using digipet.util;
using System.Numerics;
using System.Linq;

namespace digipet.world.simple;

#nullable enable

public class PhysicsObjectShow : IPhysWorld {
  public float FloorHeight => 0.8f;

  private readonly HashSet<IPhysObject> objects = new();
  private readonly ILogger logger;
  private readonly ISpriteFetcher fetcher;

  private static readonly Vector2 GRAVITY = new(0.0f, -2.1f);
  private static readonly float BOUNCINESS = 0.5f;
  private static readonly float MAX_X = 0.45f;

  private readonly Random random = new();

  public PhysicsObjectShow(IEngine engine) {
    fetcher = engine.GetSpriteFetcher();
    logger = this.GetLogger();
  }

  public IPhysObject SpawnObject(
    IWorldItem pickup
  ) => SpawnObject(
    pickup, 
    new(random.NextSingle() * 0.9f + 0.45f, 0.6f)
  );

  public IPhysObject SpawnObject(
    IWorldItem pickup,
    Vector2 position
  ) => SpawnObject(
    pickup,
    position,
    new(1.1f * random.NextSingle() + 0.55f, 0.0f)
  );

  public IPhysObject SpawnObject(
    IWorldItem pickup,
    Vector2 position,
    Vector2 velocity
  ) {

    ObjPhysObject obj = new(
      position,
      pickup.SpriteOverride ?? fetcher.GetSprite(pickup.RID),
      pickup
    );

    obj.ApplyImpulse(velocity);

    objects.Add(obj);
    return obj;
  }

  // ignore phys params
  public IPhysObject SpawnObject(
    IWorldItem pickup,
    Vector2 position,
    Vector2 velocity,
    float bounciness,
    float linearDamping,
    float frictionDamping
  ) => SpawnObject(pickup, position, velocity);

  public void Update(double delta) {
    foreach (ObjPhysObject ob in objects.Cast<ObjPhysObject>()) {

      if (ob.Position.Y > 0.01f || ob.Velocity.Length() > 0.01f) {
        // ie: if above-ground, or still clearly moving
        // (alt: if on-ground, AND not clearly moving)
        ob.ApplyForce(GRAVITY, (float)delta);
        ob.Position += ob.Velocity * (float)delta;
      }

      // ik whats happening
      // how do we keep objects stationary on the ground?

      if (ob.Position.Y < 0.0f && ob.Velocity.Y < 0.0f) {
        ob.Position = new(ob.Position.X, -ob.Position.Y * BOUNCINESS);
        ob.ApplyImpulse(new(0.0f, ob.Velocity.Y * -(1.0f + BOUNCINESS)));
        
        if (ob.Position.Y < 0.02f && ob.Velocity.Length() < 0.015f) {
          ob.Halt();
        }
      }

      if (ob.Position.X < -MAX_X || ob.Position.X > MAX_X) {
        float dx, max;
        if (ob.Position.X < 0) {
          dx = ob.Position.X + MAX_X;
          max = -MAX_X;
        } else {
          dx = ob.Position.X - MAX_X;
          max = MAX_X;
        }
        ob.Position = new(max - dx * BOUNCINESS, ob.Position.Y);
        ob.ApplyImpulse(new(ob.Velocity.X * -(1.0f + BOUNCINESS), -ob.Velocity.Y * (1.0f - BOUNCINESS)));
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