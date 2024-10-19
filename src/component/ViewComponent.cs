using digipet.input;

namespace digipet.component;

using System.Diagnostics;

public class ViewComponent : IComponent {
  public ViewComponent() {
    _stack_child = null;
    activated = false;
  }
  public virtual void HandleInput(InputType input, InputState state) {}

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
  public virtual void Tick(double delta) {}
  public virtual void Deactivate() {}

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