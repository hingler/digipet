namespace digipet.world.pet.task.builder;

public class TaskBuilderWrap(IPetTask task) : IPetTaskFactory {
  public IPetTask CreatePetTask() => task;
  public int GetPriority() => task.GetPriority();
}