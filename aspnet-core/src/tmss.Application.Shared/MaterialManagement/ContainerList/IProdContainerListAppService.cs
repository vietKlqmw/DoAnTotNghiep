using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.ContainerList
{
    public interface IProdContainerListAppService : IApplicationService
    {
        Task<PagedResultDto<ProdContainerListDto>> GetProdContainerListSearch(GetProdContainerListInput input);
    }
}
