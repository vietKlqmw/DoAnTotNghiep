using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.EngineType
{
    public class MasterEngineTypeAppService : tmssAppServiceBase, IMasterEngineTypeAppService
    {
        private readonly IRepository<MasterEngineType, long> _repo;

        public MasterEngineTypeAppService(IRepository<MasterEngineType, long> repo)
        {
            _repo = repo;
        }

        public async Task<PagedResultDto<MasterEngineTypeDto>> GetEngineTypeSearch(GetMasterEngineTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterEngineTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterEngineTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}