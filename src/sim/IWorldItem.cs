using digipet.image;

namespace digipet.sim;

#nullable enable

// no defn yet
public interface IWorldItem {
  int RID { get; }
  string Name { get; }
  string Description { get; }
  int StorePrice { get; }

  ISprite? SpriteOverride { get; }


}

// enough data for the store to receive it and get the drill