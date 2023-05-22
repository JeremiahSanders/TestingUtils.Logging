# TestingUtils.Logging ([NuGet][nuget-repository])

Methods and types supporting the use of [`Microsoft.Extensions.Logging.Abstractions`][] (e.g., `ILogger` and `ILogger<>`) within unit and integration tests.

* [API Documentation][]

## Usage Examples

### "Service" using `ILogger<>`

In this example, an application "service" performs logging of its internal work. It requires an `ILogger<>` in its constructor.

> Note that the term "service" here is intentionally loose. It is intended only to be a type which performs a unit of work or presents a required interface.
>
> In a production implementation, a similar type might have I/O dependencies (e.g., an Entity Framework `DbContext`, or an `HttpClient`). Alternately, they may require logging because they perform logic or handle requests which must be captured, e.g., for triage or compliance.
>
> When identifying if this pattern fits a use case, common applicable type name suffixes: `*Service`, `*Repository`, `*Handler`

**This example is centered around capturing logged messages which occur during [unit testing][].**

* [Example service interface under test][example-service-interface]
* [Example service implementation (the _system under test_)][example-service-sut]
* [Example unit tests][example-service-tests]

### ASP.NET Core API `Controller` using `ILogger<>`

In this example, an ASP.NET Core API `Controller` performs logging of its request processing. This example builds upon the service example, above.

**This example is centered around capturing logged messages which occur during [ASP.NET Core integration testing][].**

* [Example API `Controller`][example-api-controller]
* [Example integration tests][example-api-tests-integration]

[`Microsoft.Extensions.Logging.Abstractions`]: https://www.nuget.org/packages/Microsoft.Extensions.Logging.Abstractions
[API Documentation]: https://github.com/JeremiahSanders/TestingUtils.Logging/blob/dev/docs/api/TestingUtils.Logging.md
[ASP.NET Core integration testing]: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests
[example-api-controller]: https://github.com/JeremiahSanders/TestingUtils.Logging/blob/dev/examples/example-webapi/Controllers/ExampleController.cs
[example-api-tests-integration]: https://github.com/JeremiahSanders/TestingUtils.Logging/blob/dev/tests/unit/Examples/UseCases/ILoggerFactoryExamples.cs
[example-service-interface]: https://github.com/JeremiahSanders/TestingUtils.Logging/blob/dev/examples/example-webapi/IExampleService.cs
[example-service-sut]: https://github.com/JeremiahSanders/TestingUtils.Logging/blob/dev/examples/example-webapi/ExampleService.cs
[example-service-tests]: https://github.com/JeremiahSanders/TestingUtils.Logging/blob/dev/tests/unit/Examples/UseCases/ILoggerExamples.cs
[nuget-repository]: https://www.nuget.org/packages/Jds.TestingUtils.Logging
[unit testing]: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test
