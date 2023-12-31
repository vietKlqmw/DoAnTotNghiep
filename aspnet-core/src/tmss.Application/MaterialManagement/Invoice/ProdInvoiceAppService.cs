﻿using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Authorization;
using tmss.Dto;
using tmss.MaterialManagement.Invoice.Exporting;

namespace tmss.MaterialManagement.Invoice
{
    [AbpAuthorize(AppPermissions.Pages_Management_Intransit_Invoice_View)]
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

        public async Task<PagedResultDto<ProdInvoiceDto>> GetProdInvoiceDetailsSearch(GetProdInvoiceDetailsInput input)
        {
            string _sql = "Exec INV_PROD_INVOICE_DETAILS_SEARCH @p_InvoiceId";

            IEnumerable<ProdInvoiceDto> result = await _dapperRepo.QueryAsync<ProdInvoiceDto>(_sql, new
            {
                p_InvoiceId = input.InvoiceId
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0)
            {
                listResult[0].GrandCif = listResult.Sum(e => e.Cif);
                listResult[0].GrandThc = listResult.Sum(e => e.Thc);
                listResult[0].GrandFreight = listResult.Sum(e => e.Freight);
                listResult[0].GrandInsurance = listResult.Sum(e => e.Insurance);
                listResult[0].GrandTax = listResult.Sum(e => e.Tax);
                listResult[0].GrandVat = listResult.Sum(e => e.Vat);
                listResult[0].GrandQty = listResult.Sum(e => e.UsageQty);
                listResult[0].GrandCost = listResult.Sum(e => e.Cost);
            }

            return new PagedResultDto<ProdInvoiceDto>(totalCount, pagedAndFiltered);
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

            IEnumerable<ProdInvoiceDto> result = await _dapperRepo.QueryAsync<ProdInvoiceDto>(_sql, new
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

        public async Task EditInvoice(ProdInvoiceDto input)
        {
            string _sql = "Exec INV_PROD_INVOICE_EDIT @p_InvoiceId, @p_InvoiceDate, @p_Status, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_InvoiceId = input.Id,
                p_InvoiceDate = input.InvoiceDate,
                p_Status = input.Status,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task UpdateAmountInvoice(ProdInvoiceDetailsDto input)
        {
            string _sql = "Exec INV_PROD_INVOICE_UPDATE_AMOUNT_INVOICE @p_InvoiceDetailsId, @p_Freight, " +
               "@p_Insurance, @p_Tax, @p_Vat, @p_Thc, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_InvoiceDetailsId = input.Id,
                p_Freight = input.Freight,
                p_Insurance = input.Insurance,
                p_Tax = input.Tax,
                p_Vat = input.Vat,
                p_Thc = input.Thc,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
