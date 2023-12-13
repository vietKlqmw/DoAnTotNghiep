using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.MaterialManagement.StockReceiving.Exporting;

namespace tmss.MaterialManagement.StockReceiving
{
    public class ProdStockReceivingAppService : tmssAppServiceBase, IProdStockReceivingAppService
    {
        private readonly IDapperRepository<ProdStockReceiving, long> _dapperRepo;
        private readonly IProdStockReceivingExcelExporter _excelExporter;

        public ProdStockReceivingAppService(
            IDapperRepository<ProdStockReceiving, long> dapperRepo,
            IProdStockReceivingExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdStockReceivingDto>> GetProdStockReceivingSearch(GetProdStockReceivingInput input)
        {
            string _sql = "Exec INV_PROD_STOCK_RECEIVING_SEARCH @p_PartNo, @p_RequestDateFrom, @p_RequestDateTo, @p_SupplierNo, @p_Model, @p_Warehouse";

            IEnumerable<ProdStockReceivingDto> result = await _dapperRepo.QueryAsync<ProdStockReceivingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_RequestDateFrom = input.RequestDateFrom,
                p_RequestDateTo = input.RequestDateTo,
                p_SupplierNo = input.SupplierNo,
                p_Model = input.Model,
                p_Warehouse = input.Warehouse
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0)
            {
                listResult[0].GrandQty = listResult.Sum(e => e.Qty);
                listResult[0].GrandActualQty = listResult.Sum(e => e.ActualQty);
                listResult[0].GrandOrderQty = listResult.Sum(e => e.OrderQty);
            }

            return new PagedResultDto<ProdStockReceivingDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdStockReceivingToExcel(GetProdStockReceivingExportInput input)
        {
            string _sql = "Exec INV_PROD_STOCK_RECEIVING_SEARCH @p_PartNo, @p_RequestDateFrom, @p_RequestDateTo, @p_SupplierNo, @p_Model, @p_Warehouse";

            IEnumerable<ProdStockReceivingDto> result = await _dapperRepo.QueryAsync<ProdStockReceivingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_RequestDateFrom = input.RequestDateFrom,
                p_RequestDateTo = input.RequestDateTo,
                p_SupplierNo = input.SupplierNo,
                p_Model = input.Model,
                p_Warehouse = input.Warehouse
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
