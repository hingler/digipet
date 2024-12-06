using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.view.container;
using digipet.view.menu;

#nullable enable

// extends textmenu to provide simple input handling functionality
// also does some simple container shit
public class SimpleTextMenuView : ViewComponent {

  private TextMenu base_menu;

  public bool Closable = true;
  public SimpleTextMenuView(IEngine engine, FontType font) : base() {
    base_menu = new(engine, font);
    BorderContainer container = new();
    MarginContainer margin = new() {
      MarginPx = 8
    };

    margin.AddView(base_menu);
    container.AddView(margin);
    AddView(container);
  }

  public void AddItem(string name, Action<int>? callback = null) {
    base_menu.AddItem(name, callback);
  }

  public override bool HandleInput(InputType input, InputState state) {
    if (state != InputState.RELEASE) {
      if (input == InputType.UP) {
        base_menu.DecrementSelector();
      } else if (input == InputType.DOWN) {
        base_menu.IncrementSelector();
      } else if (input == InputType.CONFIRM) {
        base_menu.ConfirmSelector();
      } else if (input == InputType.BACK) {
        if (Closable) {
          PopSelf();
        } else {
          // allow close to bubble up - alt, fire an event on close
          return false;
        }
      }

    }

    // consume the event
    return true;
  }
}