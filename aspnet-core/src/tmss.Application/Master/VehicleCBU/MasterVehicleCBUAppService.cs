using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.Master.VehicleCBU.Exporting;

namespace tmss.Master.VehicleCBU
{
    public class MasterVehicleCBUAppService : tmssAppServiceBase, IMasterVehicleCBUAppService
    {
        private readonly IDapperRepository<MasterVehicleCBU, long> _dapperRepo;
        private readonly IMasterVehicleCBUExcelExporter _excelExporter;

        public MasterVehicleCBUAppService(
            IDapperRepository<MasterVehicleCBU, long> dapperRepo,
            IMasterVehicleCBUExcelExporter excelExporter
                                         )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<MasterVehicleCBUDto>> GetVehicleCBUSearch(GetMasterVehicleCBUInput input)
        {
            string _sql = "Exec INV_MASTER_VEHICLE_CBU_SEARCH @p_VehicleType, @p_Model";

            IEnumerable<MasterVehicleCBUDto> result = await _dapperRepo.QueryAsync<MasterVehicleCBUDto>(_sql, new
            {
                p_VehicleType = input.VehicleType,
                p_Model = input.Model

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MasterVehicleCBUDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetVehicleCBUToExcel(GetMasterVehicleCBUExportInput input)
        {
            string _sql = "Exec INV_MASTER_VEHICLE_CBU_SEARCH @p_VehicleType, @p_Model";

            IEnumerable<MasterVehicleCBUDto> result = await _dapperRepo.QueryAsync<MasterVehicleCBUDto>(_sql, new
            {
                p_VehicleType = input.VehicleType,
                p_Model = input.Model

            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
