using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.ProductGroup
{
    public class MasterProductGroupAppService : tmssAppServiceBase, IMasterProductGroupAppService
    {
        private readonly IRepository<MasterProductGroup, long> _repo;

        public MasterProductGroupAppService(IRepository<MasterProductGroup, long> repo)
        {
            _repo = repo;
        }

        public async Task<PagedResultDto<MasterProductGroupDto>> GetProductGroupSearch(GetMasterProductGroupInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterProductGroupDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterProductGroupDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
