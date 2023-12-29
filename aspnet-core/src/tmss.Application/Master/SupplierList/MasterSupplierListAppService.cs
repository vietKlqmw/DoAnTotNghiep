using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using tmss.Authorization;

namespace tmss.Master.SupplierList
{
    [AbpAuthorize(AppPermissions.Pages_Master_General_Supplier_View)]
    public class MasterSupplierListAppService : tmssAppServiceBase, IMasterSupplierListAppService
    {
        private readonly IRepository<MasterSupplierList, long> _repo;
        private readonly IDapperRepository<MasterSupplierList, long> _dapperRepo;

        public MasterSupplierListAppService(IRepository<MasterSupplierList, long> repo,
            IDapperRepository<MasterSupplierList, long> dapperRepo)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
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


        public async Task DeleteSupplier(int? Id)
        {
            string _sql = "DELETE MasterSupplierList WHERE Id = @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id
            });
        }
    }
}
