using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.MaterialManagement.ContainerWH.Exporting;

namespace tmss.MaterialManagement.ContainerWH
{
    public class ProdContainerRentalWHPlanAppService : tmssAppServiceBase, IProdContainerRentalWHPlanAppService
    {
        private readonly IDapperRepository<ProdContainerRentalWHPlan, long> _dapperRepo;
        private readonly IProdContainerRentalWHPlanExcelExporter _excelExporter;

        public ProdContainerRentalWHPlanAppService(
            IDapperRepository<ProdContainerRentalWHPlan, long> dapperRepo,
            IProdContainerRentalWHPlanExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdContainerRentalWHPlanDto>> GetProdContainerRentalWHPlanSearch(GetProdContainerRentalWHPlanInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_WAREHOUSE_SEARCH @p_ContainerNo, @p_InvoiceNo, @p_BillOfLadingNo, @p_SupplierNo, @p_SealNo, @p_RequestDateFrom, @p_RequestDateTo";

            IEnumerable<ProdContainerRentalWHPlanDto> result = await _dapperRepo.QueryAsync<ProdContainerRentalWHPlanDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_InvoiceNo = input.InvoiceNo,
                p_BillOfLadingNo = input.BillofladingNo,
                p_SupplierNo = input.SupplierNo,
                p_SealNo = input.SealNo,
                p_RequestDateFrom = input.RequestDateFrom,
                p_RequestDateTo = input.RequestDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdContainerRentalWHPlanDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdContainerRentalWHPlanToExcel(GetProdContainerRentalWHPlanExportInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_WAREHOUSE_SEARCH @p_ContainerNo, @p_InvoiceNo, @p_BillOfLadingNo, @p_SupplierNo, @p_SealNo, @p_RequestDateFrom, @p_RequestDateTo";

            IEnumerable<ProdContainerRentalWHPlanDto> result = await _dapperRepo.QueryAsync<ProdContainerRentalWHPlanDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_InvoiceNo = input.InvoiceNo,
                p_BillOfLadingNo = input.BillofladingNo,
                p_SupplierNo = input.SupplierNo,
                p_SealNo = input.SealNo,
                p_RequestDateFrom = input.RequestDateFrom,
                p_RequestDateTo = input.RequestDateTo
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
