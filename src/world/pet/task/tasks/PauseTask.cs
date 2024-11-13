using digipet.util;

namespace digipet.world.pet.task.tasks;

public class PauseTask : IPetTask {

  private double dt = 0.0;
  private readonly double duration;
  private readonly ILogger logger;

  public bool Interruptable { get; }

  public PauseTask(double duration) : this(duration, false) {}

  public PauseTask(double duration, bool interruptable) {
    this.duration = duration;
    logger = this.GetLogger();
    Interruptable = interruptable;
  }

  public void BeginTask() {
    dt = 0.0;
    logger.Log("starting pause task...");
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    dt += delta;

    // what now?

    SimplePetObject res = new(prev_state) {
      PetState = PetAction.PAUSE,
      Emote = digipet.pet.PetEmote.Neutral
    };

    return res;
  }

  public bool Complete() {
    return dt > duration;
  }

  public int GetPriority() {
    return 2;
  }
}