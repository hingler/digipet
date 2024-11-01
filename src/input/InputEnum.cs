namespace digipet.input;

public enum InputType {
  LEFT,
  RIGHT,
  UP,
  DOWN,
  CONFIRM,
  BACK
}

public enum InputState {
  // broadcast on init press
  PRESS,
  // after short interval, broadcast as "hold" repeatedly
  HOLD,
  RELEASE
}