using digipet.framework;
using digipet.sim;
using digipet.sim.toy;
using digipet.world.pet.task.tasks;
using ToyData = System.Tuple<digipet.sim.toy.IToyActivator, int>;

namespace digipet.world.pet.task.toy;

#nullable enable

public class ToyInteractTask : IPetTask {
  private readonly IEngine engine;
  private readonly IToyModel toy_model;
  private readonly IFunModel fun_model;
  private ChainedPetTask? task;
  public bool Interruptable => true;

  public ToyInteractTask(IEngine engine, SimProvider provider) {
    this.engine = engine;
    toy_model = provider.GetToyModel();
    fun_model = provider.GetFunModel();
  }

  // wb: returning the chain here, instead of having to manage life cycle manually?
  public void BeginTask() {
    IPhysWorld world = engine.GetPhysWorld();
    IToyActivator? target = GetPriority_internal().Item1;

    if (target == null) {
      return;
    }

    IPhysObject? phys_target = world.GetPhysObjects().Where(o => o.Pickup?.RID == target.RID).SingleOrDefault();

    if (phys_target != null) {
      task = new ChainedPetTask();
      task.AddTask(new MotionTask(phys_target));
      task.AddTask(new CallbackTask(() => {
        if (GetTogglePriorityForToy(target) > 0) {
          target.Active = !target.Active;
        }
      }));

      task.BeginTask();
    }

  }

  // see if any toys should be toggled

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

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    if (task != null) {
      return task.Update(prev_state, delta);
    } else {
      return new SimplePetObject(prev_state);
    }
  }

  public bool Complete() => task?.Complete() ?? true;
}