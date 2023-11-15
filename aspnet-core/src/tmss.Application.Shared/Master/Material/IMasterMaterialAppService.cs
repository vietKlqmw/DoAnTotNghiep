using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace tmss.Master.Material
{
    public interface IMasterMaterialAppService : IApplicationService
    {
        Task<PagedResultDto<MasterMaterialDto>> GetMaterialSearch(GetMasterMaterialInput input);

        Task EditInfoMaterial(MasterMaterialDto input);

        Task<List<MasterMaterialImportDto>> ImportMaterialFromExcel(byte[] fileBytes, string fileName);
    }
}
