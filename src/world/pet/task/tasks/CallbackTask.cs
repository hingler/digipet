namespace digipet.world.pet.task.tasks;

public class CallbackTask : IPetTask {
  private readonly Action task;
  private bool completed;

  public bool Interruptable => false;

  public CallbackTask(Action task) {
    this.task = task;
    completed = false;
  }

  public void BeginTask() {
    task();
    completed = true;
  }

  public IPetState Update(IPetStateReadOnly state, double delta) {
    return new SimplePetObject(state);
  }

  public bool Complete() => completed;
  
  public int GetPriority() {
    return 100;
  }
}