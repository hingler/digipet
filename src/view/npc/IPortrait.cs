using digipet.pet;

namespace digipet.view.npc;

// simple interface for working with portraits
public interface IPortrait {
  bool Talk { get; set; }
  PetEmote Emote { get; set; }


  public void Tick(double delta);
}