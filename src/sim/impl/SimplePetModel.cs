using digipet.file.stream;
using digipet.pet;
using digipet.sim.edible;
using digipet.sim.water;
using digipet.util;

namespace digipet.sim.impl;

public class SimplePetModel : IPetModel {
  // food
  private readonly IHungerModel hungerModel;
  private readonly IThirstModel thirstModel;
  public double Food => hungerModel.Fullness;
  public double Water => thirstModel.Quenchiness;
  public double Fun { get; set; }
  public double Social { get; set; }
  public double Energy { get; set; }

  public long PetExp { get; set; }
  public string PetName { get; set; }

  public IPetPersonality Personality { get; set; }

  private readonly ILogger logger;

  public SimplePetModel(
    IHungerModel hungerModel,
    IThirstModel thirstModel
  ) : this(hungerModel, thirstModel, new PetDataParcel()) {}

  public SimplePetModel(
    IHungerModel hungerModel,
    IThirstModel thirstModel,
    IPetData petData
  ) {
    this.hungerModel = hungerModel;
    this.thirstModel = thirstModel;
    
    hungerModel.Fullness = petData.Food;
    thirstModel.Quenchiness = petData.Water;

    Fun = petData.Fun;
    Social = petData.Social;
    Energy = petData.Energy;
    PetExp = petData.PetExp;
    PetName = petData.PetName;

    Personality = petData.Personality;

    logger = this.GetLogger();
  }

  public bool CanEat(IEdiblePickup pickup) {
    return hungerModel.CanEat(pickup);
  }

  public bool TryEat(IEdiblePickup food_item, out double score) {
    if (hungerModel.Eat(food_item, out score)) {
      logger.Log("ate food ", food_item.Name, " w score ", score);
      return true;
    }

    return false;
  }

  public double Drink(double units, IWaterSource source) {
    logger.Log("drinking ", units, " units...");
    return thirstModel.Drink(units, source);
  } 
}