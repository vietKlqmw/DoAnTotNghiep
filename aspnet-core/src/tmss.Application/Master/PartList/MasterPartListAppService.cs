using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.Master.PartList.Exporting;

namespace tmss.Master.PartList
{
    public class MasterPartListAppService : tmssAppServiceBase, IMasterPartListAppService
    {
        private readonly IDapperRepository<MasterPartList, long> _dapperRepo;
        private readonly IMasterPartListExcelExporter _excelExporter;

        public MasterPartListAppService(
            IDapperRepository<MasterPartList, long> dapperRepo,
            IMasterPartListExcelExporter excelExporter
                                         )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }
        public async Task<PagedResultDto<MasterPartListDto>> GetPartListSearch(GetMasterPartListInput input)
        {
            string _sql = "Exec INV_MASTER_PART_LIST_SEARCH @p_PartNo, @p_SupplierNo, @p_Cfc";

            IEnumerable<MasterPartListDto> result = await _dapperRepo.QueryAsync<MasterPartListDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_SupplierNo = input.SupplierNo,
                p_Cfc = input.CarfamilyCode
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MasterPartListDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetPartListToExcel(GetMasterPartListExportInput input)
        {
            string _sql = "Exec INV_MASTER_PART_LIST_SEARCH @p_PartNo, @p_SupplierNo, @p_Cfc";

            IEnumerable<MasterPartListDto> result = await _dapperRepo.QueryAsync<MasterPartListDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_SupplierNo = input.SupplierNo,
                p_Cfc = input.CarfamilyCode
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

    }
}
