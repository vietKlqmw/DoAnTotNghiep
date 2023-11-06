using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.ProductGroup
{
    public interface IMasterProductGroupAppService : IApplicationService
    {
        Task<PagedResultDto<MasterProductGroupDto>> GetProductGroupSearch(GetMasterProductGroupInput input);
    }
}
