namespace SimpleWebApi;

/// <summary>
///   An example type which represents an implementation of application functionality. For example, it might implement a
///   version of <see cref="IExampleService" /> which gets the data from a remote API.
/// </summary>
/// <remarks>
///   <para>
///     It is expected that a type like this will be registered as an implementation/concrete type with dependency
///     resolution service providers, if such are used. E.g.,
///     <c>serviceCollection.AddTransient&lt;IExampleService,ExampleService&gt;()</c>.
///   </para>
/// </remarks>
public class ExampleService : IExampleService
{
  private readonly ILogger<ExampleService> _logger;

  /// <summary>
  ///   This constructor is important to this arrangement. Specifically, its use of <see cref="ILogger{TCategoryName}" />.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     In ASP.NET, the logging infrastructure provided by default registers <see cref="ILogger{TCategoryName}" /> with the
  ///     dependency resolution service provider. The non-generic <see cref="ILogger" /> is not registered.
  ///   </para>
  /// </remarks>
  public ExampleService(ILogger<ExampleService> logger)
  {
    _logger = logger;
  }

  public async Task<object> GetValueAsync(CancellationToken cancellationToken)
  {
    async Task<object> GetValueFromAsynchronousSource()
    {
      // This local function represents an asynchronous operation which returns the data expected by IExampleService implementations. E.g., a database query or API request.
      await Task.Delay(1, cancellationToken);
      return new object();
    }

    try
    {
      _logger.LogDebug("Beginning asynchronous value retrieval.");

      var result = await GetValueFromAsynchronousSource();

      _logger.LogInformation("Retrieved value.");

      return result;
    }
    catch (Exception exception)
    {
      _logger.LogError("Asynchronous value retrieval failed.", exception);
      throw;
    }
  }
}