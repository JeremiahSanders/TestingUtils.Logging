namespace Jds.TestingUtils.Logging;

/// <summary>
///   A mutable <see cref="CapturedLog" /> store.
/// </summary>
public interface ICapturedLogStore : IReadOnlyList<CapturedLog>
{
  /// <summary>
  ///   Adds a <see cref="CapturedLog" /> to this store.
  /// </summary>
  /// <param name="capturedLog">A <see cref="CapturedLog" />.</param>
  void Add(CapturedLog capturedLog);
}
