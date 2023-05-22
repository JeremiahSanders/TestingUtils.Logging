using SimpleWebApi;

namespace Jds.TestingUtils.Logging.Tests.Unit.Examples.UseCases;

/// <summary>
///   An example test use case which tests a type which depends upon <see cref="ILogger{TCategoryName}" />.
/// </summary>
/// <remarks>
///   <para>
///     In this arrangement we are testing an implementation of an interface which performs an asynchronous operation.
///     Specifically, we test whether it logs a few expected messages.
///   </para>
///   <para>
///     This example represents a common pattern. Some examples: ASP.NET <c>Controller</c>s, classes using Entity
///     Framework <c>DbContext</c>, request handlers when using a mediator pattern.
///   </para>
///   <para>
///     In a real project tests would also exist to validate the return value; those are omitted for clarity.
///   </para>
/// </remarks>
public static class ILoggerExamples
{
  /// <summary>
  ///   An example xUnit test class.
  /// </summary>
  public class ExampleServiceTests
  {
    private readonly CapturingLogger<ExampleService> _logger;
    private readonly IExampleService _systemUnderTest;

    public ExampleServiceTests()
    {
      // Arrange
      _logger = new CapturingLogger<ExampleService>();
      _systemUnderTest = new ExampleService(_logger);
    }

    [Fact]
    public async Task GetValueAsync_WhenSuccessful_LogsBeginning()
    {
      // Act
      _ = await _systemUnderTest.GetValueAsync(default(CancellationToken));

      // Assert
      Assert.Contains(_logger.Logs, log => log.Message == "Beginning asynchronous value retrieval.");
    }

    [Fact]
    public async Task GetValueAsync_WhenSuccessful_LogsCompletion()
    {
      // Act
      _ = await _systemUnderTest.GetValueAsync(default(CancellationToken));

      // Assert
      Assert.Contains(_logger.Logs, log => log.Message == "Retrieved value.");
    }

    [Fact]
    public async Task GetValueAsync_WhenFailure_LogsError()
    {
      // Arrange
      var provider = new CancellationTokenSource();
      provider.Cancel();

      try
      {
        // Act + Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => _systemUnderTest.GetValueAsync(provider.Token));
      }
      catch (Exception exception)
      {
        // Assert
        Assert.Contains(_logger.Logs, log => log.LogLevel == LogLevel.Error && log.Exception == exception);
      }
    }

    [Fact]
    public async Task GetValueAsync_WhenFailure_LogsBeginning()
    {
      // Arrange
      var provider = new CancellationTokenSource();
      provider.Cancel();

      try
      {
        // Act + Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => _systemUnderTest.GetValueAsync(provider.Token));
      }
      catch (Exception exception)
      {
        // Assert
        Assert.Contains(_logger.Logs, log => log.Message == "Beginning asynchronous value retrieval.");
      }
    }
  }

}
