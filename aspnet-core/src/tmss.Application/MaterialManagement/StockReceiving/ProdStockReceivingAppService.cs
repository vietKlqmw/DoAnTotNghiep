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
            string _sql = "Exec INV_PROD_STOCK_RECEIVING_SEARCH @p_PartNo, @p_WorkingDateFrom, @p_WorkingDateTo, @p_SupplierNo, @p_ContainerNo, @p_InvoiceNo, @p_Model";

            IEnumerable<ProdStockReceivingDto> result = await _dapperRepo.QueryAsync<ProdStockReceivingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo,
                p_SupplierNo = input.SupplierNo,
                p_ContainerNo = input.ContainerNo,
                p_InvoiceNo = input.InvoiceNo,
                p_Model = input.Model
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
            string _sql = "Exec INV_PROD_STOCK_RECEIVING_SEARCH @p_PartNo, @p_WorkingDateFrom, @p_WorkingDateTo, @p_SupplierNo, @p_ContainerNo, @p_InvoiceNo, @p_Model";

            IEnumerable<ProdStockReceivingDto> result = await _dapperRepo.QueryAsync<ProdStockReceivingDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_WorkingDateFrom = input.WorkingDateFrom,
                p_WorkingDateTo = input.WorkingDateTo,
                p_SupplierNo = input.SupplierNo,
                p_ContainerNo = input.ContainerNo,
                p_InvoiceNo = input.InvoiceNo,
                p_Model = input.Model
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
