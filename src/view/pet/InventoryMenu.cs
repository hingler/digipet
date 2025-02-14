using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.util;
using digipet.util.text;
using digipet.view.container;
using digipet.view.menu;

namespace digipet.view.pet;

#nullable enable

public class InventoryMenu : ViewComponent {
  private readonly BorderContainer container = new();
  private readonly List<Action> actions = [];
  private readonly List<Action> confirm_actions = [];
  private readonly ComponentMenu menu;

  public InventoryMenu(IEngine engine) {
    menu = new(engine);

    container.AddView(menu);
    AddView(container);
  }

  public InventoryMenu(IEngine engine, IInventoryMenuBuilder builder) : this(engine) {
    builder.CreateItems(this);
  }

  public void AddItem(string content, string datum, Action on_selected, DigiColor? color = null) {
    DigiColor col = color ?? DigiColor.BLACK;
    InventoryView item_view = new(FontType.TINY) {
      Name = content.Truncate(18),
      Datum = datum,
      PixelSizeY = 11.0f,
      TextColor = col
    };

    actions.Add(on_selected);
    menu.AddItem(item_view, OnSelect);
  }

  public override bool HandleInput(InputType input, InputState state) {
    if (base.HandleInput(input, state)) {
      return true;
    }

    if (state != InputState.RELEASE) {
      if (input == InputType.UP) {
        menu.DecrementSelector();
      } else if (input == InputType.DOWN) {
        menu.IncrementSelector();
      } else if (input == InputType.CONFIRM) {
        menu.ConfirmSelector();

        foreach (Action a in confirm_actions) {
          a();
        }

      } else if (input == InputType.BACK) {
        PopSelf();
      }
    }

    return true;
  }

  public void AddConfirmListener(Action action) {
    confirm_actions.Add(action);
  }

  public void OnSelect(int index) {
    actions[index].Invoke();
  }
}