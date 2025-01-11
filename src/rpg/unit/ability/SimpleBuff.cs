using digipet.rpg.context;
using digipet.rpg.context.simple;
using digipet.rpg.unit;

namespace digipet.rpg.unit.ability;

public class SimpleBuff : ICharBuff {
  public SimpleCharStats Mult = new();
  public SimpleCharStats Add = new();

  public ICharStats BuffMult => Mult;
  public ICharStats BuffAdd => Add;

  private double time_remaining;
  public double TimeRemaining => time_remaining;

  public SimpleBuff() {
    Mult.Set(1);
    Add.Set(0);

    time_remaining = 1;
  }
}