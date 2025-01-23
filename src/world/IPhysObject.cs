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

  public bool FlipX { get; set; }

  // how do we want to do force?
  public void ApplyForce(Vector2 force, float delta);
  public void ApplyImpulse(Vector2 impulse);
  public void Halt();
}