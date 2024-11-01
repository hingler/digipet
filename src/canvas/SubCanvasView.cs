using System.Collections.Generic;
using System.Numerics;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.util;
using digipet.view;

namespace digipet.canvas;

public class SubCanvasView : ViewComponent, IContainer {
  private ISubCanvas dep;
  public SubCanvasView(IEngine engine) : this(engine.CreateSubCanvas()) {}
  public SubCanvasView(ISubCanvas canvas) {
    dep = canvas;
  }

  public Vector2 GetSubCanvasSize() {
    return dep.Size;
  }

  public override void Create() {
    base.Create();
    dep.GetRoot().Create();
  }

  public override void Activate() {
    dep.GetRoot().Activate();
  }

  public override void Deactivate() {
    dep.GetRoot().Deactivate();
  }

  public override void Tick(double delta) {
    dep.GetRoot().Tick(delta);
  }

  public override void Draw(ICanvas canvas) {
    // resize dep
    dep.Size = canvas.GetSizePx();
    ISprite sprite = dep.GetCanvasAsSprite();
    canvas.Tex(sprite, Vector2.Zero, Vector2.One, false);
  }

  public IReadOnlyList<ViewComponent> GetComponents() {
    return dep.GetChildren();
  }

  public void AddView(ViewComponent v) {
    dep.AddView(v);
  }

  public void RemoveView(ViewComponent v) {
    dep.RemoveView(v);
  }
}