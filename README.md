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
By default the OpenRefine server URL is http://127.0.0.1:3333.

```csharp
var _client = new OpenRefineClient(); // new OpenRefineClient("YOUR_OPENREFINE_URL");

var fileInfo = new FileInfo("Samples/dates.txt");

var content = new byte[fileInfo.Length];

using var fs = fileInfo.OpenRead();
await fs.ReadAsync(content);

var csrf = await _client.GetCsrfTokenAsync();

var project = await _client.CreateProjectAsync(new CreateProjectRequest
{
    Token = csrf.Token,
    ProjectName = fileInfo.Name,
    FileName = fileInfo.Name,
    Content = content
});

var operations = File.ReadAllText("Samples/operations.json");

var appliedOps = await _client.ApplyOperationsAsync(new ApplyOperationsRequest
{
    Token = csrf.Token,
    ProjectId = project.ProjectId,
    Operations = operations
});

var fileName = await _client.ExportRowsAsync(new ExportRowsRequest { 
    Token = csrf.Token,
    ProjectId = project.ProjectId,
    FileName = "test.csv"
});

var results = File.ReadAllText(fileName);

var deleted = await _client.DeleteProjectAsync(new DeleteProjectRequest {
    Token = csrf.Token,
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

