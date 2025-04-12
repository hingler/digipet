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

  private bool reflow_dirty_ = true;
  private bool resize_dirty_ = true;

  private Vector2 anchor_;

  // offset of anchor relative to parent
  public Vector2 Offset {
    get => offset_;
    set {
      offset_ = value;
      coord_state.Offset = CoordMetric.Relative;
      resize_dirty_ = true;
    }
  }

  public Vector2 OffsetPx {
    get => offset_px_;
    set {
      offset_px_ = value;
      coord_state.Offset = CoordMetric.Absolute;
      resize_dirty_ = true;
    }
  }

  public float X {
    get => offset_.X;
    set {
      offset_.X = value;
      coord_state.OffsetX = CoordMetric.Relative;
      resize_dirty_ = true;
    }
  }

  public float Y {
    get => offset_.Y;
    set {
      offset_.Y = value;
      coord_state.OffsetY = CoordMetric.Relative;
      resize_dirty_ = true;
    }
  }

  public float PixelX {
    get => offset_px_.X;
    set {
      offset_px_.X = value;
      coord_state.OffsetX = CoordMetric.Absolute;
      resize_dirty_ = true;
    }
  }

  public float PixelY {
    get => offset_px_.Y;
    set {
      offset_px_.Y = value;
      coord_state.OffsetY = CoordMetric.Absolute;
      resize_dirty_ = true;
    }
  }

  // size relative to parent
  public Vector2 Size {
    get => size_;
    set {
      size_ = value;
      coord_state.Size = CoordMetric.Relative;
      resize_dirty_ = true;
      reflow_dirty_ = true;
    }
  }

  public Vector2 SizePx {
    get => size_px_;
    set {
      size_px_ = value;
      coord_state.Size = CoordMetric.Absolute;
      resize_dirty_ = true;
      reflow_dirty_ = true;
    }
  }

  public float SizeX {
    get => size_.X;
    set {
      size_.X = value;
      coord_state.SizeX = CoordMetric.Relative;
      resize_dirty_ = true;
      reflow_dirty_ = true;
    }
  }

  public float SizeY {
    get => size_.Y;
    set {
      size_.Y = value;
      coord_state.SizeY = CoordMetric.Relative;
      resize_dirty_ = true;
      reflow_dirty_ = true;
    }
  }

  public float PixelSizeX {
    get => size_px_.X;
    set {
      size_px_.X = value;
      coord_state.SizeX = CoordMetric.Absolute;
      resize_dirty_ = true;
      reflow_dirty_ = true;
    }
  }

  public float PixelSizeY {
    get => size_px_.Y;
    set {
      size_px_.Y = value;
      coord_state.SizeY = CoordMetric.Absolute;
      resize_dirty_ = true;
      reflow_dirty_ = true;
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

  public float ZIndex;

  private readonly IList<ViewComponent> children = [];

  private readonly TransitionQueue transitions = new();

  private readonly HashSet<Action> pop_listeners = new();

  public ViewComponent() {
    activated = false;

    Offset = Vector2.Zero;
    Size = Vector2.One;
    Anchor = Vector2.Zero;
    Opacity = 1.0f;
    
    ZIndex = 0;
  }

  public virtual IReadOnlyList<ViewComponent> GetChildren() => children.AsReadOnly();

  public virtual void AddView(ViewComponent v) {
    children.Add(v);
  }

  public virtual void RemoveView(ViewComponent v) {
    children.Remove(v);
  }

  public void AddPopListener(Action a) {
    pop_listeners.Add(a);
  }

  public bool PreInput(IKeyEvent @event) {
    // children handle first (top down)
    for (int i = children.Count - 1; i >= 0; i--) {
      ViewComponent c = children[i];
      if (c.PreInput(@event)) {
        return true;
      }
    }

    // parent handles last
    if (HandleInput(@event) || HandleInput(@event.Action, @event.State)) {
      return true;
    }


    return false;
  }

  public virtual bool HandleInput(IKeyEvent @event) => false;
  public virtual bool HandleInput(InputType input, InputState state) => false;

  public void PreActivate() {
    if (!activated) {
      activated = true;
      Activate();
    }
  }

  public void PreDeactivate() {
    if (activated) {
      activated = false;
      Deactivate();
    }
  }

  // implemented by user
  public virtual void Create() {}
  public virtual void Activate() {}

  public void PreTick(double delta) {
    transitions.Tick(delta);
    Tick(delta);
    IReadOnlyList<ViewComponent> children = [..GetChildren()];
    foreach (ViewComponent child in children) {
      child.PreTick(delta);
      // check for child-disposes or self-disposes
      if (child.Dispose) {
        child.AcknowledgeDispose();
        RemoveView(child);
      }
    }
  }

  public void PrePhysicsTick(double delta) {
    PhysicsTick(delta);
    IReadOnlyList<ViewComponent> children = [..GetChildren()];

    foreach (ViewComponent child in children) {
      child.PrePhysicsTick(delta);
      // skip dispose pass for physics
    }
  }


  public virtual void Tick(double delta) {}
  public virtual void PhysicsTick(double delta) {}
  public virtual void Deactivate() {}

  public void PreDestroy() {
    foreach (ViewComponent c in children) {
      c.PreDestroy();
    }

    Destroy();
  }
  public virtual void Destroy() {}

  protected void QueueReflow() {
    reflow_dirty_ = true;
  }

  public void ResizePass(ICanvas canvas) {
    if (resize_dirty_) {
      HandleResize(canvas);
      resize_dirty_ = false;
    }

    IReadOnlyList<ViewComponent> children = GetChildren();
    for (int i = 0; i < children.Count; i++) {
      children[i].ResizePass(canvas);
    }
  }

  public void PreDraw(ICanvas canvas) {
    // don't like this
    HandleResize(canvas);

    Vector2 start = offset_ - (size_ * Anchor);
    OffsetCanvas c = new(canvas, start, size_, 1.0f, Opacity, ZIndex);

    if (resize_dirty_) {
      HandleResize(canvas);
      resize_dirty_ = false;
    }

    if (reflow_dirty_) {
      // draws before children reflow??
      Reflow(c);
      reflow_dirty_ = false;
    }

    Draw(c);

    IReadOnlyList<ViewComponent> children = GetChildren();
    for (int i = 0; i < children.Count; i++) {
      children[i].PreDraw(c);
    }
  }

  protected void HandleResize(ICanvas canvas) {
    // not gonna worry about this anymore
    Vector2 offset_rel = offset_;
    Vector2 size_rel = size_;

    Vector2 offset_abs = canvas.PxToRelative(OffsetPx);
    Vector2 size_abs = canvas.PxToRelative(SizePx);

    // both specified in rel coords

    Vector2 offset = new(
      coord_state.OffsetX == CoordMetric.Relative ? offset_rel.X : offset_abs.X,
      coord_state.OffsetY == CoordMetric.Relative ? offset_rel.Y : offset_abs.Y
    );

    Vector2 size = new(
      coord_state.SizeX == CoordMetric.Relative ? size_rel.X : size_abs.X,
      coord_state.SizeY == CoordMetric.Relative ? size_rel.Y : size_abs.Y
    );

    offset_ = offset;
    size_ = size;

    // don't like this
    offset_px_ = canvas.GetSizePx() * offset_;
    size_px_ = canvas.GetSizePx() * size_;
  }

  // called when the size of a given component changes
  // gives view the opportunity to resize its contents
  public virtual void Reflow(ICanvas canvas) {}
  
  public virtual void Draw(ICanvas canvas) {}

  // idea: optional "state" provided here
  // think some sort of "key:value" object would be "fine"
  // (could do the same thing on creation)
  protected void PopSelf() {
    _dispose = true;

    // smth for queueing this instead of actually "removing"
    foreach (Action a in pop_listeners) {
      a();
    }
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

  public void ClearTransitions() {
    transitions.Clear();
  }

  public bool TransitionsComplete() {
    return transitions.Complete();
  }

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