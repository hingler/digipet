// need a sleep animation

using System.Numerics;
using digipet.pet;
using digipet.world.pet;

namespace digipet.diary.handlers;

public class DiarySleepTask : IPetTask, IDiaryMetaListener {
  public bool Interruptable => true;
  
  private bool awoken = false;
  public void BeginTask() { awoken = false; }

  public void OnCharInput() {
    // awaken once we detect input
    awoken = true;
  }
  public void OnSaveBegin() {}
  public void OnSaveComplete() {}


  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    // animation handler for this
    return new SimplePetObject(prev_state) {
      Facing = -Vector2.UnitX,
      PetState = PetAction.SLEEP,
      Emote = PetEmote.Sleep
    };
  }

  public bool Complete() { return awoken; }
  public int GetPriority() => 1;
}