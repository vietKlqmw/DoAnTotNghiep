using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.Master.Engine.Exporting;

namespace tmss.Master.Engine
{
    public class MasterEngineAppService : tmssAppServiceBase, IMasterEngineAppService
    {
        private readonly IDapperRepository<MasterEngine, long> _dapperRepo;
        private readonly IMasterEngineExcelExporter _excelExporter;

        public MasterEngineAppService(
            IDapperRepository<MasterEngine, long> dapperRepo,
            IMasterEngineExcelExporter excelExporter
                                         )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }
        public async Task<PagedResultDto<MasterEngineDto>> GetEngineMasterSearch(GetMasterEngineInput input)
        {
            string _sql = "Exec INV_MASTER_ENGINE_SEARCH @p_MaterialCode, @p_TransmissionType, @p_EngineModel, @p_EngineType";

            IEnumerable<MasterEngineDto> result = await _dapperRepo.QueryAsync<MasterEngineDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_TransmissionType = input.TransmissionType,
                p_EngineModel = input.EngineModel,
                p_EngineType = input.EngineType
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MasterEngineDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetEngineMasterToExcel(GetMasterEngineExportInput input)
        {
            string _sql = "Exec INV_MASTER_ENGINE_SEARCH @p_MaterialCode, @p_TransmissionType, @p_EngineModel, @p_EngineType";

            IEnumerable<MasterEngineDto> result = await _dapperRepo.QueryAsync<MasterEngineDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_TransmissionType = input.TransmissionType,
                p_EngineModel = input.EngineModel,
                p_EngineType = input.EngineType
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
