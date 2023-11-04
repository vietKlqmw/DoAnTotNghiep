using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.CustomsStatus
{
    public interface IMasterCustomsStatusAppService : IApplicationService
    {
        Task<PagedResultDto<MasterCustomsStatusDto>> GetCustomsStatusSearch(GetMasterCustomsStatusInput input);
    }
}
