using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.MaterialGroup
{
    public class MasterMaterialGroupAppService : tmssAppServiceBase, IMasterMaterialGroupAppService
    {
        private readonly IRepository<MasterMaterialGroup, long> _repo;

        public MasterMaterialGroupAppService(IRepository<MasterMaterialGroup, long> repo)
        {
            _repo = repo;
        }

        public async Task<PagedResultDto<MasterMaterialGroupDto>> GetMaterialGroupSearch(GetMasterMaterialGroupInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterMaterialGroupDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterMaterialGroupDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
