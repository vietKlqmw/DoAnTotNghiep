using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.MaterialManagement.ContainerInvoice.Exporting;

namespace tmss.MaterialManagement.ContainerInvoice
{
    public class ProdContainerInvoiceAppService : tmssAppServiceBase, IProdContainerInvoiceAppService
    {
        private readonly IDapperRepository<ProdContainerInvoice, long> _dapperRepo;
        private readonly IProdContainerInvoiceExcelExporter _excelExporter;

        public ProdContainerInvoiceAppService(
            IDapperRepository<ProdContainerInvoice, long> dapperRepo,
            IProdContainerInvoiceExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdContainerInvoiceDto>> GetProdContainerInvoiceSearch(GetProdContainerInvoiceInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INVOICE_SEARCH @p_BillNo, @p_ContainerNo, @p_InvoiceNo, @p_SealNo, @p_Status, " +
                "@p_SupplierNo, @p_BillDateFrom, @p_BillDateTo";

            IEnumerable<ProdContainerInvoiceDto> result = await _dapperRepo.QueryAsync<ProdContainerInvoiceDto>(_sql, new
            {
                p_BillNo = input.BillNo,
                p_ContainerNo = input.ContainerNo,
                p_InvoiceNo = input.InvoiceNo,
                p_SealNo = input.SealNo,
                p_Status = input.Status,
                p_SupplierNo = input.SupplierNo,
                p_BillDateFrom = input.BillDateFrom,
                p_BillDateTo = input.BillDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0)
            {
                listResult[0].GrandFreight = listResult.Sum(e => e.Freight);
                listResult[0].GrandInsurance = listResult.Sum(e => e.Insurance);
                listResult[0].GrandTax = listResult.Sum(e => e.Tax);
                listResult[0].GrandAmount = listResult.Sum(e => e.Amount);
                listResult[0].GrandTaxVnd = (long)listResult.Sum(e => (decimal?)e.TaxVnd);
                listResult[0].GrandVatVnd = (long)listResult.Sum(e => (decimal?)e.VatVnd);
            }

            return new PagedResultDto<ProdContainerInvoiceDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdContainerInvoiceToExcel(GetProdContainerInvoiceExportInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INVOICE_SEARCH @p_BillNo, @p_ContainerNo, @p_InvoiceNo, @p_SealNo, @p_Status, " +
                "@p_SupplierNo, @p_BillDateFrom, @p_BillDateTo";

            IEnumerable<ProdContainerInvoiceDto> result = await _dapperRepo.QueryAsync<ProdContainerInvoiceDto>(_sql, new
            {
                p_BillNo = input.BillNo,
                p_ContainerNo = input.ContainerNo,
                p_InvoiceNo = input.InvoiceNo,
                p_SealNo = input.SealNo,
                p_Status = input.Status,
                p_SupplierNo = input.SupplierNo,
                p_BillDateFrom = input.BillDateFrom,
                p_BillDateTo = input.BillDateTo
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
