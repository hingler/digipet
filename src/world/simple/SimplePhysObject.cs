using System.Numerics;
using digipet.image;
using digipet.sim;

namespace digipet.world.simple;

public class SimplePhysObject : IPhysObject {
  private ISprite sprite_;
  private readonly IWorldItem pickup_;
  private readonly Vector2 position_;

  public bool Active => true;

  public bool FlipX { get; set; }

  public ISprite Sprite { get => sprite_; }
  public IWorldItem Pickup { get => pickup_; }
  public Vector2 Position { get => position_; }
  public Vector2 Velocity { get => Vector2.Zero; }

  public Vector2 WorldDims { get => Sprite.Dims * 0.1f; }
  public SimplePhysObject(
    Vector2 spawnPos,
    ISprite sprite,
    IWorldItem pickup
  ) {
    pickup_ = pickup;
    sprite_ = sprite;
    position_ = spawnPos;
  }

  public void Tick(double delta) {}

  // could use OnTick for force accumulation - would work well?
  // nothing
  public void SetWorldDims(Vector2 dims) {}
  public void ApplyForce(Vector2 force, float delta) { /* no op */ }
  public void ApplyImpulse(Vector2 force) { /* no op */ }
  public void SetVelocity(Vector2 velocity) { /* no op */ }
  public void Halt() {}

}