using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.ContainerIntransit
{
    public interface IProdContainerIntransitAppService : IApplicationService
    {
        Task<PagedResultDto<ProdContainerIntransitDto>> GetProdContainerIntransitSearch(GetProdContainerIntransitInput input);

        Task<List<ProdContainerIntransitImportDto>> ImportProdContainerIntransitFromExcel(byte[] fileBytes, string fileName);
    }
}
