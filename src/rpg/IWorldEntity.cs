using System.Numerics;
using digipet.image;
using digipet.util;

namespace digipet.rpg;

#nullable enable

// time goes
public interface IWorldEntity : IPositionable {
  public void Tick(double delta);

  bool Active { get; }
  ISprite? Sprite { get; }

  Vector2 WorldDims { get; }

  // tba: quick renderer for these  
}