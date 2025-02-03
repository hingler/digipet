using digipet.framework;

namespace digipet.sim.toy;

// create handler
// create activator

// return handler and activator?

public interface IToy : IPurchasable {
  
}

public interface IToyFactory : IToy {
  // creates a new toy handler and returns it
  // "activate" is a weak flow - factories should instantiate
  BaseToyHandler Create(
    IEngine engine
  );
}