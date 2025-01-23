using digipet.db;
using digipet.sim.edible;
using digipet.sim.impl;
using digipet.sim.water;
using digipet.util;

namespace digipet.sim;

#nullable enable

public class SimProvider {
  private ITasteModel? tasteModelSingleton;
  private IWaterSource? waterSourceSingleton;
  private IHungerModel? hungerModelSingleton;
  private IThirstModel? thirstModelSingleton;
  private IPetModel? petModelSingleton;
  private readonly IDataStore? simData;

  private static readonly string WATER_KEY = "waterdata";
  private static readonly string PET_KEY = "petdata";

  public SimProvider() : this(null) {}
  public SimProvider(IDataStore? dataProvider) {
    simData = dataProvider?.GetSubspace("sim") ?? null;
  }

  public IWaterSource GetWaterSource() {
    // wire up to some save logic + provide to thirst model
    if (waterSourceSingleton == null) {
      IWaterData waterData = simData?.Fetch<IWaterData>(WATER_KEY) ?? new WaterData(10.0, 4.2);
      waterSourceSingleton = new SimpleWaterDish(waterData.Capacity, waterData.Contents);
    }

    return waterSourceSingleton;
  }

  public ITasteModel GetTasteModel() {
    return tasteModelSingleton ??= new SimpleTasteModel(simData);
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
    if (petModelSingleton == null) {
      IPetData data = simData?.Fetch<IPetData>(PET_KEY) ?? new PetDataParcel();
      petModelSingleton = new SimplePetModel(
        GetHungerModel(),
        GetThirstModel(),
        data
      );
    }
    
    return petModelSingleton;
  }

  public void SaveSimState() {
    this.GetLogger().Log("saving sim state!!!");
    simData?.Store(WATER_KEY, GetWaterSource().AsWaterData());
    simData?.Store(PET_KEY, new PetDataParcel(petModelSingleton));

  }
}