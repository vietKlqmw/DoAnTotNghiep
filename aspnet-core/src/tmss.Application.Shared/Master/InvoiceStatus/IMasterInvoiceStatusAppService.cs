using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.InvoiceStatus
{
    public interface IMasterInvoiceStatusAppService : IApplicationService
    {
        Task<PagedResultDto<MasterInvoiceStatusDto>> GetInvoiceStatusSearch(GetInvoiceStatusInput input);
    }
}
