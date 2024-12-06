// use spiral notebook bounds
// on highlight: surround with black box

// on select: open a new view which will contain a scrolling list of all elements

using System.Text.RegularExpressions;
using digipet.component;
using digipet.diary.store;
using digipet.framework;
using digipet.input;
using digipet.util;
using digipet.view.menu.grid;

namespace digipet.view.text.diary;

// scene should open up w a menu like "view / create"

public class DiaryList : ViewComponent {
  // how do we want to receive the list of diaries that are available?
  // a: let someone else do it :3

  private readonly IList<DiaryCover> covers = [];
  private readonly IList<DiaryBundle> bundles = [];

  private readonly GridMenu menu;

  private readonly IEngine engine;

  private int Cursor {
    get => menu.Cursor;
    set {
      menu.Cursor = Math.Clamp(value, 0, bundles.Count - 1);
    }
  }

  public DiaryList(IEngine engine) : base() {
    this.engine = engine;
    menu = new GridMenu();
    AddView(menu);
  }

  public override bool HandleInput(InputType input, InputState state) {
    if (base.HandleInput(input, state)) {
      return true;
    }

    if (state != InputState.RELEASE) {
      switch (input) {
        case InputType.LEFT:
          menu.CursorLeft();
          break;
        case InputType.RIGHT:
          menu.CursorRight();
          break;
        case InputType.UP:
          menu.CursorUp();
          break;
        case InputType.DOWN:
          menu.CursorDown();
          break;
        case InputType.CONFIRM:
          menu.Select();
          break;
        case InputType.BACK:
          PopSelf();
          break;
      }
    }

    return true;
  }

  public void AddBundle(string title, DiaryBundle bundle) {
    bundles.Add(bundle);
    DiaryCover cover;
    if (covers.Count < bundles.Count) {
      cover = new DiaryCover(engine);
      covers.Add(cover);
      menu.AddView(cover, () => OnSelected(bundle));
    } else {
      cover = covers[bundles.Count - 1];
      cover.Opacity = 1;
    }

    cover.Content = title;
  }

  private void OnSelected(DiaryBundle bundle) {
    // do something
    this.GetLogger().Log("selected item ", Cursor);
    AddView(new BundleViewer(engine, bundle) {
      ZIndex = 100
    });
  }

  public void ClearBundles() {
    bundles.Clear();
    foreach (DiaryCover cover in covers) {
      cover.Opacity = 0;
    }
  }

}