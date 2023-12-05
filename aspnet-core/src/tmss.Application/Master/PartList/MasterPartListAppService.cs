using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.EntityFrameworkCore;
using tmss.Master.PartList.Exporting;

namespace tmss.Master.PartList
{
    public class MasterPartListAppService : tmssAppServiceBase, IMasterPartListAppService
    {
        private readonly IDapperRepository<MasterPartList, long> _dapperRepo;
        private readonly IRepository<MasterPartList, long> _repo;
        private readonly IMasterPartListExcelExporter _excelExporter;

        public MasterPartListAppService(
            IDapperRepository<MasterPartList, long> dapperRepo,
            IRepository<MasterPartList, long> repo,
            IMasterPartListExcelExporter excelExporter
                                         )
        {
            _dapperRepo = dapperRepo;
            _repo = repo;
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

        public async Task DeletePartList(int? Id)
        {
            string _sql = "UPDATE MasterPartList SET IsDeleted = 1 WHERE Id = @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id
            });
        }

        public async Task CreateOrEdit(CreateOrEditMasterPartListDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        private async Task Create(CreateOrEditMasterPartListDto input)
        {
            var mainObj = ObjectMapper.Map<MasterPartList>(input);

            await CurrentUnitOfWork.GetDbContext<tmssDbContext>().AddAsync(mainObj);
        }

        private async Task Update(CreateOrEditMasterPartListDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll().FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }
    }
}
