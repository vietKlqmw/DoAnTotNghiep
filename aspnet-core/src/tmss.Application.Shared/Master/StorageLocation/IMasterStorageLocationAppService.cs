using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.StorageLocation
{
    public interface IMasterStorageLocationAppService : IApplicationService
    {
        Task<PagedResultDto<MasterStorageLocationDto>> GetStorageLocationSearch(GetMasterStorageLocationInput input);
    }
}
