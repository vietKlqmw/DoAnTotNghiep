using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.BillOfLading
{
    public interface IProdBillOfLadingAppService : IApplicationService
    {
        Task<PagedResultDto<ProdBillOfLadingDto>> GetProdBillOfLadingSearch(GetProdBillOfLadingInput input);
    }
}
