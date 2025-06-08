using digipet.canvas.font;
using digipet.component;
using digipet.diary.store;
using digipet.framework;
using digipet.input;
using digipet.sim;
using digipet.sim.social;
using digipet.transition;
using digipet.util;
using digipet.view.bg;
using digipet.view.text;

namespace digipet.view.diary;

public class DiaryEditorWrap : ViewComponent {
  private readonly SimpleTextMenuView file_menu;
  private readonly DiaryEditor editor;
  private readonly RepoEditor repo_editor;
  private readonly SimProvider provider;
  private static readonly ILogger logger = LoggerSingleton.GetStaticLogger<DiaryEditorWrap>();

  public DiaryEditorWrap(
    IEngine engine, 
    IDiaryRepo repo,
    SimProvider provider
  ) {
    repo_editor = new RepoEditor(repo);

    editor = new(engine, repo_editor) {
      MarginPx = 8
    };

    file_menu = new(engine, FontType.SMALL) {
      ZIndex = 100,
      Size = new(0.5f, 0.35f),
      Anchor = new(0.5f),
      Offset = new(0.5f)
    };

    this.provider = provider;

    AddView(new ColorRect(DigiColor.WHITE));

    AddView(editor);

    // obliterate on save
    file_menu.AddItem("Save", OnSave);
    file_menu.AddItem("Delete", OnDelete);
  }
  public override bool HandleInput(IKeyEvent @event) {
    if (!TransitionsComplete()) {
      // swallow events during transitions - this works for saving too!!!
      return true;
    }
    
    if (@event.Action == InputType.BACK) {
      AddView(file_menu);
      return true;
    }

    return false;
  }

  public void OnSave(int index) {
    editor.OnSaveBegin();
    repo_editor.Save();

    // put it here
    file_menu.Closable = false;
    // works :)
    // better soln would be to delegate input to this wrap instead
    editor.Active = false;


    IDiaryScoreModel diary_model = provider.GetDiaryScoreModel();
    double score = diary_model.Compute(editor.Editor);

    logger.Log("saved diary with score: ", score);

    TransitionBuilder bb = new();
    bb.Pause(1.0);
    bb.ThenCall(() => editor.OnSaveComplete());
    bb.ThenPause(1.0);
    bb.ThenCall(PopSelf);

    // feedback into pet state

    EnqueueTransition(bb.Build());
  }

  public void OnDelete(int index) {
    repo_editor.Erase();
    PopSelf();
  }
}