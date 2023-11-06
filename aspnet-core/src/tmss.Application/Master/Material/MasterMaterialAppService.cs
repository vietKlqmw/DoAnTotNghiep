using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.Master.Material.Exporting;

namespace tmss.Master.Material
{
    public class MasterMaterialAppService : tmssAppServiceBase, IMasterMaterialAppService
    {
        private readonly IDapperRepository<MasterMaterial, long> _dapperRepo;
        private readonly IMasterMaterialExcelExporter _excelExporter;

        public MasterMaterialAppService(
            IDapperRepository<MasterMaterial, long> dapperRepo,
            IMasterMaterialExcelExporter excelExporter
                                         )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<MasterMaterialDto>> GetMaterialSearch(GetMasterMaterialInput input)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_SEARCH @p_MaterialCode, @p_MaterialGroup";

            IEnumerable<MasterMaterialDto> result = await _dapperRepo.QueryAsync<MasterMaterialDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialGroup = input.MaterialGroup

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MasterMaterialDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetMaterialMasterToExcel(GetMasterMaterialExportInput input)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_SEARCH @p_MaterialCode, @p_MaterialGroup";

            IEnumerable<MasterMaterialDto> result = await _dapperRepo.QueryAsync<MasterMaterialDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialGroup = input.MaterialGroup

            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task<PagedResultDto<MasterMaterialDto>> GetDataMaterialbyId(long? IdMaterial)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_BY_ID @p_MaterialId";


            IEnumerable<MasterMaterialDto> result = await _dapperRepo.QueryAsync<MasterMaterialDto>(_sql, new
            {
                p_MaterialId = IdMaterial
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MasterMaterialDto>(
                totalCount,
                listResult);
        }
    }
}
