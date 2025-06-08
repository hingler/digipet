using digipet.pet;
using digipet.world.pet;
using digipet.world.pet.animation.handlers;

namespace digipet.diary.handlers;

public class DiaryIdleTask(double idle_duration = 10.0) : IPetTask, IDiaryMetaListener {
  public bool Interruptable => true;

  private double d_input = 0.0;

  public void BeginTask() {
    d_input = 0.0;
  }

  public void OnCharInput() {
    d_input = 0.0;
  }

  public void OnSaveBegin() {}
  public void OnSaveComplete() {}

  // tba:
  // snooze icon? (system for adding icons above pet)
  // save anim

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    d_input += delta;
    return new SimplePetObject(prev_state) {
      Facing = new System.Numerics.Vector2(-1.0f, 0.0f),
      PetState = PetAction.IDLE,
      Emote = PetEmote.Neutral
    };
  }

  public bool Complete() { return d_input > idle_duration; }
  public int GetPriority() => (int)Math.Ceiling(idle_duration - d_input);
}