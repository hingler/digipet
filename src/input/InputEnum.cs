namespace digipet.input;

public enum InputType {
  LEFT,
  RIGHT,
  UP,
  DOWN,
  CONFIRM,
  BACK,

  // does not map
  UNBOUND,

  // unknown
  UNKNOWN
}

public enum InputState {
  // broadcast on init press
  PRESS,
  // after short interval, broadcast as "hold" repeatedly
  HOLD,
  RELEASE
}