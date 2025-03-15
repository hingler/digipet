namespace digipet.rpg.venture.events;

public interface IBaseEvent {
  // nothing hehehe
}

public interface IDialogueEvent : IBaseEvent {
  public CharInfo? Speaker { get; }
  public string Content { get; }
}

