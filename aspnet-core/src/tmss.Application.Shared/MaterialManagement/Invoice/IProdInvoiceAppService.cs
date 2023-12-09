using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.Invoice
{
    public interface IProdInvoiceAppService : IApplicationService
    {
        Task<PagedResultDto<ProdInvoiceDto>> GetProdInvoiceSearch(GetProdInvoiceInput input);
        Task<PagedResultDto<ProdInvoiceDto>> GetProdInvoiceDetailsSearch(GetProdInvoiceDetailsInput input);
    }
}
