using graphui.node;

namespace digipet.input;

public static class InputToUIDirection {
  public static Direction Convert(IKeyEvent @event) {
    if (@event.State == InputState.RELEASE) {
      return Direction.NONE;
    }

    return @event.Action switch {
      InputType.LEFT => Direction.LEFT,
      InputType.RIGHT => Direction.RIGHT,
      InputType.DOWN => Direction.DOWN,
      InputType.UP => Direction.UP,
      _ => Direction.NONE
    };
  }
}
