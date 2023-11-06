using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.MaterialType
{
    public interface IMasterMaterialTypeAppService : IApplicationService
    {
        Task<PagedResultDto<MasterMaterialTypeDto>> GetMaterialTypeSearch(GetMasterMaterialTypeInput input);
    }
}
