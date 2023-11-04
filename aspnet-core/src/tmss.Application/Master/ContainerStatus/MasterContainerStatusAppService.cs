using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.ContainerStatus
{
    public class MasterContainerStatusAppService : tmssAppServiceBase, IMasterContainerStatusAppService
    {
        private readonly IRepository<MasterContainerStatus, long> _repo;

        public MasterContainerStatusAppService(IRepository<MasterContainerStatus, long> repo)
        {
            _repo = repo;
        }
        // ít data --> dùng Linq
        public async Task<PagedResultDto<MasterContainerStatusDto>> GetContainerStatusSearch(GetMasterContainerStatusInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterContainerStatusDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description,
                             DescriptionVn = o.DescriptionVn
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterContainerStatusDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
