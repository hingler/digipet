using System.Dynamic;
using System.Numerics;
using digipet.image;
using digipet.sim;

namespace digipet.world.simple;

public class SimplePhysObject : IPhysObject {
  private readonly IWorldItem pickup_;
  private readonly Vector2 position_;

  public ISprite Sprite { get => pickup_.Sprite; }
  public IWorldItem Pickup { get => pickup_; }
  public Vector2 Position { get => position_; }
  public SimplePhysObject(
    Vector2 spawnPos,
    IWorldItem pickup
  ) {
    pickup_ = pickup;
    position_ = spawnPos;
  }


}