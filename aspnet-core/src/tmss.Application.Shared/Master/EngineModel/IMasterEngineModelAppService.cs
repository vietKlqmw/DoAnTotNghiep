using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.EngineModel
{
    public interface IMasterEngineModelAppService : IApplicationService
    {
        Task<PagedResultDto<MasterEngineModelDto>> GetEngineModelSearch(GetMasterEngineModelInput input);
    }
}
