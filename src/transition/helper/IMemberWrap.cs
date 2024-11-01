namespace digipet.transition.helper;

public interface IMemberWrap<T> {
  void SetValue(T value);
  bool Valid();
}