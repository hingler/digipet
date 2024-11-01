using System.Collections.Generic;
using digipet.image;
using digipet.sim;

namespace digipet.world;

#nullable enable

public interface IObjectShow {
  // spawns a new physobject per implementer behavior
  // returns false if the object could not be spawned
  public bool SpawnObject(
    IWorldItem pickup
  );

  // returns the highest-priority phys object in this scene
  public IPhysObject? FetchByPriority();

  // removes a phys object from sim
  public void RemovePhysObject(IPhysObject obj);

  public ICollection<IPhysObject> GetPhysObjects();

  // spawns phys object somewhere in the room
  // public void SpawnObject(
  //   IPhysObject obj,
  //   Vector2 position,
  //   Vector2 velocity
  // );
}