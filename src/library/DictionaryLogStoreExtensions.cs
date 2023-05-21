using Microsoft.Extensions.Logging;

namespace Jds.TestingUtils.Logging;

/// <summary>
///   Extension methods on <see cref="IDictionary{TKey,TValue}" /> and <see cref="IReadOnlyDictionary{TKey,TValue}" />
///   supporting the retrieval of captured logged messages.
/// </summary>
public static class DictionaryLogStoreExtensions
{
  /// <summary>
  ///   Attempts to retrieve a <see cref="ICapturedLogStore" /> from this <paramref name="logStore" /> by its
  ///   <see cref="Type" />-based category name.
  /// </summary>
  /// <param name="logStore">This log store instance.</param>
  /// <typeparam name="TCategoryName">
  ///   A logging category name. I.e., the type argument used by an
  ///   <see cref="ILogger{TCategoryName}" />.
  /// </typeparam>
  /// <returns>
  ///   The <see cref="ICapturedLogStore" /> if found, or <c>null</c> if not found. A <c>null</c> response is expected when
  ///   no messages have been logged for <typeparamref name="TCategoryName" />.
  /// </returns>
  public static ICapturedLogStore? TryGetCapturedLogStore<TCategoryName>(
    this IReadOnlyDictionary<string, ICapturedLogStore> logStore
  )
  {
    return logStore.TryGetValue(typeof(TCategoryName).FullName ?? string.Empty, out var capturedLogStore)
      ? capturedLogStore
      : null;
  }

  /// <summary>
  ///   Retrieves a <see cref="ICapturedLogStore" /> from this <paramref name="logStore" /> by its
  ///   <see cref="Type" />-based category name.
  /// </summary>
  /// <param name="logStore">This log store instance.</param>
  /// <typeparam name="TCategoryName">
  ///   A logging category name. I.e., the type argument used by an
  ///   <see cref="ILogger{TCategoryName}" />.
  /// </typeparam>
  /// <returns>The <see cref="ICapturedLogStore" />.</returns>
  /// <exception cref="KeyNotFoundException">
  ///   Thrown when no <see cref="ICapturedLogStore" /> exists for
  ///   <typeparamref name="TCategoryName" />. This is expected when no messages have been logged for
  ///   <typeparamref name="TCategoryName" />.
  /// </exception>
  public static ICapturedLogStore GetCapturedLogStore<TCategoryName>(
    this IReadOnlyDictionary<string, ICapturedLogStore> logStore
  )
  {
    return logStore.TryGetCapturedLogStore<TCategoryName>() ?? throw new KeyNotFoundException(
      $"No captured log store exists for category {typeof(TCategoryName).FullName}."
    );
  }

  /// <summary>
  ///   Attempts to retrieve a <see cref="ICapturedLogStore" /> from this <paramref name="logStore" /> by its
  ///   <see cref="Type" />-based category name.
  /// </summary>
  /// <param name="logStore">This log store instance.</param>
  /// <typeparam name="TCategoryName">
  ///   A logging category name. I.e., the type argument used by an
  ///   <see cref="ILogger{TCategoryName}" />.
  /// </typeparam>
  /// <returns>
  ///   The <see cref="ICapturedLogStore" /> if found, or <c>null</c> if not found. A <c>null</c> response is expected when
  ///   no messages have been logged for <typeparamref name="TCategoryName" />.
  /// </returns>
  public static ICapturedLogStore? TryGetCapturedLogStore<TCategoryName>(
    this IDictionary<string, ICapturedLogStore> logStore
  )
  {
    return logStore.TryGetValue(typeof(TCategoryName).FullName ?? string.Empty, out var capturedLogStore)
      ? capturedLogStore
      : null;
  }

  /// <summary>
  ///   Retrieves a <see cref="ICapturedLogStore" /> from this <paramref name="logStore" /> by its
  ///   <see cref="Type" />-based category name.
  /// </summary>
  /// <param name="logStore">This log store instance.</param>
  /// <typeparam name="TCategoryName">
  ///   A logging category name. I.e., the type argument used by an
  ///   <see cref="ILogger{TCategoryName}" />.
  /// </typeparam>
  /// <returns>The <see cref="ICapturedLogStore" />.</returns>
  /// <exception cref="KeyNotFoundException">
  ///   Thrown when no <see cref="ICapturedLogStore" /> exists for
  ///   <typeparamref name="TCategoryName" />. This is expected when no messages have been logged for
  ///   <typeparamref name="TCategoryName" />.
  /// </exception>
  public static ICapturedLogStore GetCapturedLogStore<TCategoryName>(
    this IDictionary<string, ICapturedLogStore> logStore
  )
  {
    return logStore.TryGetCapturedLogStore<TCategoryName>() ?? throw new KeyNotFoundException(
      $"No captured log store exists for category {typeof(TCategoryName).FullName}."
    );
  }
}
