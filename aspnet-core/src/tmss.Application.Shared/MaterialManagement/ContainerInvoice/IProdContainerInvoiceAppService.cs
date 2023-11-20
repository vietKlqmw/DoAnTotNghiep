using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.ContainerInvoice
{
    public interface IProdContainerInvoiceAppService : IApplicationService
    {
        Task<PagedResultDto<ProdContainerInvoiceDto>> GetProdContainerInvoiceSearch(GetProdContainerInvoiceInput input);
    }
}
