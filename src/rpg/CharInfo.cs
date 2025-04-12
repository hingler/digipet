using digipet.framework;
using digipet.image;
using digipet.rpg.context;
using digipet.rpg.context.simple;
using digipet.rpg.model;
using digipet.rpg.model.demo;

namespace digipet.rpg;

#nullable enable

// wanna change this?
public struct CharInfo {
  public ICharStats Stats;
  public ISprite? CharSprite;
  public IBehaviorModel BehaviorModel;
  public string Name;

  public static CharInfo GetPlaceholderCharInfo(IEngine engine) {
    return GetPlaceholderCharInfo(engine.GetSpriteFetcher().GetSprite(sprite.attrib.SpriteID.STAT_FUN));
  }
  public static CharInfo GetPlaceholderCharInfo(ISprite? sprite = null) {
    CharInfo info = new() {
      Stats = new SimpleCharStats() {
        Attack = 1,
        Wisdom = 2,
        Defense = 3,
        Weight = 4,
        Vitality = 5,
        Speed = 6,
        Width = 0.5f
      },

      CharSprite = sprite,
      BehaviorModel = new TrivialBehaviorModel(),
      Name = "CHAR"
    };

    return info;
  }
}

// how do we identify duplicates? feels like this struct is inadequate

// - charinfo should be a quick drop-in for combat
// - use something more sturdy for storage

// - thinking: we want to convert from pet -> char, so...
//   - contain a reference to the base pet (that's a tba)
//   - contains stats
//   - contain combat class info? (ie that we can map to a behavioral model)
//   - contains name (string field)
//   - shouldn't need an ID field and can rely on ptr (each is unique!)

//   - everything a charinfo would have, minus behavioral model
//   - convert when we pass in to combat manager - else, leave as our interface
//   - most likely: repo infers everything from fetched type

// - prob just make a stub impl that converts trivially, and use that for now
//   - repo containing *all* chars
//   - list of presently active chars
//   - on exit: store active team comp so we can fetch it later