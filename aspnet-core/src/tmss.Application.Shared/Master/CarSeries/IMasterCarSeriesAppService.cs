using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.Master.CarSeries
{
    public interface IMasterCarSeriesAppService : IApplicationService
    {
        Task<PagedResultDto<MasterCarSeriesDto>> GetCarSeriesSearch(GetMasterCarSeriesInput input);
    }
}
