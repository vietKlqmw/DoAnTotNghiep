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

        public async Task DeleteContWH(int? Id)
        {
            string _sql = "Exec INV_PROD_CONTAINER_WAREHOUSE_DELETE @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id
            });
        }

        public async Task EditContainerWH(ProdContainerRentalWHPlanDto input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_WAREHOUSE_EDIT @p_Id, @p_ContainerNo, @p_RequestDate, @p_RequestTime," +
                "@p_InvoiceNo, @p_BillOfLadingNo, @p_SupplierNo, @p_SealNo, @p_ListcaseNo, @p_ListLotNo, @p_DevanningDate," +
                "@p_DevanningTime, @p_ActualDevanningDate, @p_GateInPlanTime, @p_GateInActualDateTime, @p_Transport, @p_Status, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = input.Id,
                p_ContainerNo = input.ContainerNo,
                p_RequestDate = input.RequestDate,
                p_RequestTime = input.RequestTime,
                p_InvoiceNo = input.InvoiceNo,
                p_BillOfLadingNo = input.BillofladingNo,
                p_SupplierNo = input.SupplierNo,
                p_SealNo = input.SealNo,
                p_ListcaseNo = input.ListcaseNo,
                p_ListLotNo = input.ListLotNo,
                p_DevanningDate = input.DevanningDate,
                p_DevanningTime = input.DevanningTime,
                p_ActualDevanningDate = input.ActualDevanningDate,
                p_GateInPlanTime = input.GateInPlanTime,
                p_GateInActualDateTime = input.GateInActualDateTime,
                p_Transport = input.Transport,
                p_Status = input.Status,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
