namespace digipet.sim.toy;

#nullable enable

public interface IToyManager {

  // returns enumerable of toys which can be instantiated
  IEnumerable<IToyFactory> GetAvailableToys();

  IEnumerable<IToy> GetSpawnedToys();

  bool IsSpawned(int rid);

  void Tick(double delta);

  // given an RID, instantiates a toy
  // returns null if associated toy factory DNE
  // should we remove the old toy?
  BaseToyHandler? CreateToy(int toy_rid);

  bool DestroyToy(int toy_rid);

  void ToggleToy(int toy_rid);
}