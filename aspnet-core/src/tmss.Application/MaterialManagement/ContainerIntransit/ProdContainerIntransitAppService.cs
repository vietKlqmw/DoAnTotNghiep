using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.MaterialManagement.ContainerIntransit.Exporting;

namespace tmss.MaterialManagement.ContainerIntransit
{
    public class ProdContainerIntransitAppService : tmssAppServiceBase, IProdContainerIntransitAppService
    {
        private readonly IDapperRepository<ProdContainerIntransit, long> _dapperRepo;
        private readonly IProdContainerIntransitExcelExporter _excelExporter;

        public ProdContainerIntransitAppService(
            IDapperRepository<ProdContainerIntransit, long> dapperRepo,
            IProdContainerIntransitExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }
        public async Task<PagedResultDto<ProdContainerIntransitDto>> GetProdContainerIntransitSearch(GetProdContainerIntransitInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_SEARCH @p_ContainerNo, @p_ShippingDate, @p_PortDate, @p_TransactionDate, @p_TmvDate";

            IEnumerable<ProdContainerIntransitDto> result = await _dapperRepo.QueryAsync<ProdContainerIntransitDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_ShippingDate = input.ShippingDate,
                p_PortDate = input.PortDate,
                p_TransactionDate = input.TransactionDate,
                p_TmvDate = input.TmvDate
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdContainerIntransitDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdContainerIntransitToExcel(GetProdContainerIntransitExportInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_SEARCH @p_ContainerNo, @p_ShippingDate, @p_PortDate, @p_TransactionDate, @p_TmvDate";

            IEnumerable<ProdContainerIntransitDto> result = await _dapperRepo.QueryAsync<ProdContainerIntransitDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_ShippingDate = input.ShippingDate,
                p_PortDate = input.PortDate,
                p_TransactionDate = input.TransactionDate,
                p_TmvDate = input.TmvDate
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }
    }
}
