namespace digipet.transition;

public interface ITransition {
  // updates animation dependencies
  void Tick(double delta);

  // attempts to skip the current animation, or advance to the next scene
  void Advance();

  // returns true if this transition is complete, false otherwise.
  bool Complete();
}