using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.TransmissionType
{
    public interface IMasterTransmissionTypeAppService : IApplicationService
    {
        Task<PagedResultDto<MasterTransmissionTypeDto>> GetTransmissionTypeSearch(GetMasterTransmissionTypeInput input);
    }
}
