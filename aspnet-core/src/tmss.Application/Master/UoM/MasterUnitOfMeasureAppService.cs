using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.Master.UoM.Exporting;

namespace tmss.Master.UoM
{
    public class MasterUnitOfMeasureAppService : tmssAppServiceBase, IMasterUnitOfMeasureAppService
    {
        private readonly IRepository<MasterUnitOfMeasure, long> _repo;
        private readonly IMasterUnitOfMeasureExcelExporter _excelExporter;

        public MasterUnitOfMeasureAppService(
            IRepository<MasterUnitOfMeasure, long> repo,
            IMasterUnitOfMeasureExcelExporter excelExporter)
        {
            _repo = repo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<MasterUnitOfMeasureDto>> GetUoMSearch(GetMasterUnitOfMeasureInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterUnitOfMeasureDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterUnitOfMeasureDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetUnitOfMeasureToExcel(GetMasterUnitOfMeasureExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var result = from o in pageAndFiltered
                         select new MasterUnitOfMeasureDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name
                         };

            var exportToExcel = await result.ToListAsync();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
