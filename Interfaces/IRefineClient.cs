using OpenRefine.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        Task<GetProjectMetadataResponse> GetAllProjectMetadataAsync(GetProjectMetadataRequest request);


    }
}
