using System;
using System.Numerics;
using digipet.image;
using digipet.sim;

namespace digipet.world.simple;

public class ObjPhysObject : IPhysObject {
  private ISprite sprite_;
  private readonly IWorldItem pickup_;

  private Vector2 velocity_;

  public bool Active => true;
  public ISprite Sprite { get => sprite_; }
  public IWorldItem Pickup { get => pickup_; }
  public Vector2 Position { get; set; }
  public Vector2 Velocity { get => velocity_; set => velocity_ = value; }

  // fix later
  public Vector2 WorldDims { get; set; } = Vector2.One * 0.1f;

  public bool FlipX { get; set; }

  public float Bounciness = 0.7f;
  public float LinearDamping = 0.2f;

  public float FrictionDamping = 3.0f;

  public ObjPhysObject(
    Vector2 spawnPos,
    ISprite sprite,
    IWorldItem pickup
  ) {
    pickup_ = pickup;
    sprite_ = sprite;
    Position = spawnPos;
  }

  public void Tick(double delta) {}
  public void SetWorldDims(Vector2 dims) {
    WorldDims = dims;
  }

  public void ApplyForce(Vector2 force, float delta) {
    velocity_ += force * delta;
  }

  public void ApplyImpulse(Vector2 impulse) {
    velocity_ += impulse;
  }

  public void SetVelocity(Vector2 velocity) {
    velocity_ = velocity;
  }

  public void Halt() {
    velocity_ = Vector2.Zero;
    Position = new(Position.X, 0.0f);
  }
}