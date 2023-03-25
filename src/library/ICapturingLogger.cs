using Microsoft.Extensions.Logging;

namespace TestingUtils.Logging;

/// <summary>
///   A <see cref="ILogger" /> which captures logged messages.
/// </summary>
public interface ICapturingLogger : ILogger
{
  /// <summary>
  ///   Gets the captured log messages.
  /// </summary>
  IReadOnlyList<CapturedLog> Logs { get; }
}
