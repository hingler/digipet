using System.Collections.Generic;
using digipet.world.pet.task;

namespace digipet.world.pet.task;

public class SimpleTaskProvider() : ITaskProvider {
  private readonly IList<IPetTask> tasks = [];

  public void AddTask(IPetTask task) {
    tasks.Add(task);
  }

  public IReadOnlyList<IPetTask> GetTasks() {
    return tasks.AsReadOnly();
  }
}