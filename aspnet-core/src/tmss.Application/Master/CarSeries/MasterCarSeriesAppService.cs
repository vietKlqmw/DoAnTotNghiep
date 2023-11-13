using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.CarSeries
{
    public class MasterCarSeriesAppService : tmssAppServiceBase, IMasterCarSeriesAppService
    {
        private readonly IRepository<MasterCarSeries, long> _repo;

        public MasterCarSeriesAppService(IRepository<MasterCarSeries, long> repo)
        {
            _repo = repo;
        }
        public async Task<PagedResultDto<MasterCarSeriesDto>> GetCarSeriesSearch(GetMasterCarSeriesInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterCarSeriesDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterCarSeriesDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
