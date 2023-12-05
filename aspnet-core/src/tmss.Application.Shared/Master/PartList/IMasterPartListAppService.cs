using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.PartList
{
    public interface IMasterPartListAppService : IApplicationService
    {
        Task<PagedResultDto<MasterPartListDto>> GetPartListSearch(GetMasterPartListInput input);

        Task CreateOrEdit(CreateOrEditMasterPartListDto input);
    }
}
