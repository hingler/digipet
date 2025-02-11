using System;
using digipet.pet;
using digipet.sim;
using digipet.sim.water;
using digipet.util;

namespace digipet.world.pet.task.water;

public class DrinkTask : IPetTask {
  private readonly IPetModel model;
  private readonly IWaterSource source;


  private static readonly double THIRST_MAX = 0.9;
  private static readonly double THIRST_BEGIN = 0.55;
  private static readonly double THIRST_CRITICAL = 0.2;
  private static readonly double MIN_DRINK = 0.15;

  private static readonly double DRINK_RATE = 0.1;

  private double thirst_target;

  public bool Interruptable => false;

  private readonly SimpleTicker ticker;

  public DrinkTask(IPetModel model, IWaterSource source) {
    this.model = model;
    this.source = source;

    ticker = new(0.5f);

    thirst_target = 0.75;
  }

  public void BeginTask() {
    double target_delta = Math.Max(1.5f * (THIRST_BEGIN - model.Water), MIN_DRINK);
    thirst_target = Math.Min(model.Water + target_delta, THIRST_MAX);

    ticker.Reset();
  }

  public IPetState Update(IPetStateReadOnly prev_state, double delta) {
    SimplePetObject res = new(prev_state) {
      PetState = PetAction.IDLE,
      Emote = PetEmote.Sleep
    };;

    if (model.Water < thirst_target && ticker.Update(delta)) {
      model.Drink(DRINK_RATE * ticker.TickDelta, source);
    }

    return res;
  }

  public bool Complete() {
    return model.Water >= thirst_target || source.Contents < 0.0001;
  }

  public int GetPriority() {
    if (source.Contents < 0.01 || model.Water >= THIRST_BEGIN) {
      return -1;
    } else if (model.Water < THIRST_CRITICAL) {
      return 100;
    } else if (model.Water < THIRST_BEGIN) {
      return 2;
    }
    
    return -1;
  }
}