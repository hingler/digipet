using digidev.util;
using digipet.component;
using Godot;

namespace digipet.view;

public class Text : ViewComponent {
  public string Content;
  public Vector2 Origin;
  public float Scale = 1.0f;

  public Text() : base() {}

  public override void Draw(ICanvas canvas) {
    canvas.Text(Origin.ToSysVec(), Content, Scale);
  }
}