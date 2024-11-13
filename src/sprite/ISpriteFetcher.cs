using digipet.image;

namespace digipet.sprite.attrib;

public enum SpriteID {
  STAT_FOOD,
  STAT_WATER,
  STAT_FUN,
  STAT_SOCIAL,
  STAT_ENERGY,

  SKY_FRONT,
  SKY_MID,
  SKY_BACK,

  ICON_STEP,
  ICON_STEP_12PX,

  ICON_SLEEP,
  ICON_SLEEP_12PX,

  SPRITE_PET,

  ICON_SELECTOR,

  WATER_SPOUT,
  WATER_BOWL,
  WATER_STREAM,

  OFFSET_EMOTE = 4096,

  OFFSET_FIXTURE = 8192,


  // for larger datasets, let's just give it a large offset and catalogue things separately
  OFFSET_FOOD = 16384
}

public interface ISpriteFetcher {
  public ISprite GetSprite(SpriteID id);
  public ISprite GetSprite(int sprite_id);
  public IAnimatedSprite GetAnimatedSprite(SpriteID id);

}