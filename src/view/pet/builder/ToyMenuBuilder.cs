using digipet.framework;
using digipet.sim.db;
using digipet.sim.toy;
using digipet.user.inventory;
using digipet.util;

namespace digipet.view.pet.builder;

public class ToyMenuBuilder : IInventoryMenuBuilder {
  private readonly ISimRepo<IToyFactory> factories;
  private readonly IToyManager toy_manager;
  private readonly IUserInventory inventory;
  public ToyMenuBuilder(
    IEngine engine,
    IToyManager manager,
    IUserInventory inventory
  ) {
    factories = engine.GetAssetRepo<IToyFactory>();
    this.inventory = inventory;
    toy_manager = manager;
  }

  public void CreateItems(InventoryMenu menu) {
    foreach (IToyFactory factory in factories.GetEntries()) {
      IInventoryItem item = inventory.GetItem(factory.RID);
      // should always be true
      if (item.Quantity > -2) {
        menu.AddItem(
          factory.Name, 
          "x" + item.Quantity, 
          () => toy_manager.ToggleToy(factory.RID),
          toy_manager.IsSpawned(factory.RID) ? DigiColor.BLACK.WithOpacity(0.5) : DigiColor.BLACK
        );
      }
    }
  }
}