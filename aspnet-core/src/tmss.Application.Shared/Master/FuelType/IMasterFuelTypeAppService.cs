using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.FuelType
{
    public interface IMasterFuelTypeAppService : IApplicationService
    {
        Task<PagedResultDto<MasterFuelTypeDto>> GetFuelTypeSearch(GetMasterFuelTypeInput input);
    }
}
