namespace Jds.TestingUtils.Logging.Tests.Unit;

public class CapturingLoggerProviderTests
{
  private readonly ILoggerFactory _factory;
  private readonly ILoggerProvider _provider;
  private readonly ICapturedLogStore _store;

  public CapturingLoggerProviderTests()
  {
    _store = new CapturedLogStore();
    var expectedCategory = typeof(CapturingLoggerProviderTests).FullName ??
                           throw new InvalidOperationException("Type name was null");
    _provider = new CapturingLoggerProvider(category =>
      category == expectedCategory ? _store : throw new InvalidOperationException($"Unexpected category: {category}"))
    {
      DefaultMinimumLogLevel = LogLevel.Trace
    };
    _factory = new LoggerFactory(new[] {_provider});
  }

  [Fact]
  public void ProvidesCapturingLoggers()
  {
    var logger = _factory.CreateLogger<CapturingLoggerProviderTests>();
    var scopeState = "My Scope";
    const string message = "my message";

    using (var scope = logger.BeginScope(scopeState))
    {
      logger.LogInformation(message);
    }

    var loggedScopeBegin = _store.SingleOrDefault(captured =>
      captured.Message == $"Begin Scope: {scopeState}" && captured.EventId == CapturingLoggerEvents.BeginScopeEventId);
    var loggedScopeEnd = _store.SingleOrDefault(captured =>
      captured.Message == $"End Scope: {scopeState}" && captured.EventId == CapturingLoggerEvents.EndScopeEventId);
    var loggedMessage = _store.SingleOrDefault(captured => captured.Message == message);
    Assert.NotNull(loggedScopeBegin);
    Assert.NotNull(loggedMessage);
    Assert.NotNull(loggedScopeEnd);
  }
}
