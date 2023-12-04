using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.Forwarder
{
    public interface IMasterForwarderAppService : IApplicationService
    {
        Task<PagedResultDto<MasterForwarderDto>> GetForwarderSearch(GetMasterForwarderInput input);
    }
}
