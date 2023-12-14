using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.MaterialManagement.InvoiceStock.Exporting;

namespace tmss.MaterialManagement.InvoiceStock
{
    public class ProdInvoiceStockOutAppService : tmssAppServiceBase, IProdInvoiceStockOutAppService
    {

        private readonly IDapperRepository<ProdInvoiceStockOut, long> _dapperRepo;
        private readonly IProdInvoiceStockOutExcelExporter _excelExporter;

        public ProdInvoiceStockOutAppService(
            IDapperRepository<ProdInvoiceStockOut, long> dapperRepo,
            IProdInvoiceStockOutExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdInvoiceStockOutDto>> GetProdInvoiceStockOutSearch(GetProdInvoiceStockOutInput input)
        {
            string _sql = "Exec INV_PROD_INVOICE_STOCK_OUT_SEARCH @p_InvoiceNoOut, @p_InvoiceDateFrom, @p_InvoiceDateTo, @p_Status, @p_Warehouse";

            IEnumerable<ProdInvoiceStockOutDto> result = await _dapperRepo.QueryAsync<ProdInvoiceStockOutDto>(_sql, new
            {
                p_InvoiceNoOut = input.InvoiceNoOut,
                p_InvoiceDateFrom = input.InvoiceDateFrom,
                p_InvoiceDateTo = input.InvoiceDateTo,
                p_Status = input.Status,
                p_Warehouse = input.Warehouse
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0)
            {
                listResult[0].GrandTotalOrderQty = listResult.Sum(e => e.TotalOrderQty);
                listResult[0].GrandTotalAmount = listResult.Sum(e => e.TotalAmount);
            }

            return new PagedResultDto<ProdInvoiceStockOutDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdInvoiceStockOutToExcel(GetProdInvoiceStockOutExportInput input)
        {
            string _sql = "Exec INV_PROD_INVOICE_STOCK_OUT_SEARCH @p_InvoiceNoOut, @p_InvoiceDateFrom, @p_InvoiceDateTo, @p_Status, @p_Warehouse";

            IEnumerable<ProdInvoiceStockOutDto> result = await _dapperRepo.QueryAsync<ProdInvoiceStockOutDto>(_sql, new
            {
                p_InvoiceNoOut = input.InvoiceNoOut,
                p_InvoiceDateFrom = input.InvoiceDateFrom,
                p_InvoiceDateTo = input.InvoiceDateTo,
                p_Status = input.Status,
                p_Warehouse = input.Warehouse
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
