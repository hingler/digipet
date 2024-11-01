using System.Reflection;

namespace digipet.transition.helper;

public class PropertyWrap<T>(PropertyInfo propertyInfo, object obj) : IMemberWrap<T> {
  private readonly PropertyInfo propertyInfo = propertyInfo;
  private readonly object obj = obj;

  public T GetValue() {
    return (T)propertyInfo.GetValue(obj);
  }

  public void SetValue(T val) {
    propertyInfo.SetValue(obj, val);
  }

  public bool Valid() {
    return propertyInfo != null;
  }
}