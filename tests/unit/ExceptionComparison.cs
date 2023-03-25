namespace TestingUtils.Logging.Tests.Unit;

internal static class ExceptionComparison
{
  public static bool MatchException(Exception? a, Exception? b)
  {
    return (a == null && b == null) ||
           (a != null && b != null && a.GetType() == b.GetType() && a.Message == b.Message);
  }
}
