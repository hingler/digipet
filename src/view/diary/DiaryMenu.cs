using digipet.component;
using digipet.diary.store;
using digipet.framework;
using digipet.transition;
using digipet.transition.animator;
using digipet.transition.state;
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
    menu.X = 0.5f;
  }

  private void OpenCreateMenu(int index) {
    // add view
    // call some tx to open create menu
    DiaryEditorWrap wrap = new(engine, repo);

    TransitionBuilder b = new();
    TransitionStateBuilder bb = new();
    AddMenuTransition(bb);
    AddAnimateViewOn(bb, wrap);
    b.AddState(bb.Build());


    wrap.EnqueueTransition(b.Build());

    wrap.AddPopListener(OnViewPop);
    AddView(wrap);
  }

  private void OpenViewMenu(int index) {
    DiaryList list = new(engine) {
      X = -1.0f
    };

    DiaryBundle bundle = new();
    foreach (IDiaryRecord record in repo.GetRecords()) {
      bundle.AddEntry(record);
    }

    list.AddBundle("00", bundle);

    TransitionBuilder b = new();
    TransitionStateBuilder bb = new();
    AddMenuTransition(bb);
    AddAnimateViewOn(bb, list);
    b.AddState(bb.Build());

    list.EnqueueTransition(b.Build());

    list.AddPopListener(OnViewPop);
    AddView(list);
  }

  private TransitionStateBuilder AddMenuTransition(TransitionStateBuilder bb) {
    bb.Animate(menu, "X", 0.5f, 1.5f, EasingFunctions.EaseInOutQuart).WithDuration(0.45f);
    return bb;
  }

  private TransitionStateBuilder AddAnimateViewOn(TransitionStateBuilder bb, ViewComponent viewer_view) {
    bb.Animate(viewer_view, "X", -1.0f, 0.0f, EasingFunctions.EaseInOutQuart).WithDuration(0.45f);
    return bb;
  }
}