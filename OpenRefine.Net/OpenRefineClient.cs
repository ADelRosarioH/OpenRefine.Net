using Flurl;
using OpenRefine.Net.Interfaces;
using OpenRefine.Net.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenRefine.Net
{
    public class OpenRefineClient : IRefineClient
    {
        private readonly HttpClient _httpClient;

        public OpenRefineClient(string baseUrl = "http://127.0.0.1:3333/")
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public OpenRefineClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApplyOperationsResponse> ApplyOperationsAsync(ApplyOperationsRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/apply-operations");
            requestUri.QueryParams.Add("csrf_token", request.CsrfToken);
            requestUri.QueryParams.Add("project", request.ProjectId);

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("operations", request.Operations),
            });

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Post,
                Content = content
            };

            return await SendAsync<ApplyOperationsResponse>(requestMessage, cancellationToken);
        }

        public async Task<GetProcessesResponse> CheckStatusOfAsyncProcessesAsync(GetProcessesRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/get-processes");
            requestUri.QueryParams.Add("csrf_token", request.CsrfToken);
            requestUri.QueryParams.Add("project", request.ProjectId);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Get
            };

            return await SendAsync<GetProcessesResponse>(requestMessage, cancellationToken);
        }

        public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/create-project-from-upload");
            requestUri.QueryParams.Add("csrf_token", request.CsrfToken);

            var multiPartForm = new MultipartFormDataContent();

            string options = request.Options is not null
                            ? JsonSerializer.Serialize(request.Options)
                            : string.Empty;

            multiPartForm.Add(new ByteArrayContent(request.Content), "project-file", request.FileName);
            multiPartForm.Add(new StringContent(request.ProjectName), "project-name");

            if (!string.IsNullOrEmpty(request.Format))
                multiPartForm.Add(new StringContent(request.Format), "format");

            if (!string.IsNullOrEmpty(options))
                multiPartForm.Add(new StringContent(options), "options");

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Post,
                Content = multiPartForm
            };

            var responseMessage = await SendAsync(requestMessage, cancellationToken);
            var responseUri = new Url(responseMessage.RequestMessage.RequestUri.ToString());

            if (responseUri.QueryParams.TryGetFirst("project", out var projectIdObject))
            {
                string projectId = projectIdObject.ToString();
                return new CreateProjectResponse
                {
                    ProjectId = projectId
                };
            }

            var responseString = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CreateProjectResponse>(responseString);
        }

        public async Task<DeleteProjectResponse> DeleteProjectAsync(DeleteProjectRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/delete-project");
            requestUri.QueryParams.Add("csrf_token", request.CsrfToken);
            requestUri.QueryParams.Add("project", request.ProjectId);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Post
            };

            return await SendAsync<DeleteProjectResponse>(requestMessage, cancellationToken);
        }

        public async Task<string> ExportRowsAsync(ExportRowsRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/export-rows");
            requestUri.QueryParams.Add("csrf_token", request.CsrfToken);

            requestUri.QueryParams.Add("project", request.ProjectId);
            requestUri.QueryParams.Add("format", request.Format);

            var multiPartForm = new MultipartFormDataContent();
            multiPartForm.Add(new StringContent(request.Engine), "engine");

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Post,
                Content = multiPartForm
            };

            var responseMessage = await SendAsync(requestMessage, cancellationToken);

            try
            {
                var fileInfo = new FileInfo(request.FileName);

                await using var ms = await responseMessage.Content.ReadAsStreamAsync(cancellationToken);
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

        public async Task<GetProjectsMetadataResponse> GetAllProjectsMetadataAsync(GetProjectsMetadataRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/get-all-project-metadata");

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Get
            };

            return await SendAsync<GetProjectsMetadataResponse>(requestMessage, cancellationToken);
        }

        public async Task<GetCsrfTokenResponse> GetCsrfTokenAsync(CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/get-csrf-token");

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Get
            };

            return await SendAsync<GetCsrfTokenResponse>(requestMessage, cancellationToken);
        }

        public async Task<GetProjectModelsResponse> GetProjectModelsAsync(GetProjectModelsRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/get-models");
            requestUri.QueryParams.Add("csrf_token", request.CsrfToken);
            requestUri.QueryParams.Add("project", request.ProjectId);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Get
            };

            return await SendAsync<GetProjectModelsResponse>(requestMessage, cancellationToken);
        }

        private async Task<T> SendAsync<T>(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
            where T : class
        {
            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
                var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

                responseMessage.EnsureSuccessStatusCode();

                var response = JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return response;
            }
            catch
            {
                throw;
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);

                responseMessage.EnsureSuccessStatusCode();

                return responseMessage;
            }
            catch
            {
                throw;
            }
        }
    }
}
