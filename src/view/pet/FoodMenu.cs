using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.view.container;
using digipet.view.menu;

namespace digipet.view.pet;

public class FoodMenu : ViewComponent {
  private readonly BorderContainer container = new();
  private readonly TextMenu menu;

  public override IReadOnlyList<ViewComponent> GetChildren() {
    return [ container ];
  }

  public FoodMenu(IEngine engine) {
    menu = new(engine, canvas.font.FontType.TINY);
    for (int i = 0; i < 16; i++) {
      menu.AddItem(i.ToString());
    }
  }

  public override bool HandleInput(InputType input, InputState state) {
    if (base.HandleInput(input, state)) {
      return true;
    }

    if (state == InputState.PRESS) {
      if (input == InputType.UP) {
        menu.DecrementSelector();
      } else if (input == InputType.DOWN) {
        menu.IncrementSelector();
      } else if (input == InputType.CONFIRM) {
        menu.ConfirmSelector();
        PopSelf();
      }
    }

    return true;
  }
}