using Microsoft.AspNetCore.Mvc;

namespace SimpleWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
  private readonly IExampleService _exampleService;
  private readonly ILogger<ExampleController> _logger;

  public ExampleController(IExampleService exampleService, ILogger<ExampleController> logger)
  {
    _exampleService = exampleService;
    _logger = logger;
  }

  [HttpGet]
  public async Task<IActionResult> Get(CancellationToken cancellationToken)
  {
    _logger.LogDebug("Getting value.");
    var value = await _exampleService.GetValueAsync(cancellationToken);
    _logger.LogInformation("Retrieved value. Value: {Value}", value);

    return Ok(value);
  }
}
