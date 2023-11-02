using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.VehicleCBU
{
    public interface IMasterVehicleCBUAppService : IApplicationService
    {
        Task<PagedResultDto<MasterVehicleCBUDto>> GetVehicleCBUSearch(GetMasterVehicleCBUInput input);
    }
}
