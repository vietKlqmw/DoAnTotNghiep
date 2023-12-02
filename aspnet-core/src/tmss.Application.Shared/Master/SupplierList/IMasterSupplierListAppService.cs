using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.SupplierList
{
    public interface IMasterSupplierListAppService : IApplicationService
    {
        Task<PagedResultDto<MasterSupplierListDto>> GetSupplierListSearch(GetMasterSupplierListInput input);
    }
}
