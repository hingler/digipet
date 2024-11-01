using System.Reflection;

namespace digipet.transition.helper;

public class FieldWrap<T>(FieldInfo fieldInfo, object obj) : IMemberWrap<T> {
  private readonly FieldInfo fieldInfo = fieldInfo;
  private readonly object obj = obj;

  public T GetValue() {
    return (T)fieldInfo.GetValue(obj);
  }

  public void SetValue(T val) {
    fieldInfo.SetValue(obj, val);
  }

  public bool Valid() {
    return fieldInfo != null;
  }
}