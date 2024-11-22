using System.Numerics;
using digipet.canvas.font;
using digipet.component;
using digipet.framework;
using digipet.image;
using digipet.input;
using digipet.sim;
using digipet.sprite.attrib;
using digipet.transition.text;
using digipet.user;
using digipet.util;
using digipet.view;
using digipet.view.bg;
using digipet.view.container;
using digipet.view.menu;
using digipet.view.npc;
using digipet.view.store;
using digipet.view.text;

namespace digipet.scenes;

public class StoreScene : Scene {
  public StoreScene(IEngine engine, IUserData userData) : this(engine, [], userData) {}

  public StoreScene(
    IEngine engine,
    IReadOnlyCollection<IWorldItem> items,
    IUserData userData
  ) : base(engine) {
    PushToStack(new StoreHandler(engine, items, userData));
  }

  public override void InitScene() {
    // nop
  }

  
}