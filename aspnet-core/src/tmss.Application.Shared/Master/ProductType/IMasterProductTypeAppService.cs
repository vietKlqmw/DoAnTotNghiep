using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.ProductType
{
    public interface IMasterProductTypeAppService : IApplicationService
    {
        Task<PagedResultDto<MasterProductTypeDto>> GetProductTypeSearch(GetMasterProductTypeInput input);
    }
}
