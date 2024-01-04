using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Authorization;
using tmss.Dto;
using tmss.Master.StorageLocation.Exporting;

namespace tmss.Master.StorageLocation
{
    [AbpAuthorize(AppPermissions.Pages_Master_General_Warehouse_View)]
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
            string _sql = "Exec INV_MASTER_STORAGE_LOCATION_SEARCH @p_AddressLanguageEn, @p_Status";

            IEnumerable<MasterStorageLocationDto> result = await _dapperRepo.QueryAsync<MasterStorageLocationDto>(_sql, new
            {
                p_AddressLanguageEn = input.AddressLanguageVn,
                p_Status = input.Status
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
            string _sql = "Exec INV_MASTER_STORAGE_LOCATION_SEARCH @p_AddressLanguageEn, @p_Status";

            IEnumerable<MasterStorageLocationDto> result = await _dapperRepo.QueryAsync<MasterStorageLocationDto>(_sql, new
            {
                p_AddressLanguageEn = input.AddressLanguageVn,
                p_Status = input.Status
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task DeleteWH(int? WHId)
        {
            string _sql = "UPDATE MasterStorageLocation SET IsDeleted = 1 WHERE Id = @p_WHId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_WHId = WHId
            });
        }

        public async Task EditWH(MasterStorageLocationDto input)
        {
            float inv = input.Inventory == null ? 0 : (float)input.Inventory;
            if (inv == 0) input.Status = "Empty";
            else if (inv == input.MaxStock) input.Status = "Full";
            else if ((float)(inv / input.MaxStock) < 0.25) input.Status = "Normal";
            else if ((float)(inv / input.MaxStock) < 0.5) input.Status = "Good";
            else if ((float)(inv / input.MaxStock) < 0.75) input.Status = "Medium";
            else if ((float)(inv / input.MaxStock) < 1) input.Status = "High";

            string _sql = "Exec INV_PROD_MASTER_WAREHOUSE_EDIT @p_WarehouseId, @p_AddressVn, @p_Category, @p_MaxStock, @p_Status, @p_Type, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_WarehouseId = input.Id,
                p_AddressVn = input.AddressLanguageVn,
                p_Category = input.Category,
                p_MaxStock = input.MaxStock,
                p_Status = input.Status,
                p_Type = input.Type,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
