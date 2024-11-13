using digipet.world.pet.animation;
using digipet.world.pet.task;

namespace digipet.world.pet;

// fetch tasks from task manager
// fetch animations from animation manager

public interface IPetController {
  void AddAnimationHandler(PetAnimation animation, IAnimationHandler handler);
  void AddTask(IPetTask task);
  // update call
  void Update(double delta);

  IPetStateReadOnly GetPetState();
}