using OpenRefine.Net.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OpenRefine.Net.Interfaces
{
    public interface IRefineClient
    {
        Task<GetCsrfTokenResponse> GetCsrfTokenAsync(CancellationToken cancellationToken = default);

        Task<CreateProjectResponse> CreateProjectAsync(CreateProjectRequest request, CancellationToken cancellationToken = default);
        
        Task<GetProjectModelsResponse> GetProjectModelsAsync(GetProjectModelsRequest request, CancellationToken cancellationToken = default);

        Task<ApplyOperationsResponse> ApplyOperationsAsync(ApplyOperationsRequest request, CancellationToken cancellationToken = default);

        Task<string> ExportRowsAsync(ExportRowsRequest request, CancellationToken cancellationToken = default);

        Task<DeleteProjectResponse> DeleteProjectAsync(DeleteProjectRequest request, CancellationToken cancellationToken = default);

        Task<GetProcessesResponse> CheckStatusOfAsyncProcessesAsync(GetProcessesRequest request, CancellationToken cancellationToken = default);

        Task<GetProjectsMetadataResponse> GetAllProjectsMetadataAsync(GetProjectsMetadataRequest request, CancellationToken cancellationToken = default);

    }
}
