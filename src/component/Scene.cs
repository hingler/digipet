using System.Collections.Generic;
using System.Reflection.Metadata;
using digipet.input;

namespace digipet.component;

public abstract class Scene : IComponent {
  private readonly IList<ViewComponent> stack = [];

  private bool init_flag;

  // how do we want to "initialize" scenes?

  public Scene() {
    init_flag = false;
  }

  public bool Initialized() {
    return init_flag;
  }

  protected void PushToStack(ViewComponent v) {
    if (init_flag) {
      // deactivate only if scene has already been init'd
      GetTopComponent()?.PreDeactivate();
    }

    stack.Add(v);

    if (init_flag) {
      v.Create();
      v.PreActivate();
    }
  }

  protected void PopFromStack() {
    if (stack.Count > 0) {
      if (init_flag) {
        GetTopComponent().PreDeactivate();
      }

      stack.RemoveAt(stack.Count - 1);

      if (init_flag) {
        GetTopComponent()?.PreActivate();
      }
    }
  }

  public ViewComponent GetTopComponent() {
    return stack.Count > 0 ? stack[stack.Count - 1] : null;
  }

  public void HandleInput(
    InputType type,
    InputState state
  ) {
    GetTopComponent()?.HandleInput(type, state);
  }

  // called by the user to initialize this scene
  public abstract void InitScene();

  public void Create() {
    InitScene();
    init_flag = true;
    foreach (ViewComponent c in stack) {
      c.Create();
    }
  }


  // overridables
  public virtual void Activate() {}
  public virtual void Tick(double delta) {}
  public virtual void Deactivate() {}

  // non-overridable method used to delegate activate to descendants
  public void PreActivate() {
    Activate();
    GetTopComponent()?.PreActivate();
  }
  public void SceneTick(double delta) {
    // call scene tick first
    Tick(delta);

    // then call component tick
    GetTopComponent()?.Tick(delta);
    CheckForStackChanges();
  }


  public void Draw(ICanvas canvas) {
    for (int i = 0; i < stack.Count; i++) {
      stack[i].Draw(canvas);
    }
  }

  public void PreDeactivate() {
    Deactivate();
    GetTopComponent()?.PreDeactivate();
  }


  public void Destroy() {
    ViewComponent top = GetTopComponent();
    while (GetTopComponent() != null) {
      PopFromStack();
    }
  }

  private void CheckForStackChanges() {
    ViewComponent top = GetTopComponent();
    ViewComponent request = top?.AcknowledgePush() ?? null;
    if (request != null) {
      PushToStack(request);
      
    } else {
      while (GetTopComponent()?.AcknowledgeDispose() ?? false) {
        PopFromStack();
      }
    }
  }
}