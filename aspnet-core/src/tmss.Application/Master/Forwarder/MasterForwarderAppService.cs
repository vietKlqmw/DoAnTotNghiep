using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tmss.Master.Forwarder
{
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
    }
}
