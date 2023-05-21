namespace SimpleWebApi;

/// <summary>
///   An example type which represents a part of an application's functionality. For example, to perform an I/O operation.
/// </summary>
/// <remarks>
///   <para>
///     It is expected that a type like this will be registered as a requested/registered/abstract type with
///     dependency resolution service providers, if such are used. E.g.,
///     <c>serviceCollection.AddTransient&lt;IExampleService,ExampleService&gt;()</c>.
///   </para>
/// </remarks>
public interface IExampleService
{
  /// <summary>
  ///   An example method which is valuable to application functionality. For example, it might obtain data from a REST API.
  /// </summary>
  Task<object> GetValueAsync(CancellationToken cancellationToken);
}