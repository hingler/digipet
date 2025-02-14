namespace digipet.world.pet;

#nullable enable

// alternate means of creating pet tasks
// builds 
public interface IPetTaskFactory {
  // null if pet task cannot be created
  // else, returns new task
  IPetTask? CreatePetTask();

  // fetches the priority of this task.
  int GetPriority();
}