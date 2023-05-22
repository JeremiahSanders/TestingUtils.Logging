using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc.Testing;
using SimpleWebApi;
using SimpleWebApi.Controllers;

namespace Jds.TestingUtils.Logging.Tests.Unit.Examples.UseCases;

public static class ILoggerFactoryExamples
{
  public class ProgramFactoryTests
  {
    private readonly IReadOnlyDictionary<string, ICapturedLogStore> _logCache;
    private readonly HttpClient _programHttpClient;
    private readonly IExampleService _serviceWhichLogs;
    private readonly ExampleLogCapturingWebApplicationFactory<Program> _testWebApplicationFactory;

    public ProgramFactoryTests()
    {
      _testWebApplicationFactory = new ExampleLogCapturingWebApplicationFactory<Program>();
      _programHttpClient = _testWebApplicationFactory.CreateClient();

      _serviceWhichLogs = _testWebApplicationFactory.Services.GetRequiredService<IExampleService>();
      _logCache = _testWebApplicationFactory.GetLogStoreCache(); // This method is defined below.
    }

    [Fact]
    public async Task WhenExecutingAResolvedServiceWhichLogs_LoggedMessagesAreStoredInCache()
    {
      // Act
      _ = await _serviceWhichLogs.GetValueAsync(default(CancellationToken));

      // Assert
      //   This works because the service implementation, ExampleService, uses an ILogger<ExampleService>.
      //   As a result, the LoggerFactory categories its messages with "SimpleWebApi.ExampleService" (the generic type argument).
      var capturedLogStore = _logCache.GetCapturedLogStore<ExampleService>();

      //   This asserts that we logged a particular message.
      Assert.Contains(capturedLogStore, log => log.Message == "Beginning asynchronous value retrieval.");
    }

    [Fact]
    public async Task WhenExecutingAnApiWhichLogs_DependenciesLoggedMessagesAreStoredInCache()
    {
      // Act
      _ = await _programHttpClient.GetAsync("Example");

      // Assert
      //   This works because the service implementation, ExampleService, uses an ILogger<ExampleService>.
      //   As a result, the LoggerFactory categories its messages with "SimpleWebApi.ExampleService" (the generic type argument).
      var capturedLogStore = _logCache.GetCapturedLogStore<ExampleService>();
      //   This asserts that we logged a particular message.
      Assert.Contains(capturedLogStore, log => log.Message == "Beginning asynchronous value retrieval.");
    }

    [Fact]
    public async Task WhenExecutingAnApiWhichLogs_ControllersLoggedMessagesAreStoredInCache()
    {
      // Act
      _ = await _programHttpClient.GetAsync("Example");

      // Assert
      //   This works because the controller implementation, ExampleController, uses an ILogger<ExampleController>.
      //   As a result, the LoggerFactory categories its messages with "SimpleWebApi.ExampleController" (the generic type argument).
      var capturedLogStore = _logCache.GetCapturedLogStore<ExampleController>();
      //   This asserts that we logged a particular message.
      Assert.Contains(capturedLogStore, log => log.Message == "Retrieved value. Value: System.Object");
    }
  }

  /// <summary>
  ///   This is an example <see cref="WebApplicationFactory{TEntryPoint}" /> which
  ///   captures all logs to an in-memory <see cref="ConcurrentDictionary{TKey,TValue}" />.
  /// </summary>
  public class ExampleLogCapturingWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
    where TEntryPoint : class
  {
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      // Required: Configure test application's services to capture logs.
      //   This is the primary configuration for capturing logs from a web API within integration tests.
      builder.ConfigureServices((context, collection) =>
      {
        // Create a log store cache.
        var storeDictionary = new ConcurrentDictionary<string, ICapturedLogStore>();
        // Register the log store cache as a singleton with the application's service provider. This allows us to access the messages later.
        collection.AddSingleton<IReadOnlyDictionary<string, ICapturedLogStore>>(storeDictionary);
        // Add logging and configure logging to use the CapturingLoggerProvider 
        collection.AddLogging(loggingBuilder =>
        {
          loggingBuilder.AddProvider(CapturingLoggerProvider.FromDictionary(storeDictionary));
        });
      });
    }

    /// <summary>
    ///   Get the cached log stores.
    /// </summary>
    public IReadOnlyDictionary<string, ICapturedLogStore> GetLogStoreCache()
    {
      // This relies upon the configuration performed above.
      // The ConcurrentDictionary<string, ICapturedLogStore> was registered as IReadOnlyDictionary<string, ICapturedLogStore> in Singleton lifetime scope.
      return Services.GetRequiredService<IReadOnlyDictionary<string, ICapturedLogStore>>();
    }
  }
}
