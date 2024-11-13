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

  // removes a phys object from sim
  public void RemovePhysObject(IPhysObject obj);

  public IReadOnlySet<IPhysObject> GetPhysObjects();

  // spawns phys object somewhere in the room
  // public void SpawnObject(
  //   IPhysObject obj,
  //   Vector2 position,
  //   Vector2 velocity
  // );
}