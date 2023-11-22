using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.MaterialManagement.Invoice.Exporting;

namespace tmss.MaterialManagement.Invoice
{
    public class ProdInvoiceAppService : tmssAppServiceBase, IProdInvoiceAppService
    {
        private readonly IDapperRepository<ProdInvoice, long> _dapperRepo;
        private readonly IProdInvoiceExcelExporter _excelExporter;

        public ProdInvoiceAppService(
            IDapperRepository<ProdInvoice, long> dapperRepo,
            IProdInvoiceExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdInvoiceDetailsDto>> GetProdInvoiceDetailsSearch(GetProdInvoiceDetailsInput input)
        {
            string _sql = "Exec INV_PROD_INVOICE_DETAILS_SEARCH @p_InvoiceId";

            IEnumerable<ProdInvoiceDetailsDto> result = await _dapperRepo.QueryAsync<ProdInvoiceDetailsDto>(_sql, new
            {
                p_InvoiceId = input.InvoiceId
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0)
            {
                listResult[0].GrandCif = listResult.Sum(e => e.Cif);
                listResult[0].GrandFreight = listResult.Sum(e => e.Freight);
                listResult[0].GrandInsurance = listResult.Sum(e => e.Insurance);
                listResult[0].GrandTax = listResult.Sum(e => e.Tax);
                listResult[0].GrandVat = listResult.Sum(e => e.Vat);
                listResult[0].GrandQty = listResult.Sum(e => e.UsageQty);
            }

            return new PagedResultDto<ProdInvoiceDetailsDto>(totalCount, pagedAndFiltered);
        }

        public async Task<PagedResultDto<ProdInvoiceDto>> GetProdInvoiceSearch(GetProdInvoiceInput input)
        {
            string _sql = "Exec INV_PROD_INVOICE_SEARCH @p_InvoiceNo, @p_InvoiceDateFrom, @p_InvoiceDateTo, @p_BillNo, " +
                "@p_ShipmentNo, @p_ContainerNo, @p_BillDateFrom, @p_BillDateTo, @p_SupplierNo";

            IEnumerable<ProdInvoiceDto> result = await _dapperRepo.QueryAsync<ProdInvoiceDto>(_sql, new
            {
                p_InvoiceNo = input.InvoiceNo,
                p_InvoiceDateFrom = input.InvoiceDateFrom,
                p_InvoiceDateTo = input.InvoiceDateTo,
                p_BillNo = input.BillNo,
                p_ShipmentNo = input.ShipmentNo,
                p_ContainerNo = input.ContainerNo,
                p_BillDateFrom = input.BillDateFrom,
                p_BillDateTo = input.BillDateTo,
                p_SupplierNo = input.SupplierNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdInvoiceDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdInvoiceDetailsToExcel(GetProdInvoiceDetailsExportInput input)
        {
            string _sql = "Exec INV_PROD_INVOICE_DETAILS_SEARCH @p_InvoiceId";

            IEnumerable<ProdInvoiceDetailsDto> result = await _dapperRepo.QueryAsync<ProdInvoiceDetailsDto>(_sql, new
            {
                p_InvoiceId = input.InvoiceId
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFileDetails(exportToExcel);
        }

        public async Task<FileDto> GetProdInvoiceToExcel(GetProdInvoiceExportInput input)
        {
            string _sql = "Exec INV_PROD_INVOICE_SEARCH @p_InvoiceNo, @p_InvoiceDateFrom, @p_InvoiceDateTo, @p_BillNo, " +
               "@p_ShipmentNo, @p_ContainerNo, @p_BillDateFrom, @p_BillDateTo, @p_SupplierNo";

            IEnumerable<ProdInvoiceDto> result = await _dapperRepo.QueryAsync<ProdInvoiceDto>(_sql, new
            {
                p_InvoiceNo = input.InvoiceNo,
                p_InvoiceDateFrom = input.InvoiceDateFrom,
                p_InvoiceDateTo = input.InvoiceDateTo,
                p_BillNo = input.BillNo,
                p_ShipmentNo = input.ShipmentNo,
                p_ContainerNo = input.ContainerNo,
                p_BillDateFrom = input.BillDateFrom,
                p_BillDateTo = input.BillDateTo,
                p_SupplierNo = input.SupplierNo
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
