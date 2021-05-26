using OpenRefine.Net.Interfaces;
using OpenRefine.Net.Models;
using System.IO;
using Xunit;

namespace OpenRefine.Net.Tests
{
    public class OpenRefineClientTests
    {
        private readonly IRefineClient _client;

        public OpenRefineClientTests()
        {
            _client = new OpenRefineClient();
        }

        [Fact]
        public async void OpenRefineClient_GetCsrfToken_ShouldReturn_Token()
        {
            var csrf = await _client.GetCsrfTokenAsyc();

            Assert.NotEmpty(csrf.Token);
        }


        [Fact]
        public async void OpenRefineClient_CreateProject_ShouldReturn_ProjectId()
        {
            var fileInfo = new FileInfo("Samples/dates.txt");

            var content = new byte[fileInfo.Length];

            using var fs = fileInfo.OpenRead();
            await fs.ReadAsync(content);

            var csrf = await _client.GetCsrfTokenAsyc();

            var project = await _client.CreateProjectAsync(new CreateProjectRequest
            {
                Token = csrf.Token,
                ProjectName = fileInfo.Name,
                FileName = fileInfo.Name,
                Content = content
            });

            Assert.NotEmpty(project.ProjectId);
        }

        [Fact]
        public async void OpenRefineClient_GetProjectModelsAsync_ShouldReturn_ProjectInfo()
        {
            var fileInfo = new FileInfo("Samples/dates.txt");

            var content = new byte[fileInfo.Length];

            using var fs = fileInfo.OpenRead();
            await fs.ReadAsync(content);

            var csrf = await _client.GetCsrfTokenAsyc();

            var project = await _client.CreateProjectAsync(new CreateProjectRequest
            {
                Token = csrf.Token,
                ProjectName = fileInfo.Name,
                FileName = fileInfo.Name,
                Content = content
            });

            var models = await _client.GetProjectModelsAsync(new GetProjectModelsRequest { 
                Token = csrf.Token,
                ProjectId = project.ProjectId
            });

            Assert.NotNull(models.ColumnModel);
        }

        [Fact]
        public async void OpenRefineClient_ApplyOperationsAsync_ShouldReturn_ModifyProjectContent() 
        {
            var fileInfo = new FileInfo("Samples/dates.txt");

            var content = new byte[fileInfo.Length];

            using var fs = fileInfo.OpenRead();
            await fs.ReadAsync(content);

            var csrf = await _client.GetCsrfTokenAsyc();

            var project = await _client.CreateProjectAsync(new CreateProjectRequest
            {
                Token = csrf.Token,
                ProjectName = fileInfo.Name,
                FileName = fileInfo.Name,
                Content = content,
                Format = SupportedFormats.TextLineBasedSeparatorBased
            });

            var operations = File.ReadAllText("Samples/operations.json");

            var appliedOps = await _client.ApplyOperationsAsync(new ApplyOperationsRequest
            {
                Token = csrf.Token,
                ProjectId = project.ProjectId,
                Operations = operations
            });

            Assert.Equal("ok", appliedOps.Code.ToLower());
        }

        [Fact]
        public async void OpenRefineClient_ExportRowsAsync_ShouldDownload_Content() 
        {
            var fileInfo = new FileInfo("Samples/dates.txt");

            var content = new byte[fileInfo.Length];

            using var fs = fileInfo.OpenRead();
            await fs.ReadAsync(content);

            var csrf = await _client.GetCsrfTokenAsyc();

            var project = await _client.CreateProjectAsync(new CreateProjectRequest
            {
                Token = csrf.Token,
                ProjectName = fileInfo.Name,
                FileName = fileInfo.Name,
                Content = content,
                Format = SupportedFormats.TextLineBasedSeparatorBased
            });

            var fileName = await _client.ExportRowsAsync(new ExportRowsRequest { 
                Token = csrf.Token,
                ProjectId = project.ProjectId,
                FileName = "test.csv"
            });

            var results = File.ReadAllText(fileName);

            Assert.NotEmpty(results);

            File.Delete(fileName);
        }

        [Fact]
        public async void OpenRefineClient_DeleteProjectAsync_ShouldDeleteProject() 
        {
            var fileInfo = new FileInfo("Samples/dates.txt");

            var content = new byte[fileInfo.Length];

            using var fs = fileInfo.OpenRead();
            await fs.ReadAsync(content);

            var csrf = await _client.GetCsrfTokenAsyc();

            var project = await _client.CreateProjectAsync(new CreateProjectRequest
            {
                Token = csrf.Token,
                ProjectName = fileInfo.Name,
                FileName = fileInfo.Name,
                Content = content,
                Format = SupportedFormats.TextLineBasedSeparatorBased
            });

            var deleted = await _client.DeleteProjectAsync(new DeleteProjectRequest {
                Token = csrf.Token,
                ProjectId = project.ProjectId
            });

            Assert.Equal("ok", deleted.Code.ToLower());
        }

        [Fact]
        public async void OpenRefineClient_GetAllProjectMetadataAsync_ShouldReturnProjectMetadata() 
        {
            var fileInfo = new FileInfo("Samples/dates.txt");

            var content = new byte[fileInfo.Length];

            using var fs = fileInfo.OpenRead();
            await fs.ReadAsync(content);

            var csrf = await _client.GetCsrfTokenAsyc();

            var project = await _client.CreateProjectAsync(new CreateProjectRequest
            {
                Token = csrf.Token,
                ProjectName = fileInfo.Name,
                FileName = fileInfo.Name,
                Content = content,
                Format = SupportedFormats.TextLineBasedSeparatorBased
            });

            var metadata = await _client.GetAllProjectsMetadataAsync(new GetProjectsMetadataRequest { 
                Token = csrf.Token,
                ProjectId = project.ProjectId
            });

            Assert.True(metadata.Projects.ContainsKey(project.ProjectId));
        }

        [Fact]
        public async void OpenRefineClient_CheckStatusOfAsyncProcessesAsync_ShouldReturnOk() 
        {
            var fileInfo = new FileInfo("Samples/dates.txt");

            var content = new byte[fileInfo.Length];

            using var fs = fileInfo.OpenRead();
            await fs.ReadAsync(content);

            var csrf = await _client.GetCsrfTokenAsyc();

            var project = await _client.CreateProjectAsync(new CreateProjectRequest
            {
                Token = csrf.Token,
                ProjectName = fileInfo.Name,
                FileName = fileInfo.Name,
                Content = content,
                Format = SupportedFormats.TextLineBasedSeparatorBased
            });

            var processes = await _client.CheckStatusOfAsyncProcessesAsync(new GetProcessesRequest { 
                Token = csrf.Token,
                ProjectId = project.ProjectId
            });

            Assert.NotNull(processes.Processes);
        }
    }
}
