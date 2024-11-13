using System;
using System.Numerics;
using digipet.framework;
using digipet.util;

namespace digipet.world.pet.task.tasks;

#nullable enable

public class WanderTask : IPetTask {
  private MotionTask? subtask;
  private readonly Random random;

  public bool Interruptable => true;

  public WanderTask() {
    subtask = null;
    random = new();
  }

  public void BeginTask() {
    Vector2 target_position = new(random.NextSingle() * 0.9f - 0.45f, 0.0f);
    subtask = new MotionTask(new VectorPositionable(target_position));
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    if (subtask != null) {
      return subtask.Update(prev_state, delta);
    }

    return new SimplePetObject(prev_state);
  }

  public bool Complete() {
    if (subtask?.Complete() ?? true) {
      subtask = null;
      return true;
    }

    return false;
  }

  public int GetPriority() {
    // if we're in the idle state
    return 1;
  }
}