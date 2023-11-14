using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.EngineType
{
    public interface IMasterEngineTypeAppService : IApplicationService
    {
        Task<PagedResultDto<MasterEngineTypeDto>> GetEngineTypeSearch(GetMasterEngineTypeInput input);
    }
}
