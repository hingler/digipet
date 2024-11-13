using digipet.framework;
using digipet.util;

namespace digipet.world.pet.task.edible;

public class ConsumeTask : IPetTask {
  private AnimationTicker ticker = new();
  private readonly IPhysWorld physWorld;
  private IPhysObject target;

  private int bite_count = 0;

  private static readonly int BITE_THRESHOLD = 6;

  private readonly ILogger logger;

  public bool Interruptable => false;

  public ConsumeTask(IPhysWorld world, IPhysObject target) {
    physWorld = world;
    this.target = target;
    logger = this.GetLogger();
  }

  public void BeginTask() {
    logger.Log("beginning to eat...");
    bite_count = 0;
  }

  public bool Complete() {
    return bite_count >= BITE_THRESHOLD;
  }

  public int GetPriority() {
    return 1;
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    IPetState newState = new SimplePetObject(prev_state);
    newState.PetState = PetAction.IDLE;
    if (ticker.Update(prev_state.Animation, delta)) {
      bite_count++;

      if (bite_count == BITE_THRESHOLD) {
        physWorld.RemovePhysObject(target);
      }
    }

    return newState;
  }
}