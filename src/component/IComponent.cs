using System.Numerics;
using digipet.input;

namespace digipet.component;

public interface IDigiComponent {

  // called when input is received
  // (enum for input types)
  // inputs bubble down call stack
  // return true to consume, false to let bubble down
  bool HandleInput(
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