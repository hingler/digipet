using digipet.component;
using digipet.framework;
using digipet.view.text;

namespace digipet.scenes.demo;

public class DiaryEditorDemo : Scene {
  private readonly DiaryEditor editor;

  public DiaryEditorDemo(IEngine engine) : base(engine) {
    editor = new(engine) {
      MarginPx = 8,
      Font = canvas.font.FontType.TINY,
      Offset = new(0.1f),
      Size = new(0.8f)
    };
  }

  public override void InitScene() {
    PushToStack(editor);
  }
}