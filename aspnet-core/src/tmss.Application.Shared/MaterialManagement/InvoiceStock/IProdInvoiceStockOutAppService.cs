using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.InvoiceStock
{
    public interface IProdInvoiceStockOutAppService : IApplicationService
    {
        Task<PagedResultDto<ProdInvoiceStockOutDto>> GetProdInvoiceStockOutSearch(GetProdInvoiceStockOutInput input);
    }
}
