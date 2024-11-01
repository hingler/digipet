using digipet.input;

namespace digipet.component;

using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using digipet.canvas;
using digipet.transition;
using digipet.view;

public class ViewComponent : IDigiComponent, IContainer {
  private enum CoordMetric {
    Relative, Absolute
  };

  private struct CoordState {
    public CoordMetric OffsetX = CoordMetric.Relative;
    public CoordMetric OffsetY = CoordMetric.Relative;
    public CoordMetric SizeX = CoordMetric.Relative;
    public CoordMetric SizeY = CoordMetric.Relative;

    public CoordMetric Offset {
      set {
        OffsetX = value;
        OffsetY = value;
      }
    }

    public CoordMetric Size {
      set {
        SizeX = value;
        SizeY = value;
      }
    }

    public CoordState() {}
  }

  private Vector2 offset_;
  private Vector2 offset_px_;

  private Vector2 size_;
  private Vector2 size_px_;

  private CoordState coord_state = new();



  private Vector2 anchor_;

  // offset of anchor relative to parent
  public Vector2 Offset {
    get => offset_;
    set {
      offset_ = value;
      coord_state.Offset = CoordMetric.Relative;
    }
  }

  public Vector2 OffsetPx {
    get => offset_px_;
    set {
      offset_px_ = value;
      coord_state.Offset = CoordMetric.Absolute;
    }
  }

  public float X {
    get => offset_.X;
    set {
      offset_.X = value;
      coord_state.OffsetX = CoordMetric.Relative;
    }
  }

  public float Y {
    get => offset_.Y;
    set {
      offset_.Y = value;
      coord_state.OffsetY = CoordMetric.Relative;
    }
  }

  public float PixelX {
    get => offset_px_.X;
    set {
      offset_px_.X = value;
      coord_state.OffsetX = CoordMetric.Absolute;
    }
  }

  public float PixelY {
    get => offset_px_.Y;
    set {
      offset_px_.Y = value;
      coord_state.OffsetY = CoordMetric.Absolute;
    }
  }

  // size relative to parent
  public Vector2 Size {
    get => size_;
    set {
      size_ = value;
      coord_state.Size = CoordMetric.Relative;
    }
  }

  public Vector2 SizePx {
    get => size_px_;
    set {
      size_px_ = value;
      coord_state.Size = CoordMetric.Absolute;
    }
  }

  public float SizeX {
    get => size_.X;
    set {
      size_.X = value;
      coord_state.SizeX = CoordMetric.Relative;
    }
  }

  public float SizeY {
    get => size_.Y;
    set {
      size_.Y = value;
      coord_state.SizeY = CoordMetric.Relative;
    }
  }

  public float PixelSizeX {
    get => size_px_.X;
    set {
      size_px_.X = value;
      coord_state.SizeX = CoordMetric.Absolute;
    }
  }

  public float PixelSizeY {
    get => size_px_.Y;
    set {
      size_px_.Y = value;
      coord_state.SizeY = CoordMetric.Absolute;
    }
  }

  // position of anchor point, relative to this component (0.0 - 1.0)
  public Vector2 Anchor {
    get => anchor_;
    set {
      anchor_ = value;
    }
  }

  public float Opacity;

  public int ZIndex;

  private HashSet<ViewComponent> children = [];

  private TransitionQueue transitions = new();

  public ViewComponent() {
    _stack_child = null;
    activated = false;

    Offset = Vector2.Zero;
    Size = Vector2.One;
    Anchor = Vector2.Zero;
    Opacity = 1.0f;
    
    ZIndex = 0;
  }

  public virtual IReadOnlyList<ViewComponent> GetChildren() => [.. children];

  public virtual void AddView(ViewComponent v) {
    children.Add(v);
  }

  public virtual void RemoveView(ViewComponent v) {
    children.Remove(v);
  }

  public bool PreInput(InputType input, InputState state) {
    // container handles first
    IReadOnlyList<ViewComponent> children = GetChildren();
    // children handle first (top down)
    for (int i = children.Count - 1; i >= 0; i--) {
      ViewComponent c = children[i];
      if (c.PreInput(input, state)) {
        return true;
      }
    }

    // parent handles last
    if (HandleInput(input, state)) {
      return true;
    }


    return false;
  }

  public virtual bool HandleInput(InputType input, InputState state) => false;

  public void PreActivate() {
    Debug.Assert(!activated, "attempted to activate active component!");
    activated = true;
    Activate();
  }

  public void PreDeactivate() {
    Debug.Assert(activated, "attgempted to deactivate inactive component!");
    activated = false;
    Deactivate();
  }

  // implemented by user
  public virtual void Create() {}
  public virtual void Activate() {}

  public virtual void PreTick(double delta) {
    transitions.Tick(delta);
    Tick(delta);
    foreach (ViewComponent child in GetChildren()) {
      child.PreTick(delta);
      // check for child-disposes or self-disposes
      if (child.Dispose) {
        child.AcknowledgeDispose();
        RemoveView(child);
      } else {
        ViewComponent v = child.AcknowledgePush();
        if (v != null) {
          AddView(v);
        }
      }
    }


  }
  public virtual void Tick(double delta) {}
  public virtual void Deactivate() {}

  public void PreDraw(ICanvas canvas) {

    // not gonna worry about this anymore
    Vector2 offset_rel = offset_;
    Vector2 size_rel = size_;

    Vector2 offset_abs = canvas.PxToRelative(OffsetPx);
    Vector2 size_abs = canvas.PxToRelative(SizePx);

    Vector2 offset = new(
      coord_state.OffsetX == CoordMetric.Relative ? offset_rel.X : offset_abs.X,
      coord_state.OffsetY == CoordMetric.Relative ? offset_rel.Y : offset_abs.Y
    );

    Vector2 size = new(
      coord_state.SizeX == CoordMetric.Relative ? size_rel.X : size_abs.X,
      coord_state.SizeY == CoordMetric.Relative ? size_rel.Y : size_abs.Y
    );

    Vector2 start = offset - (size * Anchor);
    OffsetCanvas c = new(canvas, start, size, 1.0f, Opacity, ZIndex);
    Draw(c);

    IReadOnlyList<ViewComponent> children = GetChildren();
    for (int i = 0; i < children.Count; i++) {
      children[i].PreDraw(c);
    }
  }
  
  public virtual void Draw(ICanvas canvas) {}

  protected void PushComponent(ViewComponent component) {
    _stack_child = component;
  }

  // idea: optional "state" provided here
  // think some sort of "key:value" object would be "fine"
  // (could do the same thing on creation)
  protected void PopSelf() {
    _dispose = true;
  }
  
  public ViewComponent AcknowledgePush() {
    ViewComponent res = _stack_child;
    _stack_child = null;
    return res;
  }

  public bool AcknowledgeDispose() {
    bool res = _dispose;
    _dispose = false;
    return res;
  }

  // transition logic
  public void EnqueueTransition(ITransition transition) {
    transitions.Enqueue(transition);
  }

  public void AdvanceTransition() {
    transitions.Advance();
  }

  public bool TransitionsComplete() {
    return transitions.Complete();
  }

  private ViewComponent _stack_child;
  private bool _dispose;

  public bool Dispose {
    get => _dispose;
  }

  private bool activated;
}

// create a simple little test
// on press: pull up a dialogue box
// on press for the dialogue box: close it and go baxk
// count up the number of times it's been opened

// then1: start doing sim work
// then2: make a dummy "pet" display