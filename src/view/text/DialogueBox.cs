using digipet.canvas;
using digipet.component;
using digipet.framework;
using digipet.view.text;

namespace digipet.transition.text;


// tba: this code gets a bit sloppy - might want to revise down the line
public class DialogueBox : ViewComponent {
  private readonly FlowText text;
  private readonly DialogueTransition transition;
  private readonly SubCanvasView subcanvas;

  public string Content {
    get => transition.Content;
    set => transition.Content = value;
  }

  public bool Complete => transition.Complete();

  public DialogueBox(IEngine engine) {
    transition = new(engine);
    text = new(transition.GetAnimator());
    subcanvas = new(engine);

    subcanvas.AddView(text);
    AddView(subcanvas);
  }

  public void Advance() {
    transition.Advance();
  }

  public override void Tick(double delta) {
    base.Tick(delta);
    transition.Tick(delta);
    text.DisplayedLineOffset = transition.LineOffset;
  }
}