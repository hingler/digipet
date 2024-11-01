using System.Collections.Generic;
using digipet.component;
using digipet.framework;
using digipet.view.container;
using digipet.view.menu;

namespace digipet.view.pet;

public class QuickMenu : ViewComponent {
  private readonly BorderContainer container = new();
  private readonly TextMenu menu;
  public override IReadOnlyList<ViewComponent> GetChildren() {
    return [ container ];
  }

  private readonly IEngine engine;

  public QuickMenu(IEngine engine) {
    this.engine = engine;
    menu = new(engine, canvas.font.FontType.TINY);
    for (int i = 0; i < 16; i++) {
      menu.AddItem(i.ToString());
    }
    // fetching DB references?
    menu.AddItem("this one is food", (int i) => HandleFoodMenu());
    for (int i = 17; i < 32; i++) {
      menu.AddItem(i.ToString());
    }

    container.AddView(menu);
  }



  private void HandleFoodMenu() {
    FoodMenu f = new(engine);
    f.ZIndex = ZIndex - 1;
    // hierarchical is starting to make sense
    PushComponent(f);
  }
}