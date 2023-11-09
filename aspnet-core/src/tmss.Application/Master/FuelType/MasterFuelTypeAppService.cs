using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.FuelType
{
    public class MasterFuelTypeAppService : tmssAppServiceBase, IMasterFuelTypeAppService
    {
        private readonly IRepository<MasterFuelType, long> _repo;

        public MasterFuelTypeAppService(IRepository<MasterFuelType, long> repo)
        {
            _repo = repo;
        }

        public async Task<PagedResultDto<MasterFuelTypeDto>> GetFuelTypeSearch(GetMasterFuelTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterFuelTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterFuelTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
