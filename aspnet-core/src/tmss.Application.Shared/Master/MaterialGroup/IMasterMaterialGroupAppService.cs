using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.MaterialGroup
{
    public interface IMasterMaterialGroupAppService : IApplicationService
    {
        Task<PagedResultDto<MasterMaterialGroupDto>> GetMaterialGroupSearch(GetMasterMaterialGroupInput input);
    }
}
