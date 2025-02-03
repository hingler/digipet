using System.Collections.Generic;
using digipet.util;
using digipet.world.pet.task.tasks;

namespace digipet.world.pet.task;

#nullable enable

public class PetTaskManager {

  private readonly ITaskProvider provider;
  private readonly ILogger logger;

  public PetTaskManager(ITaskProvider provider) {
    this.provider = provider;
    logger = this.GetLogger();
  }
  public IPetTask GetNextTask() {
    IReadOnlyList<IPetTaskFactory> tasks = provider.GetTasks();
    PriorityQueue<IPetTaskFactory, int> task_prio = new(new MaxComparer<int>());
    foreach (IPetTaskFactory task in tasks) {
      if (task.GetPriority() >= 0) {
        task_prio.Enqueue(task, task.GetPriority());
      }
    }

    IPetTask? res = null;
    while (res == null && task_prio.Count > 0) {
      res = task_prio.Dequeue().CreatePetTask();
    }

    if (res == null) {
      return new PassiveTask();
    }

    return res;
  }
}