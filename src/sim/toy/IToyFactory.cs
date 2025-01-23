using digipet.framework;

namespace digipet.sim.toy;

// create handler
// create activator

// return handler and activator?
public interface IToyFactory {
  // creates a new toy handler and returns it
  BaseToyHandler Create(
    IEngine engine
  );
}