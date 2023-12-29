using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Authorization;

namespace tmss.Master.Forwarder
{
    [AbpAuthorize(AppPermissions.Pages_Master_General_Forwarder_View)]
    public class MasterForwarderAppService : tmssAppServiceBase, IMasterForwarderAppService
    {
        private readonly IDapperRepository<MasterForwarder, long> _dapperRepo;

        public MasterForwarderAppService(
            IDapperRepository<MasterForwarder, long> dapperRepo
            )
        {
            _dapperRepo = dapperRepo;
        }
        public async Task<PagedResultDto<MasterForwarderDto>> GetForwarderSearch(GetMasterForwarderInput input)
        {
            string _sql = "Exec INV_MASTER_FORWARDER_SEARCH @p_Code, @p_Name, @p_SupplierNo";

            IEnumerable<MasterForwarderDto> result = await _dapperRepo.QueryAsync<MasterForwarderDto>(_sql, new
            {
                p_Code = input.Code,
                p_Name = input.Name,
                p_SupplierNo = input.SupplierNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MasterForwarderDto>(totalCount, pagedAndFiltered);
        }

        public async Task DeleteForwarder(int? Id)
        {
            string _sql = "DELETE MasterForwarder WHERE Id = @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id
            });
        }
    }
}
