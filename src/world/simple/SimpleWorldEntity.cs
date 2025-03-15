using System.Numerics;
using digipet.image;
using digipet.rpg;

namespace digipet.world.simple;

#nullable enable

public class SimpleWorldEntity : IWorldEntity {
  public bool Active => true;
  public Vector2 Position { get; set; }
  public Vector2 Velocity { get; set; }
  public ISprite? Sprite { get; }

  public Vector2 WorldDims { get; }

  public SimpleWorldEntity(
    ISprite sprite,
    Vector2 init_position,
    Vector2 world_scale
  ) {
    Position = init_position;
    Sprite = sprite;
    WorldDims = sprite.Dims * world_scale;
  }

  // let's just go with this for now
  // no gravity or anything - 
  public void Tick(double delta) {}
}