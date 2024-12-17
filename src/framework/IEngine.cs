using digipet.canvas;
using digipet.canvas.font;
using digipet.component;
using digipet.db;
using digipet.file;
using digipet.input;
using digipet.sim;
using digipet.sim.db;
using digipet.sprite.attrib;
using digipet.world;

namespace digipet.framework;

#nullable enable

public interface IEngine : IEngineBase {
  
  // push a scene to scene stack (a la android) and begin running
  void PushScene(Scene scene);

  // returns an asset repo for the specified asset type
  ISimRepo<T>? GetAssetRepo<T>() where T : IWorldItem;

  // returns ref to underlying 
  IPhysWorld GetPhysWorld();
}

public interface IEngineBase {
  // returns a new sub canvas
  ISubCanvas CreateSubCanvas();
  ISpriteFetcher GetSpriteFetcher();
  IInputManager GetInputManager();
  IFileLoader GetResourceLoader();
  IFileLoader GetUserdataLoader();
  IFileLoader GetDigipetAssetLoader();
  IDataStore GetSaveStore();

  IFontHelper GetFontHelper();

  public virtual void TearDown() {}

  // closes the window
  public virtual void CloseGame() {}
}