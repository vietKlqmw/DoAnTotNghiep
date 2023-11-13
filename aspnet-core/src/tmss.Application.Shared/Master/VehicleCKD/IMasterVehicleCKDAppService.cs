using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.VehicleCKD
{
    public interface IMasterVehicleCKDAppService : IApplicationService
    {
        Task<PagedResultDto<MasterVehicleCKDDto>> GetVehicleCKDSearch(GetVehicleCKDInput input);
    }
}
