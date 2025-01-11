using digipet.image;
using digipet.rpg.context;
using digipet.rpg.model;
using digipet.rpg.model.demo;

namespace digipet.rpg;

public struct CharInfo {
  public ICharStats Stats;
  public ISprite CharSprite;
  public IBehaviorModel BehaviorModel;
}