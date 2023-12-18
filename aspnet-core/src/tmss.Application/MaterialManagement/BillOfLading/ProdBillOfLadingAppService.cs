using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.MaterialManagement.BillOfLading.Exporting;

namespace tmss.MaterialManagement.BillOfLading
{
    public class ProdBillOfLadingAppService : tmssAppServiceBase, IProdBillOfLadingAppService
    {
        private readonly IDapperRepository<ProdBillOfLading, long> _dapperRepo;
        private readonly IProdBillOfLadingExcelExporter _excelExporter;

        public ProdBillOfLadingAppService(
            IDapperRepository<ProdBillOfLading, long> dapperRepo,
            IProdBillOfLadingExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdBillOfLadingDto>> GetProdBillOfLadingSearch(GetProdBillOfLadingInput input)
        {
            string _sql = "Exec INV_PROD_BILL_OF_LADING_SEARCH @p_BillofladingNo, @p_BillDateFrom, @p_BillDateTo";

            IEnumerable<ProdBillOfLadingDto> result = await _dapperRepo.QueryAsync<ProdBillOfLadingDto>(_sql, new
            {
                p_BillofladingNo = input.BillofladingNo,
                p_BillDateFrom = input.BillDateFrom,
                p_BillDateTo = input.BillDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdBillOfLadingDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdBillOfLadingToExcel(GetProdBillOfLadingExportInput input)
        {
            string _sql = "Exec INV_PROD_BILL_OF_LADING_SEARCH @p_BillofladingNo, @p_BillDateFrom, @p_BillDateTo";

            IEnumerable<ProdBillOfLadingDto> result = await _dapperRepo.QueryAsync<ProdBillOfLadingDto>(_sql, new
            {
                p_BillofladingNo = input.BillofladingNo,
                p_BillDateFrom = input.BillDateFrom,
                p_BillDateTo = input.BillDateTo
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task DeleteBillOfLading(int? BillId)
        {
            string _sql = "Exec INV_PROD_BILL_OF_LADING_DELETE @p_BillId, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_BillId = BillId,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task EditBillOfLading(ProdBillOfLadingDto input)
        {
            string _sql = "Exec INV_PROD_BILL_OF_LADING_EDIT @p_BillId, @p_BillDate, @p_StatusCode, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_BillId = input.Id,
                p_BillDate = input.BillDate,
                p_StatusCode = input.StatusCode,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task<ProdBillOfLadingDto> ViewBillOfLading(int? BillId)
        {
            string _sql = "Exec INV_PROD_BILL_OF_LADING_VIEW @p_BillId";
            return (await _dapperRepo.QueryAsync<ProdBillOfLadingDto>(_sql, new
            {
                p_BillId = BillId
            })).FirstOrDefault();
        }
    }
}
