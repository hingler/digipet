using System.Numerics;
using digipet.pet;

namespace digipet.world.pet;

public class SimplePetObject : IPetState, IPetStateReadOnly {
  public SimplePetObject() {}
  public SimplePetObject(IPetStateReadOnly state) {
    Position = state.Position;
    Facing = state.Facing;
    PetState = state.PetState;
    Animation = state.Animation;
  }
  public Vector2 Position { get; set; } = Vector2.Zero;
  public Vector2 Facing { get; set; } = Vector2.UnitX;
  public PetAction PetState { get; set; } = PetAction.IDLE;
  public PetEmote Emote { get; set; } = PetEmote.Neutral;

  public IPetStateReadOnly AsReadOnly() {
    return this;
  }

  // how do we want to enqueue animations??
  // keep separate - infer animation based on pet state
  // emotional model - action precedes emotion :-)
  // but we have some feedback here 

  public IAnimationState Animation { get; set; }
  
}