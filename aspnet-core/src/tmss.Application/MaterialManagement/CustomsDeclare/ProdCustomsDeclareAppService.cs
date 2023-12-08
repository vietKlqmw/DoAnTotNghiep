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
            string _sql = "Exec INV_PROD_CUSTOMS_DECLARE_SEARCH @p_CustomsDeclareNo, @p_DeclareDate, @p_BillOfLadingNo, @p_InvoiceNo";

            IEnumerable<ProdCustomsDeclareDto> result = await _dapperRepo.QueryAsync<ProdCustomsDeclareDto>(_sql, new
            {
                p_CustomsDeclareNo = input.CustomsDeclareNo,
                p_DeclareDate = input.DeclareDate,
                p_BillOfLadingNo = input.BillOfLadingNo,
                p_InvoiceNo = input.InvoiceNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdCustomsDeclareDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdCustomsDeclareToExcel(GetProdCustomsDeclareExportInput input)
        {
            string _sql = "Exec INV_PROD_CUSTOMS_DECLARE_SEARCH @p_CustomsDeclareNo, @p_DeclareDate, @p_BillOfLadingNo, @p_InvoiceNo";

            IEnumerable<ProdCustomsDeclareDto> result = await _dapperRepo.QueryAsync<ProdCustomsDeclareDto>(_sql, new
            {
                p_CustomsDeclareNo = input.CustomsDeclareNo,
                p_DeclareDate = input.DeclareDate,
                p_BillOfLadingNo = input.BillOfLadingNo,
                p_InvoiceNo = input.InvoiceNo
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task EditCustomsDeclare(ProdCustomsDeclareDto input)
        {
            string _sql = "Exec INV_PROD_CUSTOMS_DECLARE_EDIT @p_CustomsDeclareId, @p_CustomsDeclareNo, " +
                "@p_DeclareDate, @p_Tax, @p_Vat, @p_Status, @p_Forwarder, @p_BillId, @p_InvoiceId, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_CustomsDeclareId = input.Id,
                p_CustomsDeclareNo = input.CustomsDeclareNo,
                p_DeclareDate = input.DeclareDate,
                p_Tax = input.Tax,
                p_Vat = input.Vat,
                p_Status = input.Status,
                p_Forwarder = input.Forwarder,
                p_BillId = input.BillId,
                p_InvoiceId = input.InvoiceId,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
