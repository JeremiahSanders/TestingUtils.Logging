using Microsoft.Extensions.Logging;

namespace Jds.TestingUtils.Logging;

/// <summary>
///   Default implementation of <see cref="ICapturingLogger" />.
/// </summary>
public class CapturingLogger : ICapturingLogger
{
  private readonly ICapturedLogStore _logStore;

  /// <summary>
  ///   Initializes a new instance of the <see cref="CapturingLogger{T}" /> class.
  /// </summary>
  public CapturingLogger() : this(new CapturedLogStore()) { }

  /// <summary>
  ///   Initializes a new instance of the <see cref="CapturingLogger{T}" /> class.
  /// </summary>
  /// <param name="logDestination">A <see cref="IList{T}" /> which will be used to store messages.</param>
  /// <remarks>
  ///   <para>
  ///     While <see cref="CapturedLogStore" /> will use its own locking mechanism to provide thread safety internally,
  ///     unexpected results may occur if <paramref name="logDestination" /> is accessed concurrently elsewhere.
  ///   </para>
  /// </remarks>
  public CapturingLogger(IList<CapturedLog> logDestination) : this(new CapturedLogStore(logDestination)) { }

  /// <summary>
  ///   Initializes a new instance of the <see cref="CapturingLogger{T}" /> class.
  /// </summary>
  /// <param name="capturedLogStoreStore">A <see cref="ICapturedLogStore" /> which will be used to store messages.</param>
  public CapturingLogger(ICapturedLogStore capturedLogStoreStore)
  {
    _logStore = capturedLogStoreStore;
  }

  /// <summary>
  ///   Gets or sets the minimum log level which is currently enabled (supporting <see cref="ILogger.IsEnabled" />).
  /// </summary>
  /// <value>Defaults to <see cref="LogLevel.Trace" />.</value>
  public LogLevel MinimumLogLevel { get; set; } = LogLevel.Trace;


  /// <inheritdoc cref="ICapturingLogger.Logs" />
  public IReadOnlyList<CapturedLog> Logs => _logStore;

  /// <inheritdoc cref="ILogger.Log{TState}" />
  public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
    Func<TState, Exception?, string> formatter)
  {
    _logStore.Add(new CapturedLog<TState>(logLevel, eventId, state, exception, formatter(state, exception)));
  }

  /// <inheritdoc cref="ILogger.IsEnabled" />
  public bool IsEnabled(LogLevel logLevel)
  {
    return logLevel >= MinimumLogLevel;
  }

  /// <inheritdoc cref="ILogger.BeginScope{TState}" />
  public IDisposable BeginScope<TState>(TState state) where TState : notnull
  {
    _logStore.Add(new CapturedLog<TState>(LogLevel.Debug, CapturingLoggerEvents.BeginScopeEventId, state, null,
      $"Begin Scope: {state}"));

    return new CallbackScope(() => _logStore.Add(new CapturedLog<TState>(LogLevel.Debug,
      CapturingLoggerEvents.EndScopeEventId, state, null,
      $"End Scope: {state}")));
  }

  private class CallbackScope : IDisposable
  {
    private readonly Action? _disposalCallback;

    public CallbackScope(Action? disposalCallback = null)
    {
      _disposalCallback = disposalCallback;
    }

    public void Dispose()
    {
      _disposalCallback?.Invoke();
    }
  }
}

/// <summary>
///   A typed <see cref="CapturingLogger" />.
/// </summary>
/// <typeparam name="TCategoryName">A type which may be used for applying a category name.</typeparam>
public class CapturingLogger<TCategoryName> : CapturingLogger, ILogger<TCategoryName>
{
  /// <summary>
  ///   Initializes a new instance of the <see cref="CapturingLogger{TCategoryName}" /> class.
  /// </summary>
  public CapturingLogger()
  {
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="CapturingLogger{TCategoryName}" /> class.
  /// </summary>
  /// <param name="logDestination">A <see cref="IList{T}" /> which will be used to store messages.</param>
  /// <remarks>
  ///   <para>
  ///     While <see cref="CapturedLogStore" /> will use its own locking mechanism to provide thread safety internally,
  ///     unexpected results may occur if <paramref name="logDestination" /> is accessed concurrently elsewhere.
  ///   </para>
  /// </remarks>
  public CapturingLogger(IList<CapturedLog> logDestination) : base(logDestination)
  {
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="CapturingLogger{TCategoryName}" /> class.
  /// </summary>
  /// <param name="capturedLogStoreStore">A <see cref="ICapturedLogStore" /> which will be used to store messages.</param>
  public CapturingLogger(ICapturedLogStore capturedLogStoreStore) : base(capturedLogStoreStore)
  {
  }
}
