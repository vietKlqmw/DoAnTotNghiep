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

        public async Task<List<ListForwarderBySupplierIdDto>> GetListForwarder(string SupplierNo)
        {
            string _sql = "Exec INV_PROD_GET_LIST_FORWARDER_BY_SUPPLIERID @p_SupplierNo ";
            IEnumerable<ListForwarderBySupplierIdDto> result = await _dapperRepo.QueryAsync<ListForwarderBySupplierIdDto>(_sql, new
            {
                p_SupplierNo = SupplierNo
            });

            return result.ToList();
        }

        public async Task<List<ListStatusContDto>> GetListStatusCont()
        {
            string _sql = "Exec INV_PROD_GET_LIST_CONTAINER_STATUS ";
            IEnumerable<ListStatusContDto> result = await _dapperRepo.QueryAsync<ListStatusContDto>(_sql);

            return result.ToList();
        }

        public async Task<List<MasterMaterialDto>> GetDataMaterialbyId(long? IdMaterial)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_BY_ID @p_MaterialId";

            IEnumerable<MasterMaterialDto> result = await _dapperRepo.QueryAsync<MasterMaterialDto>(_sql, new
            {
                p_MaterialId = IdMaterial
            });

            return result.ToList();
        }

        public async Task<List<ProdOthersDto>> GetListCarfamilyCode()
        {
            string _sql = "Exec INV_PROD_GET_LIST_CFC ";
            IEnumerable<ProdOthersDto> result = await _dapperRepo.QueryAsync<ProdOthersDto>(_sql);

            return result.ToList();
        }

        public async Task<List<ListMaterialUsageDto>> GetListMaterialUsage()
        {
            string _sql = "Exec INV_PROD_GET_LIST_MATERIAL_USAGE";
            IEnumerable<ListMaterialUsageDto> result = await _dapperRepo.QueryAsync<ListMaterialUsageDto>(_sql);

            return result.ToList();
        }
    }
}
