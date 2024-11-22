using digipet.file.stream;
using digipet.user.inventory;

namespace digipet.user.stub;

public class BasicInventoryUserData : IUserData {
  public long Coins { get; } = 9999;
  public long Exp { get; } = 125;
  public string Username { get; } = "hingler";

  private readonly SimpleUserInventory inventory;

  public BasicInventoryUserData() {
    inventory = new SimpleUserInventory();
  }

  public IUserInventory GetInventory() {
    return inventory;
  }

  public bool Charge(long coins) {
    return Coins > coins;
  }

  public void ToStream(IOutputStream stream) {
    stream.WriteInt64(Coins);
    stream.WriteInt64(Exp);
    stream.WritePascalString(Username);

    inventory.ToStream(stream);
  }

  public BasicInventoryUserData(IInputStream stream) {
    Coins = stream.ReadInt64();
    Exp = stream.ReadInt64();
    Username = stream.ReadPascalString();

    inventory = new SimpleUserInventory(stream);
  }
}