using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.MaterialManagement.CustomsDeclare.Exporting;

namespace tmss.MaterialManagement.CustomsDeclare
{
    public class ProdCustomsDeclareAppService : tmssAppServiceBase, IProdCustomsDeclareAppService
    {
        private readonly IDapperRepository<ProdCustomsDeclare, long> _dapperRepo;
        private readonly IProdCustomsDeclareExcelExporter _excelExporter;

        public ProdCustomsDeclareAppService(
            IDapperRepository<ProdCustomsDeclare, long> dapperRepo,
            IProdCustomsDeclareExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }
        public async Task<PagedResultDto<ProdCustomsDeclareDto>> GetProdCustomsDeclareSearch(GetProdCustomsDeclareInput input)
        {
            string _sql = "Exec INV_PROD_CUSTOMS_DECLARE_SEARCH @p_CustomsDeclareNo, @p_DeclareDate, @p_BillOfLadingNo";

            IEnumerable<ProdCustomsDeclareDto> result = await _dapperRepo.QueryAsync<ProdCustomsDeclareDto>(_sql, new
            {
                p_CustomsDeclareNo = input.CustomsDeclareNo,
                p_DeclareDate = input.DeclareDate,
                p_BillOfLadingNo = input.BillOfLadingNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdCustomsDeclareDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdCustomsDeclareToExcel(GetProdCustomsDeclareExportInput input)
        {
            string _sql = "Exec INV_PROD_CUSTOMS_DECLARE_SEARCH @p_CustomsDeclareNo, @p_DeclareDate, @p_BillOfLadingNo";

            IEnumerable<ProdCustomsDeclareDto> result = await _dapperRepo.QueryAsync<ProdCustomsDeclareDto>(_sql, new
            {
                p_CustomsDeclareNo = input.CustomsDeclareNo,
                p_DeclareDate = input.DeclareDate,
                p_BillOfLadingNo = input.BillOfLadingNo
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task UpdateCustomsDeclare(ProdCustomsDeclareDto input)
        {
            string _sql = "Exec INV_PROD_CUSTOMS_DECLARE_EDIT @p_CustomsDeclareId, @p_DeclareDate, @p_Tax, @p_Vat, @p_Status, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_CustomsDeclareId = input.Id,
                p_DeclareDate = input.DeclareDate,
                p_Tax = input.Tax,
                p_Vat = input.Vat,
                p_Status = input.Status,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
