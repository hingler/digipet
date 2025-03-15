namespace digipet.rpg.venture.events;

#nullable enable

public struct DialogueInfo {
  public CharInfo? Speaker;
  public string Text;

  // addl info
}

public interface IVentureEventListener {
  // stop here
  public void OnDialogue(DialogueInfo info);

  // stop progress, let some other component figure out what to fight
  public void OnBattle();
}