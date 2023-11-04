using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.ContainerStatus
{
    public interface IMasterContainerStatusAppService : IApplicationService
    {
        Task<PagedResultDto<MasterContainerStatusDto>> GetContainerStatusSearch(GetMasterContainerStatusInput input);
    }
}
