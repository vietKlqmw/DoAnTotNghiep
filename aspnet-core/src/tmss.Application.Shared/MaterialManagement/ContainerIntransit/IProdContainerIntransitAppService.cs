using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.ContainerIntransit
{
    public interface IProdContainerIntransitAppService : IApplicationService
    {
        Task<PagedResultDto<ProdContainerIntransitDto>> GetProdContainerIntransitSearch(GetProdContainerIntransitInput input);
    }
}
