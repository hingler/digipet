using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.world.pet;

namespace digipet.diary.handlers.save;

public class DiarySaveTask : IPetTask, IDiaryMetaListener {
  private readonly DiarySavingTask saving;
  private readonly DiaryStarTask star;

  private readonly ChainedPetTask del_task;

  public DiarySaveTask(
    IEngine engine,
    Vector2 star_origin,
    ViewComponent root
  ) {
    saving = new();
    star = new(engine, star_origin, root);

    ChainedPetTask task = new();
    task.AddTask(saving);
    task.AddTask(star);

    del_task = task;
  }

  public void OnCharInput() => saving.OnCharInput();
  public void OnSaveBegin() => saving.OnSaveBegin();
  public void OnSaveComplete() => saving.OnSaveComplete();

  public void BeginTask() => del_task.BeginTask();
  public IPetState Update(IPetStateReadOnly prev_state, double delta) => del_task.Update(prev_state, delta);
  public bool Complete() => del_task.Complete();
  public int GetPriority() => del_task.GetPriority();
  public bool Interruptable => false;
}