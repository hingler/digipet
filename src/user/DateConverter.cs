namespace digipet.user;

public class DateConverter {
  // returns an int uniquely identifying a given calendar date

  public int GetTodayAsInt() {
    return DateToInt(DateTime.Now);
  }
  public int DateToInt(DateTime date) {
    DateOnly date_only = DateOnly.FromDateTime(date);
    return date_only.DayNumber;
  }

  public DateOnly IntToDate(int date) {
    return DateOnly.FromDayNumber(date);

  }
}