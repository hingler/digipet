using System.Numerics;

namespace digipet.rpg.overworld;

public struct OverworldNode {
  public Vector2 Position;
  public string Identifier;

  public override readonly bool Equals(object other) {
    return other is OverworldNode node && Equals(node);
  }

  public readonly bool Equals(OverworldNode other) {
    return other.Position == Position && other.Identifier == Identifier;
  }

  public static bool operator==(OverworldNode left, OverworldNode right) {
    return left.Equals(right);
  }

  public static bool operator!=(OverworldNode left, OverworldNode right) {
    return !(left == right);
  }

  public override readonly int GetHashCode() {
    int v_hash = Position.GetHashCode();
    int i_hash = Identifier.GetHashCode();

    return (v_hash * 31) ^ i_hash;
  }
}