namespace digipet.transition.helper;

public class FallbackFieldWrap<T> : IMemberWrap<T> {
  public T GetValue() {
    return default;
  }

  public void SetValue(T val) { /* no op */ }
  public bool Valid() => false;
}