﻿using OpenRefine.Net.Models;
using System.Threading.Tasks;

namespace OpenRefine.Net.Interfaces
{
    public interface IRefineClient
    {
        Task<GetCsrfTokenResponse> GetCsrfTokenAsyc();

        Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request);
        
        Task<GetProjectModelsResponse> GetProjectModelsAsync(GetProjectModelsRequest request);

        Task<ApplyOperationsResponse> ApplyOperationsAsync(ApplyOperationsRequest request);

        Task<string> ExportRowsAsync(ExportRowsRequest request);

        Task<DeleteProjectResponse> DeleteProjectAsync(DeleteProjectRequest request);

        Task<GetProcessesResponse> CheckStatusOfAsyncProcessesAsync(GetProcessesRequest request);

        Task<GetProjectsMetadataResponse> GetAllProjectsMetadataAsync(GetProjectsMetadataRequest request);

    }
}