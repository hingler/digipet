using digipet.input;

namespace digipet.component;

using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using digipet.canvas;

public class ViewComponent : IDigiComponent {

  private Vector2 offset_;
  private Vector2 offset_px_;

  private Vector2 size_;
  private Vector2 size_px_;

  // for offsets
  private bool pixel_coords = false;

  // for sizes
  private bool pixel_size = false;



  private Vector2 anchor_;

  private bool reflow_dirty_;

  // offset of anchor relative to parent
  public Vector2 Offset {
    get => offset_;
    set {
      offset_ = value;
      reflow_dirty_ = true;
      pixel_coords = false;
    }
  }

  public Vector2 OffsetPx {
    get => offset_px_;
    set {
      offset_px_ = value;
      reflow_dirty_ = true;
      pixel_coords = true;
    }
  }

  public float X {
    get => offset_.X;
    set {
      offset_.X = value;
      reflow_dirty_ = true;
      pixel_coords = false;
    }
  }

  public float Y {
    get => offset_.Y;
    set {
      offset_.Y = value;
      reflow_dirty_ = true;
      pixel_coords = false;
    }
  }

  public float PixelX {
    get => offset_px_.X;
    set {
      offset_px_.X = value;
      reflow_dirty_ = true;
      pixel_coords = true;
    }
  }

  public float PixelY {
    get => offset_px_.Y;
    set {
      offset_px_.Y = value;
      reflow_dirty_ = true;
      pixel_coords = true;
    }
  }

  // size relative to parent
  public Vector2 Size {
    get => size_;
    set {
      size_ = value;
      reflow_dirty_ = true;
      pixel_size = false;
    }
  }

  public Vector2 SizePx {
    get => size_px_;
    set {
      size_px_ = value;
      reflow_dirty_ = true;
      pixel_size = true;
    }
  }

  // position of anchor point, relative to this component (0.0 - 1.0)
  public Vector2 Anchor {
    get => anchor_;
    set {
      anchor_ = value;
      reflow_dirty_ = true;
    }
  }

  public float Opacity;

  public int ZIndex;

  public ViewComponent() {
    _stack_child = null;
    activated = false;

    Offset = Vector2.Zero;
    Size = Vector2.One;
    Anchor = Vector2.Zero;
    Opacity = 1.0f;
    
    ZIndex = 0;
  }

  public virtual IReadOnlyList<ViewComponent> GetChildren() => [];

  public bool PreInput(InputType input, InputState state) {
    // container handles first
    if (HandleInput(input, state)) {
      return true;
    }

    IReadOnlyList<ViewComponent> children = GetChildren();

    // children handle second
    for (int i = children.Count - 1; i >= 0; i--) {
      ViewComponent c = children[i];
      if (c.HandleInput(input, state)) {
        return true;
      }
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
    Tick(delta);
    foreach (ViewComponent child in GetChildren()) {
      child.PreTick(delta);
    }
  }
  public virtual void Tick(double delta) {}
  public virtual void Deactivate() {}

  public void PreDraw(ICanvas canvas) {
    // flush changes
    if (pixel_coords) {
      // convert from relative -> absolute
      offset_ = canvas.PxToRelative(OffsetPx);
    } else {
      // convert from absolute -> relative
      offset_px_ = offset_ * canvas.GetSizePx();
    }

    // always updates sizing - i'm fine with that tbh
    if (pixel_size) {
      size_ = canvas.PxToRelative(SizePx);
    } else {
      size_px_ = size_ * canvas.GetSizePx();
    }

    Vector2 start = Offset - (Size * Anchor);
    OffsetCanvas c = new(canvas, start, Size, 1.0f, Opacity, ZIndex);
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