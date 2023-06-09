# CapturingLoggerProvider class

An ILoggerProvider which can be created with [`ICapturedLogStore`](./ICapturedLogStore.md)s to enable logged messages to be accessed.

```csharp
public class CapturingLoggerProvider : ILoggerProvider
```

## Public Members

| name | description |
| --- | --- |
| [CapturingLoggerProvider](CapturingLoggerProvider/CapturingLoggerProvider.md)(…) |  (2 constructors) |
| static [FromDictionary](CapturingLoggerProvider/FromDictionary.md)(…) | Creates a [`CapturingLoggerProvider`](./CapturingLoggerProvider.md) which uses *logStore* to maintain a separate [`ICapturedLogStore`](./ICapturedLogStore.md) for each logging `categoryName` (String) parameter). |
| [DefaultMinimumLogLevel](CapturingLoggerProvider/DefaultMinimumLogLevel.md) { get; set; } | Gets or sets the default [`MinimumLogLevel`](./CapturingLogger/MinimumLogLevel.md) for created ILogger instances. |
| [CreateLogger](CapturingLoggerProvider/CreateLogger.md)(…) |  |
| [Dispose](CapturingLoggerProvider/Dispose.md)() |  |

## See Also

* namespace [Jds.TestingUtils.Logging](../TestingUtils.Logging.md)
* [CapturingLoggerProvider.cs](https://github.com/JeremiahSanders/testingutils-logging/tree/main/src/CapturingLoggerProvider.cs)

<!-- DO NOT EDIT: generated by xmldocmd for TestingUtils.Logging.dll -->
