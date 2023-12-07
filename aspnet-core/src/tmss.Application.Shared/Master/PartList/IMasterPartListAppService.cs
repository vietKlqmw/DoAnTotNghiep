using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace tmss.Master.PartList
{
    public interface IMasterPartListAppService : IApplicationService
    {
        Task<PagedResultDto<MasterPartListDto>> GetPartListSearch(GetMasterPartListInput input);

        Task<List<MasterPartListImportDto>> ImportPartListFromExcel(byte[] fileBytes, string fileName);
    }
}
