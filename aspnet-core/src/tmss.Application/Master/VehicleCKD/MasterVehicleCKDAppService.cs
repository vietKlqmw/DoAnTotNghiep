using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.Master.VehicleCKD.Exporting;

namespace tmss.Master.VehicleCKD
{
    public class MasterVehicleCKDAppService : tmssAppServiceBase, IMasterVehicleCKDAppService
    {
        private readonly IDapperRepository<MasterVehicleCKD, long> _dapperRepo;
        private readonly IMasterVehicleCKDExcelExporter _excelExporter;

        public MasterVehicleCKDAppService(
            IDapperRepository<MasterVehicleCKD, long> dapperRepo,
            IMasterVehicleCKDExcelExporter excelExporter
                                         )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }
        public async Task<PagedResultDto<MasterVehicleCKDDto>> GetVehicleCKDSearch(GetVehicleCKDInput input)
        {
            string _sql = "Exec INV_MASTER_VEHICLE_CKD_SEARCH @p_Model, @p_Cfc, @p_ModelCode";

            IEnumerable<MasterVehicleCKDDto> result = await _dapperRepo.QueryAsync<MasterVehicleCKDDto>(_sql, new
            {
                p_Model = input.Model,
                p_Cfc = input.Cfc,
                p_ModelCode = input.ModelCode
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MasterVehicleCKDDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetVehicleCKDToExcel(GetVehicleCKDExportInput input)
        {
            string _sql = "Exec INV_MASTER_VEHICLE_CKD_SEARCH @p_Model, @p_Cfc, @p_ModelCode";

            IEnumerable<MasterVehicleCKDDto> result = await _dapperRepo.QueryAsync<MasterVehicleCKDDto>(_sql, new
            {
                p_Model = input.Model,
                p_Cfc = input.Cfc,
                p_ModelCode = input.ModelCode
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
