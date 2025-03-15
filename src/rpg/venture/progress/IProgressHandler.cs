namespace digipet.rpg.venture.progress;

public interface IProgressHandler {
  void Tick(double delta);
}

public class StubProgressHandler : IProgressHandler {
  public void Tick(double delta) {}
}