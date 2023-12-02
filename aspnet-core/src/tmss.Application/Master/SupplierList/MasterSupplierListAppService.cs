using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.SupplierList
{
    public class MasterSupplierListAppService : tmssAppServiceBase, IMasterSupplierListAppService
    {
        private readonly IRepository<MasterSupplierList, long> _repo;

        public MasterSupplierListAppService(IRepository<MasterSupplierList, long> repo)
        {
            _repo = repo;
        }
        public async Task<PagedResultDto<MasterSupplierListDto>> GetSupplierListSearch(GetMasterSupplierListInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierName), e => e.SupplierName.Contains(input.SupplierName));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MasterSupplierListDto
                         {
                             Id = o.Id,
                             SupplierNo = o.SupplierNo,
                             SupplierName = o.SupplierName,
                             Remarks = o.Remarks,
                             SupplierType = o.SupplierType,
                             SupplierNameVn = o.SupplierNameVn,
                             Exporter = o.Exporter
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MasterSupplierListDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
    }
}
