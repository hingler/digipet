namespace digipet.input;

public interface IKeyEvent {
  string KeyChar { get; }
  InputState State { get; }
  InputType Action { get; }

  KeyFlags Flags { get; }
}

public interface IInputListener {
  public void OnInput(InputType type, InputState state);
  public void OnKey(IKeyEvent key);
}

public interface IInputManager {
  void Register(IInputListener listener);
  void Unregister(IInputListener listener);

  bool IsJustPressed(InputType key);
  bool IsPressed(InputType key);
  bool IsJustReleased(InputType key);

  
}