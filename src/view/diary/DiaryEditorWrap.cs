using digipet.canvas.font;
using digipet.component;
using digipet.diary.store;
using digipet.framework;
using digipet.input;
using digipet.util;
using digipet.view.bg;
using digipet.view.text;

namespace digipet.view.diary;

public class DiaryEditorWrap : ViewComponent {
  private readonly SimpleTextMenuView file_menu;
  private readonly DiaryEditor editor;
  private readonly RepoEditor repo_editor;

  public DiaryEditorWrap(IEngine engine, IDiaryRepo repo) {
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

    AddView(new ColorRect(DigiColor.WHITE));

    AddView(editor);

    file_menu.AddItem("Save", OnSave);
    file_menu.AddItem("Delete", OnDelete);
  }
  public override bool HandleInput(IKeyEvent @event) {
    if (@event.Action == InputType.BACK) {
      AddView(file_menu);
      return true;
    }

    return false;
  }

  public void OnSave(int index) {
    repo_editor.Save();
    PopSelf();
  }

  public void OnDelete(int index) {
    repo_editor.Erase();
    PopSelf();
  }
}