using System.Collections.Generic;
using digipet.component;

namespace digipet.view;

public interface IContainer {
  IReadOnlyList<ViewComponent> GetComponents();
  void AddView(ViewComponent v);
  void RemoveView(ViewComponent v);
}