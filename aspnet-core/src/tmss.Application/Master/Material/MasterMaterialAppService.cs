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

        public async Task EditInfoMaterial(MasterMaterialDto input)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_EDIT @p_MaterialId, @p_MaterialType, " +
                "@p_MaterialCode, @p_Description, @p_MaterialGroup, @p_BaseUnitOfMeasure, " +
                "@p_Plant, @p_StorageLocation, @p_ProductionGroup, @p_ProductionPurpose, " +
                "@p_ProductionType, @p_ReservedStock, @p_LotCode, @p_ProductionStorageLocation, " +
                "@p_CostingLotSize, @p_ProductionVersion, @p_StandardPrice, @p_MovingPrice, " +
                "@p_MaterialOrigin, @p_OriginGroup, @p_EffectiveDateFrom, @p_EffectiveDateTo, " +
                "@p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_MaterialId = input.Id,
                p_MaterialType = input.MaterialType,
                p_MaterialCode = input.MaterialCode,
                p_Description = input.Description,
                p_MaterialGroup = input.MaterialGroup,
                p_BaseUnitOfMeasure = input.BaseUnitOfMeasure,
                p_Plant = input.Plant,
                p_StorageLocation = input.StorageLocation,
                p_ProductionGroup = input.ProductionGroup,
                p_ProductionPurpose = input.ProductionPurpose,
                p_ProductionType = input.ProductionType,
                p_ReservedStock = input.ReservedStock,
                p_LotCode = input.LotCode,
                p_ProductionStorageLocation = input.ProductionStorageLocation,
                p_CostingLotSize = input.CostingLotSize,
                p_ProductionVersion = input.ProductionVersion,
                p_StandardPrice = input.StandardPrice,
                p_MovingPrice = input.MovingPrice,
                p_MaterialOrigin = input.MaterialOrigin,
                p_OriginGroup = input.OriginGroup,
                p_EffectiveDateFrom = input.EffectiveDateFrom,
                p_EffectiveDateTo = input.EffectiveDateTo,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task DeleteMaterial(int? MaterialId)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_DELETE @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = MaterialId
            });
        }
    }
}
