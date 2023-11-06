using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.Factory
{
    public interface IMasterFactoryAppService : IApplicationService
    {
        Task<PagedResultDto<MasterFactoryDto>> GetFactorySearch(GetMasterFactoryInput input);
    }
}
