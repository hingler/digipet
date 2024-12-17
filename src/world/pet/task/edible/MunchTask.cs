using System;
using System.Collections.Generic;
using System.Numerics;
using digipet.framework;
using digipet.sim;
using digipet.sim.edible;
using digipet.sim.stub;
using digipet.user;
using digipet.user.inventory;
using digipet.util;
using digipet.world.pet.task.tasks;

namespace digipet.world.pet.task.edible;

#nullable enable

public class MunchTask : IPetTask {
  private ChainedPetTask? task;
  private readonly IPhysWorld world;
  private IPhysObject? currentTarget;
  private IEdiblePickup? ediblePickup;
  private readonly IEngine engine;
  private readonly SimProvider provider;
  private readonly IUserData userdata;

  public bool Interruptable => false;
  public MunchTask(IEngine engine, SimProvider provider, IUserData userdata) {
    this.engine = engine;
    this.provider = provider;
    this.userdata = userdata;
    world = engine.GetPhysWorld();
    currentTarget = null;
    task = null;
  }

  public void BeginTask() {
    if (currentTarget != null && ediblePickup != null) {
      InitializeChain(currentTarget, ediblePickup);
    }

    task?.BeginTask();
    if (currentTarget == null && task != null && task.Complete()) {
      task = null;
    }
  }

  private void InitializeChain(IPhysObject ob, IEdiblePickup ep) {
    ChainedPetTask chain = new();
    chain.AddTask(new MotionTask(ob));

    IPetModel model = provider.GetPetModel();

    ChainedPetTask task_success = new();
    task_success.AddTask(new ConsumeTask(world, ob));
    task_success.AddTask(new PauseTask(2.0));
    task_success.AddTask(new EatReactionTask(ep, provider.GetPetModel()));

    ChainedPetTask task_fail = new();
    task_fail.AddTask(new RejectTask(engine, ob, userdata));

    // tba: poll model and check hunger
    chain.AddTask(new BranchTask(
      () => model.CanEat(ep), 
      task_success, 
      task_fail
    ));

    task = chain;
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    return task?.Update(prev_state, delta) ?? new SimplePetObject();
  }

  public bool Complete() {
    bool isComplete = task?.Complete() ?? true;
    if (isComplete && currentTarget != null) {
      currentTarget = null;
    }

    return isComplete;
  }

  public int GetPriority() {
    if (!Complete()) {
      return 100;
    }

    IReadOnlyCollection<IPhysObject> objects = world.GetPhysObjects();
    // complete - try to re-enqueue
    foreach (IPhysObject ob in objects) {
      if (ob.Pickup is IEdiblePickup ep) {
        if (currentTarget != ob) {
          currentTarget = ob;
          ediblePickup = ep;
        }

        return 100;
      }
    }

    task = null;
    return -1;
  }
}

// idle anim - take 3 bites, then delete the food

// up next: sample the food, and formulate a desirability score (react accordingly)