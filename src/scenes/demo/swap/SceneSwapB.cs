using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.input;
using digipet.util;
using digipet.view;
using digipet.view.bg;

namespace digipet.scenes.demo.swap;

public class SceneSwapB : Scene {

  public SceneSwapB(IEngine engine) : base(engine) {}
  public override void InitScene() {
    Text t = new() {
      Content = "B",
      Color = DigiColor.BLACK,
      Font = FontType.MID,
      Offset = new(0.5f),
      Alignment = HorizontalAlign.CENTER
    };

    PushToStack(new ColorRect(DigiColor.WHITE));
    PushToStack(t);
  }

  public override bool HandleInput(IKeyEvent @event) {
    base.HandleInput(@event);
    if (@event.Action == InputType.CONFIRM && @event.State == InputState.PRESS) {
      Finish();
    }


    return true;
  }
}