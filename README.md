# OpenRefine.Net
 
A .NET client implementation for the OpenRefine API. Build and Tested with OpenRefine v3.4.1.

Supported features:

- Create project
- Get project models
- Apply operations
- Export tows
- Delete project
- Get all projects metadata
- Check status of async processes

## Configuration
By default the OpenRefine server URL is http://127.0.0.1:3333 or you can provide a HttpClient in the constructor.

```csharp
var _client = new OpenRefineClient(); // new OpenRefineClient("YOUR_OPENREFINE_URL"); or new OpenRefineClient(_customHttpClient);

var fileInfo = new FileInfo("Samples/dates.txt");

var content = new byte[fileInfo.Length];

using var fs = fileInfo.OpenRead();
await fs.ReadAsync(content);

var project = await _client.CreateProjectAsync(new CreateProjectRequest
{
    ProjectName = fileInfo.Name,
    FileName = fileInfo.Name,
    Content = content
});

var operations = File.ReadAllText("Samples/operations.json");

var appliedOps = await _client.ApplyOperationsAsync(new ApplyOperationsRequest
{
    ProjectId = project.ProjectId,
    Operations = operations
});

var fileName = await _client.ExportRowsAsync(new ExportRowsRequest { 
    ProjectId = project.ProjectId,
    FileName = "test.csv"
});

var results = File.ReadAllText(fileName);

var deleted = await _client.DeleteProjectAsync(new DeleteProjectRequest {
    ProjectId = project.ProjectId
});

Console.WriteLine(results);
```

## Installation

Clone this repository or use the [NuGet](https://www.nuget.org/packages/OpenRefine.Net/) package.

## Tests

You must have OpenRefine running at http://localhost:3333.

```sh
> dotnet test
```

## Contribute

Pull requests with passing tests are welcome! 

