using System.Numerics;

namespace digipet.image;

// image sprite
public interface ISprite {
  // this one's pretty mUch just a memory handle

  Vector2 Dims { get; }
}