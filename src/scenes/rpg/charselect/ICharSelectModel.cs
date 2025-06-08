using digipet.rpg;
using graphui.node;

namespace digipet.scenes.rpg.charselect;


public interface ICharSelectModel : IInputReceiver {
  // return the index associated with source location (only for party view)
  int GetSelectedIndex();
  ICharData GetSelectedCharData();

  // called to mark source index
  void MarkSourceIndex();

  // swap only needs to be implemented by the party view
  // (tba: char data needs an index)
}