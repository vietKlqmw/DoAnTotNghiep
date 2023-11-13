using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.Engine
{
    public interface IMasterEngineAppService : IApplicationService
    {
        Task<PagedResultDto<MasterEngineDto>> GetEngineMasterSearch(GetMasterEngineInput input);
    }
}
