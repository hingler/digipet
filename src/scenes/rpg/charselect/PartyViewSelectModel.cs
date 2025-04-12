using digipet.rpg;
using digipet.rpg.picker;
using graphui.node;

namespace digipet.scenes.rpg.charselect;

public class PartyViewSelectModel(PartyView view) : ICharSelectModel {
  // true if we want to lock inputs to this view
  // else, false
  public bool Lock = false;
  public int GetSelectedIndex() => view.Target;
  public ICharData GetSelectedCharData() => view.GetSelectedChar();

  public ICharData SwapIn(ICharData source, int dest_index) {
    return view.Swap(dest_index, source);
  }

  public bool OnInput(Direction dir) {
    if (dir == Direction.LEFT) {
      view.Target--;
      return true;
    } else if (dir == Direction.RIGHT) {
      view.Target++;
      return true;
    }

    return Lock;
  }

  public void OnEnter() {
    view.Active = true;
  }

  public void OnExit() {
    view.Active = false;
  }

  public void OnSelect() {
    // do nothing here
  }
}