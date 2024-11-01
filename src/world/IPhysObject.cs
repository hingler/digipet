using System.Numerics;
using digipet.image;
using digipet.sim;

namespace digipet.world;

#nullable enable 

public interface IPhysObject {

  // sprite used to display this object
  public ISprite Sprite { get; }

  // pickup associated with this object
  public IWorldItem? Pickup { get; }

  // position of this physobject, wrt phys world
  public Vector2 Position { get; }
}