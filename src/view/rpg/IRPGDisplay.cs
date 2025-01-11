using System.Numerics;

namespace digipet.view.rpg;

public interface IRPGDisplay {
  public Vector2 WorldOrigin { get; set; }
  public float WorldScale { get; set; }
  public Vector2 SizePx { get; set; }
}