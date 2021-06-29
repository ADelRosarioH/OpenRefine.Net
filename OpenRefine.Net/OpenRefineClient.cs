using Flurl;
using Newtonsoft.Json;
using OpenRefine.Net.Interfaces;
using OpenRefine.Net.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
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

        public async Task<ApplyOperationsResponse> ApplyOperationsAsync(ApplyOperationsRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/apply-operations");
            requestUri.QueryParams.Add("csrf_token", request.Token);
            requestUri.QueryParams.Add("project", request.ProjectId);

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("operations", request.Operations),
            });

            try
            {
                var responseMessage = await _httpClient.PostAsync(requestUri, content, cancellationToken);
                var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                
                responseMessage.EnsureSuccessStatusCode();

                var response = JsonConvert.DeserializeObject<ApplyOperationsResponse>(responseString);

                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetProcessesResponse> CheckStatusOfAsyncProcessesAsync(GetProcessesRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/get-processes");
            requestUri.QueryParams.Add("csrf_token", request.Token);
            requestUri.QueryParams.Add("project", request.ProjectId);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Get
            };

            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
                var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

                responseMessage.EnsureSuccessStatusCode();

                var response = JsonConvert.DeserializeObject<GetProcessesResponse>(responseString);

                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/create-project-from-upload");
            requestUri.QueryParams.Add("csrf_token", request.Token);

            var multiPartForm = new MultipartFormDataContent();

            string options = request.Options != null ? JsonConvert.SerializeObject(request.Options) : string.Empty;

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

            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
                var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

                responseMessage.EnsureSuccessStatusCode();
                
                var responseUri = new Url(responseMessage.RequestMessage.RequestUri.ToString());

                var (name, val) = responseUri.QueryParams.FirstOrDefault(q => q.Name == "project");

                string projectId = val?.ToString();

                if (string.IsNullOrEmpty(projectId))
                    return JsonConvert.DeserializeObject<CreateProjectResponse>(responseString);

                return new CreateProjectResponse {
                    ProjectId = projectId
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<DeleteProjectResponse> DeleteProjectAsync(DeleteProjectRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/delete-project");
            requestUri.QueryParams.Add("csrf_token", request.Token);
            requestUri.QueryParams.Add("project", request.ProjectId);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Post
            };

            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
                var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                
                responseMessage.EnsureSuccessStatusCode();

                var response = JsonConvert.DeserializeObject<DeleteProjectResponse>(responseString);

                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> ExportRowsAsync(ExportRowsRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/export-rows");
            requestUri.QueryParams.Add("csrf_token", request.Token);

            requestUri.QueryParams.Add("project", request.ProjectId);
            requestUri.QueryParams.Add("format", request.Format);

            var multiPartForm = new MultipartFormDataContent();
            multiPartForm.Add(new StringContent(request.Engine), "engine");

            try
            {
                var fileInfo = new FileInfo(request.FileName);

                var responseMessage = await _httpClient.PostAsync(requestUri, multiPartForm, cancellationToken);
                responseMessage.EnsureSuccessStatusCode();

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

            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
                var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

                responseMessage.EnsureSuccessStatusCode();

                var response = JsonConvert.DeserializeObject<GetProjectsMetadataResponse>(responseString);

                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetCsrfTokenResponse> GetCsrfTokenAsyc(CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/get-csrf-token");

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Get
            };

            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
                var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                
                responseMessage.EnsureSuccessStatusCode();

                var response = JsonConvert.DeserializeObject<GetCsrfTokenResponse>(responseString);

                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GetProjectModelsResponse> GetProjectModelsAsync(GetProjectModelsRequest request, CancellationToken cancellationToken = default)
        {
            Url requestUri = new Url("command/core/get-models");
            requestUri.QueryParams.Add("csrf_token", request.Token);
            requestUri.QueryParams.Add("project", request.ProjectId);

            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(requestUri, UriKind.Relative),
                Method = HttpMethod.Get
            };

            try
            {
                var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);
                var responseString = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                
                responseMessage.EnsureSuccessStatusCode();

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
