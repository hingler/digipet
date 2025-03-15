using digipet.rpg.venture.events;
using digipet.rpg.venture.progress;

namespace digipet.rpg.venture;

// manages an ongoing "adventure"
public class VentureManager : IVentureEventListener {
  private readonly List<CharInfo> char_list;
  private readonly IEventHandler event_handler;
  private readonly IProgressHandler progress_handler;
  

  public VentureManager(
    IReadOnlyList<CharInfo> team
  ) : this(
    team,
    new StubEventHandler(),
    new StubProgressHandler()
  ) {}

  public VentureManager(
    IReadOnlyList<CharInfo> team, 
    IEventHandler event_manager,
    IProgressHandler progress_handler
  ) {

    char_list = [..team];
    event_handler = event_manager;
    this.progress_handler = progress_handler;

    event_handler.RegisterListener(this);
  }

  // tick?
  public void Tick(double delta) {
    progress_handler.Tick(delta);
    
    // decides when to show events right
    event_handler.Tick(delta);
  }

  public void OnDialogue(DialogueInfo info) {
    // reroute to some dialogue handler component
  }

  public void OnBattle() {
    // pause progress
    // rely on a battle handler to decide what battle is going on
    // send a "handler" which will resolve when the battle is complete
  }

  // thinking: the workflow is going to be event based
  // we generate an event-prompt, which stops our in-game clock
  // the user responds, and when they do, we start the in-game clock again

  // (do we stop sending ticks, or do we tell the progress handler to halt??)
}