using System;
using digipet.pet;
using digipet.sim;
using digipet.sim.edible;
using digipet.util;
using digipet.world.pet.task.tasks;

namespace digipet.world.pet.task.edible;

public class EatReactionTask : IPetTask {
  private readonly IEdiblePickup target;
  private readonly IPetModel model;
  private IdleTask del_task = new();
  private readonly ILogger logger;

  public bool Interruptable => false;

  public EatReactionTask(IEdiblePickup target, IPetModel model) {
    this.target = target;
    this.model = model;

    logger = this.GetLogger();
  }

  public void BeginTask() {
    PetEmote emote;
    if (model.TryEat(target, out double reaction)) {
      int reaction_cast = (int)Math.Round(reaction);
      emote = reaction_cast switch {
        -5 or -4 => PetEmote.Ill,
        -3 or -2 => PetEmote.Tired,
        2 or 3 => PetEmote.Happy,
        4 or 5 => PetEmote.Flustered,
        _ => PetEmote.Glance,
      };

    } else {
      // poor containment - how can we resolve this?
      // not a good way to do it unfortunately
      logger.Error("attempted to eat something which is not currently edible!");
      emote = PetEmote.Scrunch;
    }

    del_task = new IdleTask(2.0, emote);
    del_task.BeginTask();
    // what to do in this case??
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    IPetState state = del_task.Update(prev_state, delta);
    state.PetState = PetAction.EXPRESS;
    return state;
  }

  public bool Complete() {
    return del_task.Complete();
  }

  public int GetPriority() {
    return 1;
  }
}