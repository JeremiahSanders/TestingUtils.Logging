using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace TestingUtils.Logging.Tests.Unit;

public class CapturingLoggerTests
{
  private readonly ICapturingLogger _logger;
  private readonly ITestOutputHelper _testOutputHelper;

  public CapturingLoggerTests(ITestOutputHelper testOutputHelper)
  {
    _testOutputHelper = testOutputHelper;
    _logger = new CapturingLogger {MinimumLogLevel = LogLevel.Trace};
  }

  private static string Formatter(object? state, Exception? exception)
  {
    return state switch
    {
      null => exception switch
      {
        null => "null",
        not null => exception.ToString()
      },
      not null => exception switch
      {
        null => state.ToString(),
        not null => $"{state} {exception}"
      }
    } ?? string.Empty;
  }

  public static IEnumerable<object?[]> CreateTestLogs(int eventId, object? state, string? exceptionMessage)
  {
    object?[] CreateCase(LogLevel logLevel, EventId eventIdValue, object? stateValue, Exception? exception)
    {
      return new[] {logLevel, eventIdValue, stateValue, exception};
    }

    var id = new EventId(eventId);
    state ??= 9876543;
    var exception = new InvalidOperationException(exceptionMessage);
    foreach (var logLevel in new[]
             {
               LogLevel.Trace, LogLevel.Debug, LogLevel.Information, LogLevel.Warning, LogLevel.Error,
               LogLevel.Critical
             })
    {
      yield return CreateCase(logLevel, eventId, null, exception);
      yield return CreateCase(logLevel, eventId, state, null);
      yield return CreateCase(logLevel, eventId, state, exception);
    }
  }

  [Fact]
  public void LogsScopes()
  {
    const string expectedScope = "an expected scope";
    const string expectedMessage = "an expected message";
    using (var scope = _logger.BeginScope(expectedScope))
    {
      _logger.LogInformation(expectedMessage);
    }

    _ = _logger.Logs.Single(captured => captured.Message == $"Begin Scope: {expectedScope}" &&
                                        captured.EventId == CapturingLoggerEvents.BeginScopeEventId);
    _ = _logger.Logs.Single(captured =>
      captured.Message == $"End Scope: {expectedScope}" && captured.EventId == CapturingLoggerEvents.EndScopeEventId);
  }

  [Theory]
  [MemberData(nameof(CreateTestLogs), 42, "a value", "an expected exception occurred")]
  public void LogsMessages(LogLevel logLevel, EventId eventId, object? state, Exception? exception)
  {
    var expected = new CapturedLog(logLevel, eventId, state, exception, Formatter(state, exception));

    _logger.Log(logLevel, eventId, state, exception, Formatter);

    _testOutputHelper.WriteLine($"Logs: {string.Join(", ", _logger.Logs)}");
    _ = _logger.Logs.Single(captured =>
      captured.LogLevel == expected.LogLevel
      && captured.EventId == expected.EventId
      && captured.State == expected.State
      && captured.Message == expected.Message
      && ExceptionComparison.MatchException(captured.Exception, expected.Exception)
    );
    _ = _logger.Logs.Single();
  }
}
