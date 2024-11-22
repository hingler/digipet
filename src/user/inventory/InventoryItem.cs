using digipet.file.stream;
using digipet.sim;

namespace digipet.user.inventory;

// issue: serial/deserial for this is iffy

public interface IInventoryItem {
  public int RID { get; }
  public int Quantity { get; }
}
public class InventoryItemImpl : IInventoryItem, IStreamable {
  private int quantity_;
  public int Quantity { get => quantity_; }
  public int RID { get; }

  public InventoryItemImpl(int RID, int init_quantity) {
    quantity_ = init_quantity;
    this.RID = RID;
  }

  public void Increment() => Increment(1);

  public void Increment(int num) {
    if (num > 0) {
      quantity_ += num;
    }
  }

  public int Decrement() => Decrement(1);
  public int Decrement(int num) {
    int dec = Math.Min(quantity_, num);
    if (dec > 0) {
      quantity_ -= dec;
    }

    return dec;
  }

  public InventoryItemImpl(IInputStream stream) : this(
    stream.ReadInt32(),
    stream.ReadInt32()
  ) {}

  public void ToStream(IOutputStream stream) {
    stream.WriteInt32(quantity_);
    stream.WriteInt32(RID);
  }
}