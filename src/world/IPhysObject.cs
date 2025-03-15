using System.Numerics;
using digipet.image;
using digipet.rpg;
using digipet.sim;
using digipet.util;

namespace digipet.world;

#nullable enable 

public interface IPhysObject : IWorldEntity {

  // sprite used to display this object

  // pickup associated with this object
  public IWorldItem? Pickup { get; }

  public bool FlipX { get; set; }

  // how do we want to do force?
  public void SetWorldDims(Vector2 dims);
  public void ApplyForce(Vector2 force, float delta);
  public void ApplyImpulse(Vector2 impulse);
  public void SetVelocity(Vector2 velocity);
  public void Halt();
}