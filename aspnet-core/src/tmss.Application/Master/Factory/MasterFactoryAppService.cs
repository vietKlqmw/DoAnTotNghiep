using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.Factory
{
    public class MasterFactoryAppService : tmssAppServiceBase, IMasterFactoryAppService
    {
        private readonly IRepository<MasterFactory, long> _repo;

        public MasterFactoryAppService(IRepository<MasterFactory, long> repo)
        {
            _repo = repo;
        }

        public async Task<PagedResultDto<MasterFactoryDto>> GetFactorySearch(GetMasterFactoryInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PlantName), e => e.PlantName.Contains(input.PlantName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BranchNo), e => e.BranchNo.Contains(input.BranchNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.AddressLanguageEn), e => e.AddressLanguageEn.Contains(input.AddressLanguageEn));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterFactoryDto
                         {
                             Id = o.Id,
                             PlantCode = o.PlantCode,
                             PlantName = o.PlantName,
                             BranchNo = o.BranchNo,
                             AddressLanguageEn = o.AddressLanguageEn,
                             AddressLanguageVn = o.AddressLanguageVn
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterFactoryDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
