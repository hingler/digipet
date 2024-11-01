using digipet.canvas;
using digipet.component;
using digipet.input;
using digipet.sprite.attrib;

namespace digipet.framework;

public interface IEngine : IEngineBase {
  
  // push a scene to scene stack (a la android) and begin running
  void PushScene(Scene scene);
}

public interface IEngineBase {
  // returns a new sub canvas
  ISubCanvas CreateSubCanvas();
  ISpriteFetcher GetSpriteFetcher();
  IInputManager GetInputManager();
}