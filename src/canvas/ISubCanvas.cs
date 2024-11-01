using System.Collections.Generic;
using System.Numerics;
using digipet.component;
using digipet.image;
using digipet.view;

namespace digipet.canvas;

// canvas-like interface but with a texture field which can be sampled

public interface ISubCanvas : ICanvas, IContainer {
  public Vector2 Size { get; set; }
  ISprite GetCanvasAsSprite();
  IDigiComponent GetRoot();
}