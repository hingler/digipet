using System;
using System.Numerics;
using digipet.pet;
using digipet.util;

namespace digipet.world.pet.task.tasks;

// tba: put food approach dingus here

class MotionTask : IPetTask {
  static readonly double speed = 0.08;
  private double last_position_delta = speed;
  private Vector2 last_position = Vector2.Zero;

  private readonly ILogger logger;

  private readonly IPositionable target;

  private readonly AnimationTicker ticker;

public bool Interruptable { get; }
  
  public MotionTask(IPositionable target) : this(target, false) {}

  public MotionTask(IPositionable target, bool can_interrupt) {
    this.target = target;
    logger = this.GetLogger();
    ticker = new();

    Interruptable = can_interrupt;
  }
  public void BeginTask() {
    last_position_delta = speed;
    logger.Log("initializing motion");
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    IAnimationState data = prev_state.Animation;
    SimplePetObject new_state = new(prev_state);

    // thinking: separate this into some sort of "deduper"
    // - pass in animation and have it do this logic
    // - return true  for a tick frame, false otherwise
    // - (since we'll need it for eating as well)
    if (ticker.Update(data, delta)) {
      Vector2 pos = prev_state.Position;
      double dx = target.Position.X - pos.X;
      double frame_duration = data.FrameDuration;

      double travel_dist = Math.Clamp(dx, -speed * frame_duration, speed * frame_duration);
      last_position_delta = Math.Abs(travel_dist);

      pos.X += (float)travel_dist;

      new_state.Position = pos;
      new_state.Facing = new Vector2(Math.Sign(dx), 0.0f);
      last_position = pos;
    }

    new_state.Emote = PetEmote.Neutral;
    new_state.PetState = PetAction.MOVING;

    return new_state;
  }

  public bool Complete() {
    // account for velocity?
    if (
      (last_position - target.Position).Length() < 0.08
      && target.Velocity.Length() < 0.02
    ) {
      return true;
    }

    return false;
  }

  public int GetPriority() {
    return 1;
  }
}