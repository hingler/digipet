using digipet.framework;
using digipet.sim;
using digipet.sim.water;
using digipet.world.pet.task.tasks;

namespace digipet.world.pet.task.water;

#nullable enable

public class QuaffTask : IPetTask {
  private readonly ChainedPetTask task;
  private readonly IPetModel model;
  private readonly IPhysObject bowl;
  private readonly IWaterSource source;

  public bool Interruptable => false;

  public QuaffTask(IPetModel model, IPhysObject bowl, IWaterSource source) {
    this.model = model;
    this.bowl = bowl;
    this.source = source;
    task = new ChainedPetTask();

    InitializeChain();
  }

  private void InitializeChain() {

    task.AddTask(new MotionTask(bowl));
    task.AddTask(new DrinkTask(model, source));
  }

  public void BeginTask() {
    task.BeginTask();
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    return task.Update(prev_state, delta);
  }

  public bool Complete() {
    return task.Complete();
  }

  public int GetPriority() {
    return task.GetPriority();
  }
}