using Microsoft.Extensions.Logging;

namespace TestingUtils.Logging;

public static class CapturingLoggerEvents
{
  private const int BeginScopeHashCode = -583254972;
  private const int EndScopeHashCode = -505448997;

  /// <summary>
  ///   Gets the <see cref="EventId" /> recorded when <see cref="CapturingLogger" />
  ///   <see cref="ILogger.BeginScope{TState}" /> executes.
  /// </summary>
  public static EventId BeginScopeEventId { get; } = new(BeginScopeHashCode, "BeginScope");

  /// <summary>
  ///   Gets the <see cref="EventId" /> recorded when the <see cref="IDisposable" /> returned by
  ///   <see cref="CapturingLogger" /> <see cref="ILogger.BeginScope{TState}" /> executes.
  /// </summary>
  public static EventId EndScopeEventId { get; } = new(EndScopeHashCode, "EndScope");
}
