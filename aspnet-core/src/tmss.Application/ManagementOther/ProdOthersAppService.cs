using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Master.Material;

namespace tmss.ManagementOther
{
    public class ProdOthersAppService : tmssAppServiceBase, IProdOthersAppService
    {
        private readonly IDapperRepository<MasterMaterial, long> _dapperRepo;

        public ProdOthersAppService(IDapperRepository<MasterMaterial, long> dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }

        public async Task<List<ListSupplierDto>> GetListSupplier()
        {
            string _sql = "Exec INV_PROD_GET_LIST_SUPPLIER ";
            IEnumerable<ListSupplierDto> result = await _dapperRepo.QueryAsync<ListSupplierDto>(_sql);

            return result.ToList();
        }

        public async Task<List<ListSupplierDto>> GetListForwarder(int? SupplierId)
        {
            string _sql = "Exec INV_PROD_GET_LIST_FORWARDER_BY_SUPPLIERID @p_SupplierId ";
            IEnumerable<ListSupplierDto> result = await _dapperRepo.QueryAsync<ListSupplierDto>(_sql, new
            {
                p_SupplierId = SupplierId
            });

            return result.ToList();
        }
    }
}
