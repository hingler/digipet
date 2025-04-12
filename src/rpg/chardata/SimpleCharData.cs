using digipet.framework;
using digipet.image;
using digipet.rpg.context;
using digipet.rpg.context.simple;
using digipet.rpg.model.behavior;
using digipet.rpg.model.demo;

namespace digipet.rpg.chardata;

#nullable enable

// thought for now
// ICharData repo needs to build itself off other sub-repos, namely:
// - merc progression
// - pet data
// - matching pet progression

// what does that mean
// - pet progression will need to be fetchable with pet data
public class SimpleCharData : ICharData { 
  public int RID { get; set; } = 0;
  public ICharStatsBase BaseStats { get; set; }
  public string Name { get; set; }

  public float Width { get; set; } = 0.5f;

  // combat style / class: still need to figure out
  public CharClass Class { get; set; }
  public CombatStyle Style { get; set; }

  private SimpleCharProgression progression;

  private readonly ISprite sprite;
  public SimpleCharData(IEngine engine) {
    BaseStats = new SimpleCharStats();
    Name = "placeholder";
    Class = CharClass.Attack;
    Style = CombatStyle.Melee;

    sprite = engine.GetSpriteFetcher().GetSprite(digipet.sprite.attrib.SpriteID.STAT_FUN);
    progression = new();
  }

  // 
  public CharInfo ToCharInfo() {
    return new() {
      Stats = new SimpleCharStats(BaseStats, Width, progression.GetAbilities()),
      CharSprite = sprite,
      BehaviorModel = new GreedyChargeModel(),
      Name = Name
    };
  }

  public ISprite? SpriteOverride => sprite;

  // how do we want to do sprite behavior?
  // make it the responsibility of the class
}