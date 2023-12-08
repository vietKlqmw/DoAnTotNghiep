using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.CustomsDeclare
{
    public interface IProdCustomsDeclareAppService : IApplicationService
    {
        Task<PagedResultDto<ProdCustomsDeclareDto>> GetProdCustomsDeclareSearch(GetProdCustomsDeclareInput input);
    }
}
