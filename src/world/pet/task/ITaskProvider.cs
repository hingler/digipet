using System.Collections.Generic;

namespace digipet.world.pet.task;

public interface ITaskProvider {
  // returns an unordered list of all available tasks atm
  public IReadOnlyList<IPetTask> GetTasks();
}

// what tasks do i want rn?
// - a chained task for walking up to some food, eating it, then reacting to it
//   - need some feedback on "animation state"
// - an idle wandering task
