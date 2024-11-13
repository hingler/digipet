using System.Xml;
using digipet.framework;
using digipet.pet;
using digipet.sim;
using digipet.util;

namespace digipet.world.pet.task.tasks;

public class RejectTask : IPetTask {
  public bool Interruptable => false;

  private static readonly int NOD_COUNT = 4;

  private int nod_count = 0;

  private readonly AnimationTicker ticker = new();

  private readonly IEngine engine;
  private readonly IPhysObject item;

  public RejectTask(
    IEngine engine,
    IPhysObject item
  ) {
    this.engine = engine;
    this.item = item;
  }

  public void BeginTask() {
    nod_count = 0;
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    if (ticker.Update(prev_state.Animation, delta)) {
      nod_count++;
      
      if (nod_count >= NOD_COUNT) {
        engine.GetPhysWorld().RemovePhysObject(item);
      }
    }


    SimplePetObject res = new(prev_state) {
      Emote = PetEmote.Sleep,
      PetState = PetAction.SHAKE_HEAD
    };

    return res;
  }

  public bool Complete() {
    return nod_count >= NOD_COUNT;
  }

  public int GetPriority() {
    return 2;
  }
}