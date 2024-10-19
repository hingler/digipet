using digipet.input;

namespace digipet.component;

public interface IComponent {
  // called when input is received
  // (enum for input types)
  void HandleInput(
    InputType input,
    InputState state
  );

  // called when this component is added to the stack
  void Create();

  // called (after create) when this component is at the top of the stack
  void Activate();

  // called every update-frame.
  void Tick(double delta);

  // called when we wish to draw this component.
  void Draw(ICanvas canvas);

  // called (before destroy) when this component is no longer at the top of the stack
  // (either removed or covered)
  void Deactivate();
}