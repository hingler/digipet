using System;
using System.Numerics;
using digipet.pet;
using digipet.util;

namespace digipet.world.pet.task.tasks;

#nullable enable

public class PassiveTask : IPetTask {
  private MotionTask wander;
  private IdleTask idle;
  private bool is_wandering;
  private Random random;

  public bool Interruptable => true;

  public PassiveTask() {
    random = new();
    wander = new MotionTask(GetRandomPosition());
    idle = new IdleTask(GetRandomIdleInterval(), PetEmote.Neutral);
    is_wandering = false;
  }

  private IPositionable GetRandomPosition() {
    Vector2 pos = new((random.NextSingle() - 0.5f) * 0.9f, 0.0f);
    return new VectorPositionable(pos);
  }

  private double GetRandomIdleInterval() {
    return 1.0 * (random.Next() % 5) + 3.0;
  }

  public void BeginTask() {
    wander = new MotionTask(GetRandomPosition());
    idle = new IdleTask(GetRandomIdleInterval() + 5.0, PetEmote.Neutral);
    is_wandering = false;
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    IPetState new_state;
    if (is_wandering) {
      new_state = wander.Update(prev_state, delta);
      if (wander.Complete()) {
        is_wandering = false;
        wander = new MotionTask(GetRandomPosition());
      }
    } else {
      new_state = idle.Update(prev_state, delta);
      if (idle.Complete()) {
        is_wandering = true;
        idle = new IdleTask(GetRandomIdleInterval(), PetEmote.Neutral);
      }
    }

    return new_state;
  }

  public bool Complete() {
    return false;
  }

  public int GetPriority() {
    return 1;
  }
}