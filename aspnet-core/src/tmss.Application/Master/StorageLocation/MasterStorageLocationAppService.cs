using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.Master.StorageLocation.Exporting;

namespace tmss.Master.StorageLocation
{
    public class MasterStorageLocationAppService : tmssAppServiceBase, IMasterStorageLocationAppService
    {
        private readonly IDapperRepository<MasterStorageLocation, long> _dapperRepo;
        private readonly IMasterStorageLocationExcelExporter _excelExporter;

        public MasterStorageLocationAppService(
            IDapperRepository<MasterStorageLocation, long> dapperRepo,
            IMasterStorageLocationExcelExporter excelExporter
                                         )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }
        public async Task<PagedResultDto<MasterStorageLocationDto>> GetStorageLocationSearch(GetMasterStorageLocationInput input)
        {
            string _sql = "Exec INV_MASTER_STORAGE_LOCATION_SEARCH @p_PlantName, @p_StorageLocationName, @p_AddressLanguageEn, @p_Category";

            IEnumerable<MasterStorageLocationDto> result = await _dapperRepo.QueryAsync<MasterStorageLocationDto>(_sql, new
            {
                p_PlantName = input.PlantName,
                p_StorageLocationName = input.StorageLocationName,
                p_AddressLanguageEn = input.AddressLanguageEn,
                p_Category = input.Category
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MasterStorageLocationDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetStorageLocationToExcel(GetMasterStorageLocationExportInput input)
        {
            string _sql = "Exec INV_MASTER_STORAGE_LOCATION_SEARCH @p_PlantName, @p_StorageLocationName, @p_AddressLanguageEn, @p_Category";

            IEnumerable<MasterStorageLocationDto> result = await _dapperRepo.QueryAsync<MasterStorageLocationDto>(_sql, new
            {
                p_PlantName = input.PlantName,
                p_StorageLocationName = input.StorageLocationName,
                p_AddressLanguageEn = input.AddressLanguageEn,
                p_Category = input.Category
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
