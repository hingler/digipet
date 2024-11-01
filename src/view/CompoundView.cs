using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using digipet.component;
using digipet.input;

namespace digipet.view;

public class CompoundView : ViewComponent, IContainer {
  private readonly ICollection<ViewComponent> sub_components = new HashSet<ViewComponent>();

  public CompoundView() : base() {}

  public IReadOnlyList<ViewComponent> GetComponents() {
    return GetChildren();
  }

  public void AddView(ViewComponent v) {
    sub_components.Add(v);
  }

  public void RemoveView(ViewComponent v) {
    // not stellar hehe
    sub_components.Remove(v);
  }

  public override IReadOnlyList<ViewComponent> GetChildren() {
    return [.. sub_components];
  }
}