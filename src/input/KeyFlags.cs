namespace digipet.input;

[Flags]
public enum KeyFlags {
  NONE = 0,
  // ctrl held
  CTRL = 1,

  // alt held
  ALT = 2,

  // shift held
  SHIFT = 4,

  // key is a delete key
  DELETE = 8,
  
  // add line break
  LINE_BREAK = 16,
}