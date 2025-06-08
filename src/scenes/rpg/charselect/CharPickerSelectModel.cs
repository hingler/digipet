using digipet.rpg;
using digipet.view.rpg.picker;
using graphui.node;

namespace digipet.scenes.rpg.charselect;

public class CharPickerSelectModel(CharPickerMenu picker) : ICharSelectModel {
  public int GetSelectedIndex() => -1;
  public ICharData GetSelectedCharData() => picker.Select();

  // nop - exit accounts for it
  public void MarkSourceIndex() {}

  public bool OnInput(Direction dir) {
    if (dir == Direction.UP && picker.Index > 0) {
      picker.Decrement();
      return true;
    } else if (dir == Direction.DOWN && picker.Index < picker.Count) {
      picker.Increment();
      return true;
    }

    return false;
  }

  public void OnEnter() {
    picker.Active = true;
  }

  public void OnExit() {
    picker.Active = false;
  }

  public void OnSelect() {
    // uhh also nothing i think
  }
}