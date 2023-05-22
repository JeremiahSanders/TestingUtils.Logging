using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Jds.TestingUtils.Logging;

/// <summary>
///   An <see cref="ILoggerProvider" /> which can be created with <see cref="ICapturedLogStore" />s to enable logged
///   messages to be accessed.
/// </summary>
public class CapturingLoggerProvider : ILoggerProvider
{
  private readonly ICapturedLogStore? _capturedLogStore;
  private readonly Func<string, ICapturedLogStore>? _logStoreProvider;

  /// <summary>
  /// </summary>
  /// <param name="capturedLogStore">
  ///   Optional. Recommended. An <see cref="ICapturedLogStore" /> which will be shared by all
  ///   created loggers.
  /// </param>
  public CapturingLoggerProvider(ICapturedLogStore? capturedLogStore = null)
  {
    _capturedLogStore = capturedLogStore;
  }

  /// <summary>
  /// </summary>
  /// <param name="logStoreProvider">
  ///   A function which accepts a category name (i.e., the parameter to
  ///   <see cref="ILoggerProvider.CreateLogger" />) and returns a <see cref="ICapturedLogStore" />.
  /// </param>
  public CapturingLoggerProvider(Func<string, ICapturedLogStore> logStoreProvider)
  {
    _logStoreProvider = logStoreProvider;
  }

  /// <summary>
  ///   Gets or sets the default <see cref="CapturingLogger.MinimumLogLevel" /> for created <see cref="ILogger" /> instances.
  /// </summary>
  public LogLevel DefaultMinimumLogLevel { get; set; } = LogLevel.Trace;

  /// <inheritdoc cref="IDisposable.Dispose" />
  public void Dispose()
  {
  }

  /// <inheritdoc cref="ILoggerProvider.CreateLogger" />
  public ILogger CreateLogger(string categoryName)
  {
    return _logStoreProvider != null
      ? new CapturingLogger(_logStoreProvider(categoryName)) {MinimumLogLevel = DefaultMinimumLogLevel}
      : _capturedLogStore != null
        ? new CapturingLogger(_capturedLogStore) {MinimumLogLevel = DefaultMinimumLogLevel}
        : new CapturingLogger {MinimumLogLevel = DefaultMinimumLogLevel};
  }

  /// <summary>
  ///   Creates a <see cref="CapturingLoggerProvider" /> which uses <paramref name="logStore" /> to maintain a separate
  ///   <see cref="ICapturedLogStore" /> for each logging <c>categoryName</c> (<see cref="ILoggerFactory.CreateLogger" />
  ///   parameter).
  /// </summary>
  /// <remarks>
  ///   When <see cref="ILogger{TCategoryName}" /> instances are created by a <see cref="ILoggerFactory" /> the
  ///   <c>TCategoryName</c> generic type argument is used as the category name.
  /// </remarks>
  /// <param name="logStore">
  ///   A dictionary to maintain log stores by category. Consider using a
  ///   <see cref="ConcurrentDictionary{TKey,TValue}" />.
  /// </param>
  /// <returns></returns>
  public static CapturingLoggerProvider FromDictionary(IDictionary<string, ICapturedLogStore> logStore)
  {
    return new CapturingLoggerProvider(category =>
    {
      if (!logStore.ContainsKey(category))
      {
        logStore[category] = new CapturedLogStore();
      }

      return logStore[category];
    });
  }
}
