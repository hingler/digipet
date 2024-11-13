using System.Numerics;
using digipet.pet;
using digipet.sim;

namespace digipet.world.pet;



// tba: animation handler for actions
public enum PetAction {
  IDLE,
  EATING,
  MOVING,
  EXPRESS,
  PAUSE,
  SHAKE_HEAD,
}

public interface IPetStateReadOnly {
  Vector2 Position { get; }
  Vector2 Facing { get; }
  PetAction PetState { get; }
  IAnimationState Animation { get; }
  PetEmote Emote { get; }
}

public interface IPetState {
  Vector2 Position { get; set; }
  // more 
  Vector2 Facing { get; set; }
  PetAction PetState { get; set; }
  IAnimationState Animation { get; set; }
  PetEmote Emote { get; set; }

  IPetStateReadOnly AsReadOnly();
}