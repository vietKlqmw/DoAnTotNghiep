using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Authorization;
using tmss.Dto;
using tmss.MaterialManagement.ContainerList.Exporting;

namespace tmss.MaterialManagement.ContainerList
{
    [AbpAuthorize(AppPermissions.Pages_Management_Warehouse_ContainerList_View)]
    public class ProdContainerListAppService : tmssAppServiceBase, IProdContainerListAppService
    {
        private readonly IDapperRepository<ProdContainerList, long> _dapperRepo;
        private readonly IProdContainerListExcelExporter _excelExporter;

        public ProdContainerListAppService(
            IDapperRepository<ProdContainerList, long> dapperRepo,
            IProdContainerListExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdContainerListDto>> GetProdContainerListSearch(GetProdContainerListInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_LIST_SEARCH @p_ContainerNo, @p_SupplierNo, @p_BillOfLadingNo, @p_PortDateFrom, @p_PortDateTo, " +
                "@p_ReceiveDateFrom, @p_ReceiveDateTo, @p_InvoiceNo, @p_ContainerStatus";

            IEnumerable<ProdContainerListDto> result = await _dapperRepo.QueryAsync<ProdContainerListDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_SupplierNo = input.SupplierNo,
                p_BillOfLadingNo = input.BillOfLadingNo,
                p_PortDateFrom = input.PortDateFrom,
                p_PortDateTo = input.PortDateTo,
                p_ReceiveDateFrom = input.ReceiveDateFrom,
                p_ReceiveDateTo = input.ReceiveDateTo,
                p_InvoiceNo = input.InvoiceNo,
                p_ContainerStatus = input.ContainerStatus
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0)
            {
                listResult[0].GrandFreight = listResult.Sum(e => e.Freight);
                listResult[0].GrandInsurance = listResult.Sum(e => e.Insurance);
                listResult[0].GrandTax = listResult.Sum(e => e.Tax);
                listResult[0].GrandCif = listResult.Sum(e => e.Cif);
                listResult[0].GrandAmount = listResult.Sum(e => e.Amount);
                listResult[0].GrandCost = listResult.Sum(e => e.TotalAmount);
            }

            return new PagedResultDto<ProdContainerListDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdContainerListToExcel(GetProdContainerListExportInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_LIST_SEARCH @p_ContainerNo, @p_SupplierNo, @p_BillOfLadingNo, @p_PortDateFrom, @p_PortDateTo, " +
                "@p_ReceiveDateFrom, @p_ReceiveDateTo, @p_InvoiceNo, @p_ContainerStatus";

            IEnumerable<ProdContainerListDto> result = await _dapperRepo.QueryAsync<ProdContainerListDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_SupplierNo = input.SupplierNo,
                p_BillOfLadingNo = input.BillOfLadingNo,
                p_PortDateFrom = input.PortDateFrom,
                p_PortDateTo = input.PortDateTo,
                p_ReceiveDateFrom = input.ReceiveDateFrom,
                p_ReceiveDateTo = input.ReceiveDateTo,
                p_InvoiceNo = input.InvoiceNo,
                p_ContainerStatus = input.ContainerStatus
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
