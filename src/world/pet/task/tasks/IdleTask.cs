using digipet.pet;
using digipet.util;

namespace digipet.world.pet.task.tasks;

public class IdleTask : IPetTask {
  private readonly double duration;
  private readonly PetEmote emote;
  private double dt;
  private AnimationTicker ticker;

  public bool Interruptable { get; }

  // tba: override with current emote state
  public IdleTask() : this(-1.0, PetEmote.Neutral) {}

  public IdleTask(
    double max_duration,
    PetEmote emote
  ) {
    duration = max_duration;
    this.emote = emote;
    ticker = new();

    Interruptable = duration < 0.0;
  }
  public void BeginTask() {
    dt = 0.0;
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    if (ticker.Update(prev_state.Animation, delta)) {
      dt += prev_state.Animation.FrameDuration;
    }
  
    SimplePetObject res = new(prev_state) {
      PetState = PetAction.IDLE,
      Emote = emote
    };

    return res;
  }

  public bool Complete() {
    return (duration >= 0.0) && (dt >= duration);
  }

  public int GetPriority() {
    return 1;
  }
}