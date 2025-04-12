using digipet.component;
using digipet.framework;
using digipet.rpg;
using digipet.rpg.chardata;
using digipet.view.menu;
using digipet.view.text.diary;

namespace digipet.view.rpg.picker;

public class CharPickerMenu : ViewComponent {
  private readonly ComponentMenu menu;
  private readonly List<ICharData> char_infos;
  public bool Active {
    get => menu.Active;
    set => menu.Active = value;
  }

  // three defaults
  public CharPickerMenu(
    IEngine engine
  ) : this(
    engine,
    [
      new SimpleCharData(engine),
      new SimpleCharData(engine),
      new SimpleCharData(engine),
      new SimpleCharData(engine),
      new SimpleCharData(engine),
      new SimpleCharData(engine),
    ]
  ) {}

  // fetch from enumerable, or fetch from repo?
  // (tba it)
  public CharPickerMenu(
    IEngine engine,
    IEnumerable<ICharData> infos
  ) {
    menu = new(engine) {
      Margin = 0.0f
    };

    char_infos = [];

    foreach (ICharData ci in infos) {
      char_infos.Add(ci);
      menu.AddItem(
        new CharMenuModal(ci) {
          SizeX = 1.0f,
          PixelSizeY = 48.0f
        }, null
      );
    }

    AddView(menu);
  }

  // event should be passed down
  public void Increment() => menu.IncrementSelector();
  public void Decrement() => menu.DecrementSelector();
  public int Index => menu.GetSelected();
  public int Count => menu.Count;
  public ICharData Select() => char_infos[menu.GetSelected()];
}