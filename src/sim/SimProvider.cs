using digipet.sim.edible;
using digipet.sim.impl;
using digipet.sim.water;

namespace digipet.sim;

#nullable enable

public class SimProvider {
  private ITasteModel? tasteModelSingleton;
  private IWaterSource? waterSourceSingleton;
  private IHungerModel? hungerModelSingleton;
  private IThirstModel? thirstModelSingleton;
  private IPetModel? petModelSingleton;

  public IWaterSource GetWaterSource() {
    // wire up to some save logic + provide to thirst model
    return waterSourceSingleton ??= new SimpleWaterDish(10.0, 4.2);
  }

  public ITasteModel GetTasteModel() {
    return tasteModelSingleton ??= new SimpleTasteModel();
  }

  public IHungerModel GetHungerModel() {
    return hungerModelSingleton ??= new SimpleHungerModel(0.2, GetTasteModel());
  }

  public IThirstModel GetThirstModel() {
    return thirstModelSingleton ??= new SimpleThirstModel(
      0.1,
      GetWaterSource()
    );
  }

  public IPetModel GetPetModel() {
    return petModelSingleton ??= new SimplePetModel(
      GetHungerModel(), GetThirstModel()
    );
  }
}