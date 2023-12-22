using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.MaterialManagement.OrderPart.Exporting;

namespace tmss.MaterialManagement.OrderPart
{
    public class ProdOrderPartAppService : tmssAppServiceBase, IProdOrderPartAppService
    {
        private readonly IDapperRepository<ProdOrderPart, long> _dapperRepo;
        private readonly IProdOrderPartExcelExporter _excelExporter;

        public ProdOrderPartAppService(
            IDapperRepository<ProdOrderPart, long> dapperRepo,
            IProdOrderPartExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdOrderPartDto>> GetProdOrderPartSearch(GetProdOrderPartInput input)
        {
            string _sql = "Exec INV_PROD_ORDER_PART_SEARCH @p_PartNo, @p_SupplierNo, @p_Cfc, @p_ContainerNo, @p_ShimentNo, @p_Status, @p_OrderDateFrom, @p_OrderDateTo";

            IEnumerable<ProdOrderPartDto> result = await _dapperRepo.QueryAsync<ProdOrderPartDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_SupplierNo = input.SupplierNo,
                p_Cfc = input.CarfamilyCode,
                p_ContainerNo = input.ContainerNo,
                p_ShimentNo = input.ShipmentNo,
                p_Status = input.Status,
                p_OrderDateFrom = input.OrderDateFrom,
                p_OrderDateTo = input.OrderDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdOrderPartDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdOrderPartToExcel(GetProdOrderPartExportInput input)
        {
            string _sql = "Exec INV_PROD_ORDER_PART_SEARCH @p_PartNo, @p_SupplierNo, @p_Cfc, @p_ContainerNo, @p_ShimentNo, @p_Status, @p_OrderDateFrom, @p_OrderDateTo";

            IEnumerable<ProdOrderPartDto> result = await _dapperRepo.QueryAsync<ProdOrderPartDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_SupplierNo = input.SupplierNo,
                p_Cfc = input.CarfamilyCode,
                p_ContainerNo = input.ContainerNo,
                p_ShimentNo = input.ShipmentNo,
                p_Status = input.Status,
                p_OrderDateFrom = input.OrderDateFrom,
                p_OrderDateTo = input.OrderDateTo
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task DeleteOrder(int? Id)
        {
            string _sql = "UPDATE ProdOrderPart SET IsDeleted = 1 WHERE Id = @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id
            });
        }
    }
}
