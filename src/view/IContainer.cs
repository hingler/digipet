using System.Collections.Generic;
using digipet.component;

namespace digipet.view;

public interface IContainer {
  IReadOnlyList<ViewComponent> GetChildren();
  void AddView(ViewComponent v);
  void RemoveView(ViewComponent v);
}