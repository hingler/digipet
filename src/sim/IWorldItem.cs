using digipet.image;

namespace digipet.sim;

#nullable enable

// no defn yet
public interface IWorldItem {
  int RID { get; }
  ISprite? SpriteOverride { get; }
}