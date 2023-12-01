using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.StockReceiving
{
    public interface IProdStockReceivingAppService : IApplicationService
    {
        Task<PagedResultDto<ProdStockReceivingDto>> GetProdStockReceivingSearch(GetProdStockReceivingInput input);
    }
}
