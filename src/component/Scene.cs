using System.Collections.Generic;
using digipet.framework;
using digipet.input;
using digipet.transition;

namespace digipet.component;

public abstract class Scene : IDigiComponent {
  private readonly IList<ViewComponent> stack = [];
  private bool init_flag;

  private bool _finished;
  public bool Finished {
    get => _finished;
  }

  private TransitionQueue transitions = new();
  public readonly IEngine Engine;

  // how do we want to "initialize" scenes?

  public Scene(IEngine engine) {
    init_flag = false;
    Engine = engine;
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
        GetTopComponent().PreDestroy();
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

  public void PreInput(
    InputType type,
    InputState state
  ) {
    bool consumed = false;
    int cursor = stack.Count - 1;
    while (!consumed && cursor >= 0) {
      consumed = consumed || stack[cursor--].PreInput(type, state);
    }

    if (!consumed) {
      HandleInput(type, state);
    }
  }

  public virtual bool HandleInput(InputType type, InputState state) => false;

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

  public void EnqueueTransition(ITransition transition) {
    // should we be able to run multiple at once??
    transitions.Enqueue(transition);
  }

  public void AdvanceTransition() {
    transitions.Advance();
  }

  public bool TransitionComplete() {
    // complete if empty, or if last transition is complete
    return transitions.Complete();
  }

  // non-overridable method used to delegate activate to descendants
  public void PreActivate() {
    Activate();
    GetTopComponent()?.PreActivate();
  }

  public void SceneTick(double delta) {
    transitions.Tick(delta);

    Tick(delta);

    // then call component tick
    for (int i = 0; i < stack.Count; i++) {
      stack[i].PreTick(delta);
    }

    CheckForStackChanges();
  }


  public void Draw(ICanvas canvas) {
    for (int i = 0; i < stack.Count; i++) {
      stack[i].PreDraw(canvas);
    }
  }

  public void PreDeactivate() {
    Deactivate();
    GetTopComponent()?.PreDeactivate();
  }

  protected void Finish() {
    // no cleanup
    _finished = true;
  }


  public void PreDestroy() {
    while (GetTopComponent() != null) {
      PopFromStack();
    }

    Destroy();
  }

  public virtual void Destroy() {}

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