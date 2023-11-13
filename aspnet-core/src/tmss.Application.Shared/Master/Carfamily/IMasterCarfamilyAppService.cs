using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.Carfamily
{
    public interface IMasterCarfamilyAppService : IApplicationService
    {
        Task<PagedResultDto<MasterCarfamilyDto>> GetCarfamilySearch(GetMasterCarfamilyInput input);
    }
}
