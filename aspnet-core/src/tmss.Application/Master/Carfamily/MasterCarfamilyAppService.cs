using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.Carfamily
{
    public class MasterCarfamilyAppService : tmssAppServiceBase, IMasterCarfamilyAppService
    {
        private readonly IRepository<MasterCarfamily, long> _repo;

        public MasterCarfamilyAppService(IRepository<MasterCarfamily, long> repo)
        {
            _repo = repo;
        }
        public async Task<PagedResultDto<MasterCarfamilyDto>> GetCarfamilySearch(GetMasterCarfamilyInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterCarfamilyDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterCarfamilyDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
