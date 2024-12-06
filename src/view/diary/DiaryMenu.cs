using digipet.component;
using digipet.diary.store;
using digipet.framework;
using digipet.view.text.diary;

namespace digipet.view.diary;

public class DiaryMenu : ViewComponent {
  private readonly SimpleTextMenuView menu;
  private readonly IDiaryRepo repo;
  private readonly IEngine engine;

  public DiaryMenu(IEngine engine, IDiaryRepo repo) : base() {
    this.engine = engine;
    this.repo = repo;

    menu = new(engine, canvas.font.FontType.SMALL) {
      Size = new(0.5f),
      Anchor = new(0.5f),
      Offset = new(0.5f),
      Closable = false
    };

    menu.AddItem("Create", OpenCreateMenu);
    menu.AddItem("View", OpenViewMenu);



    AddView(menu);
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    // how do we know that our menu has focus?
  }

  private void OnViewPop() {
    menu.Opacity = 1;
  }

  private void OpenCreateMenu(int index) {
    // add view
    // call some tx to open create menu
    menu.Opacity = 0;
    DiaryEditorWrap wrap = new(engine, repo);
    wrap.AddPopListener(OnViewPop);
    AddView(wrap);
  }

  private void OpenViewMenu(int index) {
    menu.Opacity = 0;
    DiaryList list = new(engine);

    DiaryBundle bundle = new();
    foreach (IDiaryRecord record in repo.GetRecords()) {
      bundle.AddEntry(record);
    }

    list.AddBundle("00", bundle);

    list.AddPopListener(OnViewPop);
    AddView(list);
  }
}