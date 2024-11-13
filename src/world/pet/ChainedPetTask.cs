using System;
using System.Collections.Generic;
using System.Linq;

namespace digipet.world.pet;

public class ChainedPetTask : IPetTask {
  // treat task0 as the "intro task"
  // - poll for prio
  private int task_offset = 0;
  private readonly IList<IPetTask> tasks;

  public bool Interruptable {
    get => tasks.All((task) => task.Interruptable);
  }

  public ChainedPetTask() {
    tasks = [];
  }

  public void AddTask(IPetTask task) {
    tasks.Add(task);
    task_offset = tasks.Count;
  }

  public void BeginTask() {
    task_offset = 0;
    tasks[0].BeginTask();
  }

  private IPetTask GetCurrentTask() {
    return tasks[Math.Clamp(task_offset, 0, tasks.Count - 1)];
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    while (!Complete() && GetCurrentTask().Complete()) {
      task_offset++;
      if (!Complete()) {
        GetCurrentTask().BeginTask();
      }
    }

    if (!Complete()) {
      return GetCurrentTask().Update(prev_state, delta);
    }

    // just return a copy of the prev state
    return new SimplePetObject(prev_state);
  }

  public bool Complete() {
    return task_offset == tasks.Count;
  }

  public int GetPriority() {
    int max_prio = -1;
    foreach (IPetTask task in tasks) {
      max_prio = Math.Max(max_prio, task.GetPriority());
    }

    return max_prio;
  }
}