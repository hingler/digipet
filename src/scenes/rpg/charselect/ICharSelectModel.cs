using digipet.rpg;
using graphui.node;

namespace digipet.scenes.rpg.charselect;


public interface ICharSelectModel : IInputReceiver {
  // return the index
  int GetSelectedIndex();
  // not pet data
  ICharData GetSelectedCharData();

  // swap only needs to be implemented by the party view
  // (tba: char data needs an index)
}