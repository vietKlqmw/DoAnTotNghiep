using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.OrderPart
{
    public interface IProdOrderPartAppService : IApplicationService
    {
        Task<PagedResultDto<ProdOrderPartDto>> GetProdOrderPartSearch(GetProdOrderPartInput input);

        Task<List<ProdOrderPartImportDto>> ImportProdOrderPartFromExcel(byte[] fileBytes, string fileName);
    }
}
