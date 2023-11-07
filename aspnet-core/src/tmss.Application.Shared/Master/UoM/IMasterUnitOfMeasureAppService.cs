using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.UoM
{
    public interface IMasterUnitOfMeasureAppService : IApplicationService
    {
        Task<PagedResultDto<MasterUnitOfMeasureDto>> GetUoMSearch(GetMasterUnitOfMeasureInput input);
    }
}
