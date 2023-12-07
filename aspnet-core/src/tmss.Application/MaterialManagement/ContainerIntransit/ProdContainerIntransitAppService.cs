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
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_SEARCH @p_ContainerNo, @p_ShippingDate, @p_PortDate, @p_TransactionDate";

            IEnumerable<ProdContainerIntransitDto> result = await _dapperRepo.QueryAsync<ProdContainerIntransitDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_ShippingDate = input.ShippingDate,
                p_PortDate = input.PortDate,
                p_TransactionDate = input.TransactionDate
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdContainerIntransitDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdContainerIntransitToExcel(GetProdContainerIntransitExportInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_SEARCH @p_ContainerNo, @p_ShippingDate, @p_PortDate, @p_TransactionDate";

            IEnumerable<ProdContainerIntransitDto> result = await _dapperRepo.QueryAsync<ProdContainerIntransitDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_ShippingDate = input.ShippingDate,
                p_PortDate = input.PortDate,
                p_TransactionDate = input.TransactionDate
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task DeleteContainerIntransit(int? Id)
        {
            string _sql = "UPDATE ProdContainerIntransit SET IsDeleted = 1 WHERE Id = @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id
            });
        }

        public async Task EditContainerIntransit(ProdContainerIntransitDto input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_EDIT @p_ContId, @p_ContainerNo, @p_SupplierNo, @p_ShippingDate, " +
                "@p_PortDate, @p_TransactionDate, @p_Forwarder, @p_ShipmentId, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_ContId = input.Id,
                p_ContainerNo = input.ContainerNo,
                p_SupplierNo = input.SupplierNo,
                p_ShippingDate = input.ShippingDate,
                p_PortDate = input.PortDate,
                p_TransactionDate = input.TransactionDate,
                p_Forwarder = input.Forwarder,
                p_ShipmentId = input.ShipmentId,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
