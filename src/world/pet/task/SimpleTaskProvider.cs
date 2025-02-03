using System.Collections.Generic;
using digipet.world.pet.task;

namespace digipet.world.pet.task;

public class SimpleTaskProvider() : ITaskProvider {
  private readonly IList<IPetTaskFactory> tasks = [];

  public void AddTask(IPetTaskFactory task) { 
    tasks.Add(task);
  }

  public IReadOnlyList<IPetTaskFactory> GetTasks() {
    return tasks.AsReadOnly();
  }
}