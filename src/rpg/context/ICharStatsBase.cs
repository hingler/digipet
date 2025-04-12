namespace digipet.rpg.context;

public interface ICharStatsBase {
  public long Attack { get; }
  public long Wisdom { get; }
  public long Defense { get; }
  public long Weight { get; }
  public long Vitality { get; }
  public long Speed { get; }
}