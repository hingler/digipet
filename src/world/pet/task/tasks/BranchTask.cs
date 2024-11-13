namespace digipet.world.pet.task.tasks;

#nullable enable

public class BranchTask : IPetTask {
  public delegate bool CallbackType();
  private readonly CallbackType predicate;

  private readonly IPetTask? branch_t;
  private readonly IPetTask? branch_f;

  private IPetTask? branch_active;

  public bool Interruptable {
    get {
      return (branch_t?.Interruptable ?? true) && (branch_f?.Interruptable ?? true);
    }
  }

  public BranchTask(
    CallbackType predicate,
    IPetTask? branch_t,
    IPetTask? branch_f

  ) {
    this.predicate = predicate;
    this.branch_t = branch_t;
    this.branch_f = branch_f;

    branch_active = null;
  }

  public void BeginTask() {
    if (predicate()) {
      branch_active = branch_t;
    } else {
      branch_active = branch_f;
    }

    branch_active?.BeginTask();
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    return branch_active?.Update(prev_state, delta) ?? new SimplePetObject(prev_state);
  }

  public bool Complete() {
    return branch_active?.Complete() ?? true;
  }

  public int GetPriority() {
    return branch_active?.GetPriority() ?? -1;
  }
}