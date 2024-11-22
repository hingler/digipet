using digipet.file.stream;
using digipet.user.activity;
using digipet.user.inventory;

namespace digipet.user;

public interface IUserData : IStreamable {
  long Coins { get; }
  long Exp { get; }
  string Username { get; }
  IUserInventory GetInventory();

  bool Charge(long coins);
}