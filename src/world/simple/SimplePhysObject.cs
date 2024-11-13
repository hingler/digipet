using System.Dynamic;
using System.Numerics;
using digipet.image;
using digipet.sim;

namespace digipet.world.simple;

public class SimplePhysObject : IPhysObject {
  private ISprite sprite_;
  private readonly IWorldItem pickup_;
  private readonly Vector2 position_;

  public ISprite Sprite { get => sprite_; }
  public IWorldItem Pickup { get => pickup_; }
  public Vector2 Position { get => position_; }
  public Vector2 Velocity { get => Vector2.Zero; }
  public SimplePhysObject(
    Vector2 spawnPos,
    ISprite sprite,
    IWorldItem pickup
  ) {
    pickup_ = pickup;
    sprite_ = sprite;
    position_ = spawnPos;
  }


}