using System;
using System.Collections.Generic;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.transition.animator;
using digipet.transition.state;
using digipet.util;
using digipet.view.container;
using digipet.view.menu;

namespace digipet.view.pet;

public class QuickMenu : ViewComponent {
  private readonly BorderContainer container = new();
  private readonly TextMenu menu;
  public override IReadOnlyList<ViewComponent> GetChildren() {
    return [ container, ..base.GetChildren() ];
  }

  private readonly IEngine engine;

  public QuickMenu(IEngine engine) {
    this.engine = engine;
    menu = new(engine, canvas.font.FontType.TINY);
    for (int i = 0; i < 16; i++) {
      menu.AddItem(i.ToString(), (int g) => LoggerSingleton.GetLogger().Log("you fool"));
    }
    // fetching DB references?
    menu.AddItem("food!!!", (int i) => HandleFoodMenu());
    for (int i = 17; i < 32; i++) {
      menu.AddItem(i.ToString());
    }

    container.AddView(menu);
  }

  public override bool HandleInput(InputType input, InputState state) {
    base.HandleInput(input, state);

    if (state != InputState.RELEASE) {
      if (input == InputType.UP) {
        menu.DecrementSelector();
      } else if (input == InputType.DOWN) {
        menu.IncrementSelector();
      } else if (input == InputType.CONFIRM) {
        menu.ConfirmSelector();
      } else if (input == InputType.BACK) {
        PopSelf();
      }
    }

    return true;
  }


    private void HandleFoodMenu() {
    FoodMenu f = new(engine);
    f.ZIndex = -1;
    f.PixelSizeY = 96;
    // hierarchical is starting to make sense
    TransitionStateBuilder bb = new();
    bb.Animate(f, "X", 0.0f, 1.0f, EasingFunctions.EaseOutQuart)
      .WithDuration(0.4f);
    f.EnqueueTransition(bb.Build());

    AddView(f);
  }
}