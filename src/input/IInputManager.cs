namespace digipet.input;

public interface IInputListener {
  public void OnInput(InputType type, InputState state);
}

public interface IInputManager {
  void Register(IInputListener listener);
  void Unregister(IInputListener listener);

  bool IsJustPressed(InputType key);
  bool IsPressed(InputType key);
  bool IsJustReleased(InputType key);

  
}