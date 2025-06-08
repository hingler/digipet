using digipet.file.stream;
using digipet.pet;
using digipet.sim.edible;
using digipet.sim.energy;
using digipet.sim.social;
using digipet.sim.toy;
using digipet.sim.water;
using digipet.util;

namespace digipet.sim.impl;

public class SimplePetModel : IPetModel {
  // food
  private readonly IHungerModel hungerModel;
  private readonly IThirstModel thirstModel;
  private readonly IFunModel funModel;
  private readonly ISocialModel socialModel;
  private readonly IEnergyModel energyModel;
  public double Food => hungerModel.Fullness;
  public double Water => thirstModel.Quenchiness;
  public double Fun => funModel.Fun;
  public double Social => socialModel.Social;
  public double Energy => energyModel.Energy;

  public long PetExp { get; set; }
  public string PetName { get; set; }

  public IPetPersonality Personality { get; set; }

  private readonly ILogger logger;

  public SimplePetModel(
    IHungerModel hungerModel,
    IThirstModel thirstModel,
    IFunModel funModel,
    ISocialModel socialModel,
    IEnergyModel energyModel
  ) : this(hungerModel, thirstModel, funModel, socialModel, energyModel, new PetDataParcel()) {}

  public SimplePetModel(
    IHungerModel hungerModel,
    IThirstModel thirstModel,
    IFunModel funModel,
    ISocialModel socialModel,
    IEnergyModel energyModel,
    IPetData petData
  ) {
    this.hungerModel = hungerModel;
    this.thirstModel = thirstModel;
    this.funModel = funModel;
    this.socialModel = socialModel;
    this.energyModel = energyModel;
    
    hungerModel.Fullness = petData.Food;
    thirstModel.Quenchiness = petData.Water;
    funModel.Fun = petData.Fun;
    socialModel.Social = petData.Social;
    energyModel.Energy = petData.Energy;

    PetExp = petData.PetExp;
    PetName = petData.PetName;

    Personality = petData.Personality;

    logger = this.GetLogger();
  }

  public void Tick(int tick_seconds) {
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