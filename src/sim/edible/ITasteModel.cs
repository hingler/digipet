namespace digipet.sim.edible;

public interface ITasteModel {
  // return quality of food
  public double GetTaste(IEdiblePickup pickup);
}