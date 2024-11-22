using digipet.file.stream;
using digipet.sim;

namespace digipet.user.inventory;

public interface IUserInventory : IStreamable {
  // fetch based on WorldItem, or based on something else??
  // (ex. some inventory items arent world items)

  // food
  // "fixtures"
  // things which aren't necessarily "world items??"

  // nvm - use InventoryItem instead of IWorldItem
  // alt: rig so that IUserInventory can return world items given some RID?
  // (ie: inventory items map trivially to world items - fetch when requested)

  IEnumerable<IInventoryItem> GetItems();
  IInventoryItem GetItem(int RID);
  void AddToInventory(int RID);

  // false if item not in inventory
  bool RemoveFromInventory(int RID);
}