using System.Numerics;
using digipet.image;

namespace digipet.rpg;

#nullable enable

// time goes
public interface ICombatEntity {
  public void Tick(double delta);

  bool Active { get; }
  Vector2 Position { get; }
  ISprite? Sprite { get; }

  Vector2 WorldDims { get; }

  // tba: quick renderer for these  
}