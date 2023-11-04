using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.InvoiceStatus
{
    public class MasterInvoiceStatusAppService : tmssAppServiceBase, IMasterInvoiceStatusAppService
    {
        private readonly IRepository<MasterInvoiceStatus, long> _repo;

        public MasterInvoiceStatusAppService(IRepository<MasterInvoiceStatus, long> repo)
        {
            _repo = repo;
        }

        public async Task<PagedResultDto<MasterInvoiceStatusDto>> GetInvoiceStatusSearch(GetInvoiceStatusInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterInvoiceStatusDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterInvoiceStatusDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
