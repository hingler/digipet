using System.Numerics;
using digipet.framework;
using digipet.pet;
using digipet.world.pet;

namespace digipet.diary.handlers.save;

// as long as: 
public class DiarySavingTask : IPetTask, IDiaryMetaListener {
  private bool save_start_flag = false;
  private bool save_end_flag = false;

  public bool Interruptable => false;

  public void BeginTask() {}

  public void OnCharInput() {}
  public void OnSaveBegin() { 
    save_start_flag = true; 
    save_end_flag = false;
  }
  public void OnSaveComplete() { save_end_flag = true; }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    return new SimplePetObject(prev_state) {
      Facing = -Vector2.UnitX,
      PetState = PetAction.SLEEP,
      Emote = PetEmote.Sleep
    };
  }

  public bool Complete() => save_start_flag && save_end_flag;
  public int GetPriority() => (save_start_flag && !save_end_flag) ? 100 : -1;
}