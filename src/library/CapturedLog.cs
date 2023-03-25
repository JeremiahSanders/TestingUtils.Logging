using Microsoft.Extensions.Logging;

namespace TestingUtils.Logging;

/// <summary>
///   A captured result of <see cref="ILogger.Log{TState}" />.
/// </summary>
/// <param name="LogLevel">The log level.</param>
/// <param name="EventId">The event id.</param>
/// <param name="State">Optional. The state.</param>
/// <param name="Exception">Optional. The exception.</param>
/// <param name="Message">The rendered log message.</param>
public record CapturedLog(LogLevel LogLevel, EventId EventId, object? State, Exception? Exception, string Message)
{
}

/// <summary>
///   A captured result of <see cref="ILogger.Log{TState}" />.
/// </summary>
/// <param name="LogLevel">The log level.</param>
/// <param name="EventId">The event id.</param>
/// <param name="TypedState">Optional. The state.</param>
/// <param name="Exception">Optional. The exception.</param>
/// <param name="Message">The rendered log message.</param>
/// <typeparam name="TState">The state type.</typeparam>
public record CapturedLog<TState>(LogLevel LogLevel, EventId EventId, TState? TypedState, Exception? Exception,
  string Message) : CapturedLog(LogLevel, EventId, TypedState, Exception, Message)
{
}
