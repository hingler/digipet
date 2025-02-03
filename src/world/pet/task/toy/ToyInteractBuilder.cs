using digipet.framework;
using digipet.sim;
using digipet.sim.toy;
using digipet.world.pet.task.tasks;
using ToyData = System.Tuple<digipet.sim.toy.IToyActivator, int>;

namespace digipet.world.pet.task.toy;

#nullable enable

public class ToyInteractBuilder : IPetTaskFactory {
  private readonly IEngine engine;
  private readonly IToyModel toy_model;
  private readonly IFunModel fun_model;

  public bool Interruptable => true;

  public ToyInteractBuilder(
    IEngine engine,
    SimProvider provider
  ) {
    this.engine = engine;
    toy_model = provider.GetToyModel();
    fun_model = provider.GetFunModel();
  }

  public IPetTask? CreatePetTask() {
    IToyActivator? target = GetPriority_internal().Item1;
    IPhysObject? world_target = engine.GetPhysWorld().GetPhysObjects()
      .Where(o => o.Pickup?.RID == target.RID)
      .SingleOrDefault();

    if (target != null && world_target != null) {
      ChainedPetTask task = new();
      task.AddTask(new MotionTask(world_target, true));
      task.AddTask(new CallbackTask(() => {
        target.Active = !target.Active;
      }));

      return task;
    }
    

    return null;
  }

  public int GetPriority() {
    return GetPriority_internal().Item2;
  }

  private ToyData GetPriority_internal() {
    int max_prio = -100;
    IToyActivator? max_toy = null;
    foreach (IToyActivator toy in toy_model.GetSpawnedToys()) {
      int prio = GetTogglePriorityForToy(toy);
      if (prio > max_prio) {
        max_prio = prio;
        max_toy = toy;
      }
    }

    return new(max_toy, max_prio);
  }

  public int GetTogglePriorityForToy(IToy toy) {
    int activate_priority = (int)(toy_model.GetRawInterest(toy) * 100);
    if (toy_model.IsActive(toy)) {
      return 25 - activate_priority;
    } else {
      // negative when prio drops below 
      return activate_priority - 35;
    }
  }


}