using digipet.image;

namespace digipet.sim;

public class EmptyWorldItem : IWorldItem
{
  public int RID => -1;
  public ISprite SpriteOverride { get; }

  public EmptyWorldItem(ISprite sprite) {
    SpriteOverride = sprite;
  }
}