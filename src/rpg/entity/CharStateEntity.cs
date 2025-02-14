using System.Numerics;
using digipet.image;
using digipet.rpg;
using digipet.rpg.context.simple;

namespace digipet.rpg.entity;

#nullable enable

public class CharStateEntity : IWorldEntity {
  private readonly SimpleCharState character;
  public bool Active => character.CurrentHP > 0;

  // tba: cache on tick??
  public Vector2 Position => new(character.Position.X, character.Position.Y + sprite_height / 2);
  public ISprite Sprite { get; }

  // create a scene to design little nodes for this
  public Vector2 WorldDims {
    get => new(character.Width, sprite_height);
  }

  private readonly float sprite_height;

  public CharStateEntity(SimpleCharState character, ISprite sprite) {
    Sprite = sprite;
    this.character = character;

    float pix_ratio = character.Width / sprite.Dims.X;
    float height = pix_ratio * sprite.Dims.Y;

    sprite_height = height;
  }

  public void Tick(double delta) {
    // nothing happens - eventually we'll animate
  }

}