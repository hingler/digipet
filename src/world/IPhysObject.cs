using System.Numerics;
using digipet.image;
using digipet.sim;
using digipet.util;

namespace digipet.world;

#nullable enable 

public interface IPhysObject : IPositionable {

  // sprite used to display this object
  public ISprite Sprite { get; }

  // pickup associated with this object
  public IWorldItem? Pickup { get; }
}