using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.OrderPart
{
    public interface IProdOrderPartAppService : IApplicationService
    {
        Task<PagedResultDto<ProdOrderPartDto>> GetProdOrderPartSearch(GetProdOrderPartInput input);
    }
}
