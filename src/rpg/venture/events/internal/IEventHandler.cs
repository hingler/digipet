using digipet.rpg.venture.events;

namespace digipet.rpg.venture;

public interface IEventHandler {
  public void RegisterListener(IVentureEventListener listener);
  public void UnregisterListener(IVentureEventListener listener);

  public void Tick(double delta);
}

public class StubEventHandler : IEventHandler {
  public void RegisterListener(IVentureEventListener listener) {}
  public void UnregisterListener(IVentureEventListener listener) {}

  public void Tick(double delta) {}
}