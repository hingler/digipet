using System.Diagnostics;
using digipet.util;
using digipet.world.pet.animation;
using digipet.world.pet.task;
using digipet.world.pet.task.builder;

namespace digipet.world.pet.controller;

#nullable enable

public class SimplePetController : IPetController {
  private readonly AnimationManager animation_manager;
  private readonly PetTaskManager task_manager;
  private readonly SimpleTaskProvider provider;

  private IPetState state = new SimplePetObject();

  private IPetTask? currentTask = null;
  private IAnimationData? currentAnimation = null;

  private readonly ILogger logger;

  public SimplePetController() {
    animation_manager = new();
    provider = new();
    task_manager = new(provider);

    logger = this.GetLogger();
  }

  public void AddAnimationHandler(PetAnimation animation, IAnimationHandler handler) {
    animation_manager.AddHandler(animation, handler);
  }

  public void AddTask(IPetTask task) {
    // retool
    // - builder for pet tasks (references priority)
    // - pet task itself (exactly the same thing - build and call "begin")
    provider.AddTask(new TaskBuilderWrap(task));
  }

  public void AddTaskFactory(IPetTaskFactory fac) {
    provider.AddTask(fac);
  }

  public void Update(double delta) {
    IPetTask max_task = task_manager.GetNextTask();

    if (currentTask == null 
      || (
        (max_task.GetPriority() > currentTask.GetPriority())
        && currentTask.Interruptable
      )
    ) {
      logger.Log("replacing task with: ", max_task.GetType(), ", prio ", max_task.GetPriority());
      max_task.BeginTask();
      currentTask = max_task;
    }

    // check complete BEFORE animating and AFTER update
    IPetState state_new = currentTask.Update(state.AsReadOnly(), delta);

    if (currentTask.Complete()) {
      // doesn't account for tasks which don't end (ie idle task, etc)
      // - check task list on delta interval
      logger.Log("Current task complete | fetching next task...");
      currentTask = task_manager.GetNextTask();
      logger.Log("fetched task: ", currentTask.GetType(), " , prio ", currentTask.GetPriority());
      currentTask.BeginTask();
      state_new = currentTask.Update(state.AsReadOnly(), 0.0001);
    }

    if (
      state_new.PetState != state.PetState
      || currentAnimation == null
      || currentAnimation.Complete()
    ) {
      // (tba: map from pet state to pet animation)
      currentAnimation = animation_manager.GetAnimationData(TranslatePetAction(state_new.PetState));
      logger.Log("enqueueing animation: ", currentAnimation.GetType());
      currentAnimation.Reset();
    }

    currentAnimation.Update(delta);

    state_new.Animation = currentAnimation;
    state = state_new;
  }

  private PetAnimation TranslatePetAction(PetAction action) {
    return action switch {
      PetAction.IDLE => PetAnimation.IDLE,
      PetAction.EATING => PetAnimation.IDLE,
      PetAction.PAUSE => PetAnimation.PAUSE,
      PetAction.EXPRESS => PetAnimation.EXPRESS,
      PetAction.MOVING => PetAnimation.ACTIVE,
      PetAction.SHAKE_HEAD => PetAnimation.REJECT,
      _ => PetAnimation.IDLE
    };
  }

  public IPetStateReadOnly GetPetState() {
    return state.AsReadOnly();
  }
}