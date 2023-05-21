using System.Collections;

namespace Jds.TestingUtils.Logging;

/// <summary>
///   Default implementation of <see cref="ICapturedLogStore" />.
/// </summary>
/// <remarks>
///   <para>
///     Uses an <see cref="IList{T}" /> to store captured logs. All interactions are performed within <c>lock</c>
///     statements to provide thread safety at the expense of concurrency.
///   </para>
/// </remarks>
public class CapturedLogStore : ICapturedLogStore
{
  private readonly object _concurrentUpdateLocker = new();
  private readonly IList<CapturedLog> _logDestination;

  /// <summary>
  ///   Initializes a new instance of the <see cref="CapturedLogStore" /> class.
  /// </summary>
  /// <param name="logDestination">A <see cref="IList{T}" /> which will be used to store messages.</param>
  /// <remarks>
  ///   <para>
  ///     While <see cref="CapturedLogStore" /> will use its own locking mechanism to provide thread safety internally,
  ///     unexpected results may occur if <paramref name="logDestination" /> is accessed concurrently elsewhere.
  ///   </para>
  /// </remarks>
  public CapturedLogStore(IList<CapturedLog> logDestination)
  {
    _logDestination = logDestination;
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="CapturedLogStore" /> class.
  /// </summary>
  public CapturedLogStore() : this(new List<CapturedLog>()) { }

  /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
  public IEnumerator<CapturedLog> GetEnumerator()
  {
    lock (_concurrentUpdateLocker)
    {
      return _logDestination.GetEnumerator();
    }
  }

  /// <inheritdoc cref="IEnumerable.GetEnumerator" />
  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }

  /// <inheritdoc cref="IReadOnlyCollection{T}.Count" />
  public int Count
  {
    get
    {
      lock (_concurrentUpdateLocker)
      {
        return _logDestination.Count;
      }
    }
  }

  /// <inheritdoc cref="IReadOnlyList{T}.this" />
  public CapturedLog this[int index]
  {
    get
    {
      lock (_concurrentUpdateLocker)
      {
        return _logDestination[index];
      }
    }
  }

  /// <inheritdoc cref="ICapturedLogStore.Add" />
  public void Add(CapturedLog capturedLog)
  {
    lock (_concurrentUpdateLocker)
    {
      _logDestination.Add(capturedLog);
    }
  }
}
