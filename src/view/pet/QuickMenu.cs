using System;
using System.Collections.Generic;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.sim;
using digipet.sim.water;
using digipet.transition.animator;
using digipet.transition.state;
using digipet.user;
using digipet.util;
using digipet.view.container;
using digipet.view.menu;
using digipet.view.water;

namespace digipet.view.pet;

public class QuickMenu : ViewComponent {
  private readonly BorderContainer container = new();
  private readonly TextMenu menu;
  public override IReadOnlyList<ViewComponent> GetChildren() {
    return [ container, ..base.GetChildren() ];
  }

  private readonly IEngine engine;
  private readonly SimProvider provider;
  private readonly IUserData userdata;

  public QuickMenu(
    IEngine engine,
    SimProvider provider,
    IUserData userdata
  ) {
    this.engine = engine;
    this.provider = provider;
    this.userdata = userdata;
    menu = new(engine, canvas.font.FontType.TINY);
    for (int i = 0; i < 16; i++) {
      menu.AddItem(i.ToString(), (int g) => LoggerSingleton.GetLogger().Log("you fool"));
    }
    // fetching DB references?
    menu.AddItem("food!!!", (int i) => HandleFoodMenu());
    menu.AddItem("water!!!", (int i) => HandleWaterMenu());
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
    FoodMenu f = new(engine, userdata)
    {
        ZIndex = -1,
        PixelSizeY = 96,
        SizeX = 2.0f
    };

    f.AddConfirmListener(PopSelf);

    // hierarchical is starting to make sense
    TransitionStateBuilder bb = new();
    bb.Animate(f, "X", 0.0f, 1.0f, EasingFunctions.EaseOutQuart)
      .WithDuration(0.4f);
    f.EnqueueTransition(bb.Build());

    AddView(f);
  }

  private void HandleWaterMenu() {
    WaterFill w = new(engine, provider.GetWaterSource()) {
      ZIndex = -1,
      PixelSizeY = 64
    };

    TransitionStateBuilder bb = new();
    bb.Animate(w, "X", 0.0f, 1.0f, EasingFunctions.EaseOutQuart)
      .WithDuration(0.4f);
    w.EnqueueTransition(bb.Build());

    AddView(w);
  }
}