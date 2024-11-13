using System.Collections.Generic;
using digipet.util;

namespace digipet.world.pet.task;

public class PetTaskManager {

  private readonly ITaskProvider provider;
  private readonly ILogger logger;

  public PetTaskManager(ITaskProvider provider) {
    this.provider = provider;
    logger = this.GetLogger();
  }
  public IPetTask GetNextTask() {
    IReadOnlyList<IPetTask> tasks = provider.GetTasks();
    PriorityQueue<IPetTask, int> task_prio = new(new MaxComparer<int>());
    foreach (IPetTask task in tasks) {
      if (task.GetPriority() >= 0) {
        task_prio.Enqueue(task, task.GetPriority());
      }
    }

    IPetTask res = task_prio.Dequeue();
    return res;
  }
}