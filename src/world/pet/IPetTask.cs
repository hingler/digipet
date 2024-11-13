namespace digipet.world.pet;

public interface IPetTask {
  // tells this task that it's just been enqueued
  void BeginTask();

  // updates the target and returns
  // - receive prev state
  IPetState Update(IPetStateReadOnly prev_state, double delta);

  // wb cases where we want multiple tasks to be performed?

  // returns complete if the state
  bool Complete();

  // returns the present priority of this task
  // if LT 0, then ignored
  int GetPriority();

  bool Interruptable { get; }
}