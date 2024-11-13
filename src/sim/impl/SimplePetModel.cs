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
  public double Fun => 0.5;
  public double Social => 0.5;
  public double Energy => 0.5;

  private readonly ILogger logger;


  public SimplePetModel(
    IHungerModel hungerModel,
    IThirstModel thirstModel
  ) {
    this.hungerModel = hungerModel;
    this.thirstModel = thirstModel;

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

  public long PetExp => 25;
  public string PetName => "Dingus";
}