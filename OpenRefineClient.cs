using Flurl;
using Newtonsoft.Json;
using OpenRefine.Net.Interfaces;
using OpenRefine.Net.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenRefine.Net
{
    public class OpenRefineClient : IRefineClient
    {
        private readonly HttpClient _httpClient;

        public OpenRefineClient(string baseUrl = "http://127.0.0.1:3333/")
        {
            _httpClient = new HttpClient() { 
                BaseAddress = new Uri(baseUrl)
            };
        }

        public async Task<ApplyOperationsResponse> ApplyOperationsAsync(ApplyOperationsRequest request)
        {
            Url requestUri = new Url("command/core/apply-operations");
            requestUri.QueryParams.Add("project", request.ProjectId);

            var multiPartForm = new MultipartFormDataContent();
            multiPartForm.Add(new StringContent(request.Operations), "operations");

            try
            {
                var responseMessage = await _httpClient.PostAsync(requestUri, multiPartForm);
                responseMessage.EnsureSuccessStatusCode();

                var responseString = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<ApplyOperationsResponse>(responseString);

                return response;
            }
            catch
            {
                throw;
            }
        }

        public Task<GetProcessesResponse> CheckStatusOfAsyncProcessesAsync(GetProcessesRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request)
        {
            const string requestUri = "command/core/create-project-from-upload";

            var multiPartForm = new MultipartFormDataContent();

            multiPartForm.Add(new StringContent(request.ProjectName), "project-name");
            multiPartForm.Add(new ByteArrayContent(request.FileContent), "project-file");
            multiPartForm.Add(new StringContent(request.Format), "format");
            multiPartForm.Add(new StringContent(JsonConvert.SerializeObject(request.Options)), "options");

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Post,
                Content = multiPartForm
            };

            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage);
                responseMessage.EnsureSuccessStatusCode();
                
                var responseUri = new Url(responseMessage.RequestMessage.RequestUri.ToString());

                var (name, val) = responseUri.QueryParams.FirstOrDefault(q => q.Name == "project");

                return new CreateProjectResponse {
                    ProjectId = val?.ToString()
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<DeleteProjectResponse> DeleteProjectAsync(DeleteProjectRequest request)
        {
            Url requestUri = new Url("command/core/delete-project");
            requestUri.QueryParams.Add("project", request.ProjectId);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Get
            };

            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage);
                responseMessage.EnsureSuccessStatusCode();

                var responseString = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<DeleteProjectResponse>(responseString);

                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> ExportRowsAsync(ExportRowsRequest request)
        {
            Url requestUri = new Url("command/core/export-rows");
            requestUri.QueryParams.Add("project", request.ProjectId);
            requestUri.QueryParams.Add("format", request.Format);

            var multiPartForm = new MultipartFormDataContent();
            multiPartForm.Add(new StringContent(request.Engine), "engine");

            try
            {
                var fileInfo = new FileInfo(request.DownloadPath);

                var responseMessage = await _httpClient.PostAsync(requestUri, multiPartForm);
                responseMessage.EnsureSuccessStatusCode();

                await using var ms = await responseMessage.Content.ReadAsStreamAsync();
                await using var fs = File.Create(fileInfo.FullName);
                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(fs);

                return fileInfo.FullName;
            }
            catch
            {
                throw;
            }
        }

        public Task<GetProjectMetadataResponse> GetAllProjectMetadataAsync(GetProjectMetadataRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<GetProjectModelsResponse> GetProjectModelsAsync(GetProjectModelsRequest request)
        {
            Url requestUri = new Url("command/core/get-models");
            requestUri.QueryParams.Add("project", request.ProjectId);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Get
            };

            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage);
                responseMessage.EnsureSuccessStatusCode();

                var responseString = await responseMessage.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<GetProjectModelsResponse>(responseString);

                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}
