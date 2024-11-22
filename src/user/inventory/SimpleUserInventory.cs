using digipet.file.stream;
using digipet.sim;
using digipet.util;

namespace digipet.user.inventory;

public class SimpleUserInventory : IUserInventory {
  private readonly Dictionary<int, InventoryItemImpl> items;
  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<SimpleUserInventory>();

  public SimpleUserInventory() {
    items = [];
  }

  private static void LogItem(IInventoryItem item) {
    logger.Log("qty of rid ", item.RID, ": ", item.Quantity);
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteInt32(items.Count);
    foreach (KeyValuePair<int, InventoryItemImpl> item in items) {
      stream.WriteInt32(item.Value.RID);
      stream.WriteInt32(item.Value.Quantity);
    }
  }

  public SimpleUserInventory(IInputStream stream) : this() {
    int count = stream.ReadInt32();
    for (int i = 0; i < count; i++) {
      int RID = stream.ReadInt32();
      InventoryItemImpl item = new(RID, stream.ReadInt32());
      items.Add(RID, item);

      LogItem(item);
    }
  }

  public IEnumerable<IInventoryItem> GetItems() {
    return items.Values;
  }

  public IInventoryItem GetItem(int RID) {
    return items.GetValueOrDefault(RID, new InventoryItemImpl(RID, 0));
  }

  public void AddToInventory(int RID) {
    if (!items.TryGetValue(RID, out InventoryItemImpl item)) {
      item = new InventoryItemImpl(RID, 0);
      items[RID] = item;
    }

    item.Increment();
    LogItem(item);


  }

  public bool RemoveFromInventory(int RID) {
    if (items.TryGetValue(RID, out InventoryItemImpl value)) {
      value.Decrement();
      LogItem(value);
      if (value.Quantity <= 0) {
        items.Remove(RID);
      }


      return true;
    }

    return false;
  }
}