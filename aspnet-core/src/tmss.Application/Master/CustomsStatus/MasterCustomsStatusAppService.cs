using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.CustomsStatus
{
    public class MasterCustomsStatusAppService : tmssAppServiceBase, IMasterCustomsStatusAppService
    {
        private readonly IRepository<MasterCustomsStatus, long> _repo;

        public MasterCustomsStatusAppService(IRepository<MasterCustomsStatus, long> repo)
        {
            _repo = repo;
        }

        public async Task<PagedResultDto<MasterCustomsStatusDto>> GetCustomsStatusSearch(GetMasterCustomsStatusInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterCustomsStatusDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterCustomsStatusDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
