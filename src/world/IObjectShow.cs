using System.Collections.Generic;
using System.Numerics;
using digipet.image;
using digipet.sim;

namespace digipet.world;

#nullable enable

public interface IPhysWorld {
  // spawns a new physobject per implementer behavior
  // returns false if the object could not be spawned

  public float FloorHeight { get; }

  public Vector2 Gravity { get; }

  public IPhysObject SpawnObject(
    IWorldItem pickup
  );

  public IPhysObject SpawnObject(
    IWorldItem pickup,
    Vector2 position
  );

  public IPhysObject SpawnObject(
    IWorldItem pickup,
    Vector2 position,
    Vector2 velocity
  );

  public IPhysObject SpawnObject(
    IWorldItem pickup,
    Vector2 position,
    Vector2 velocity,
    float bounciness,
    float linearDamping,
    float frictionDamping
  );

  // removes a phys object from sim
  public void RemovePhysObject(IPhysObject obj);

  public IEnumerable<IPhysObject> GetPhysObjects();

  // spawns phys object somewhere in the room
  // public void SpawnObject(
  //   IPhysObject obj,
  //   Vector2 position,
  //   Vector2 velocity
  // );
}