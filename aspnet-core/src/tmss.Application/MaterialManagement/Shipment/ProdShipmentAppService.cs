using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.MaterialManagement.Shipment.Exporting;

namespace tmss.MaterialManagement.Shipment
{
    public class ProdShipmentAppService : tmssAppServiceBase, IProdShipmentAppService
    {
        private readonly IDapperRepository<ProdShipment, long> _dapperRepo;
        private readonly IRepository<ProdShipment, long> _repo;
        private readonly IProdShipmentExcelExporter _excelExporter;

        public ProdShipmentAppService(
            IDapperRepository<ProdShipment, long> dapperRepo,
            IRepository<ProdShipment, long> repo,
            IProdShipmentExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _repo = repo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdShipmentDto>> GetProdShipmentSearch(GetProdShipmentInput input)
        {
            string _sql = "Exec INV_PROD_SHIPMENT_SEARCH @p_ShipmentNo, @p_SupplierNo, @p_FromPort, @p_ToPort, @p_ShipmentDate";

            IEnumerable<ProdShipmentDto> result = await _dapperRepo.QueryAsync<ProdShipmentDto>(_sql, new
            {
                p_ShipmentNo = input.ShipmentNo,
                p_SupplierNo = input.SupplierNo,
                p_FromPort = input.FromPort,
                p_ToPort = input.ToPort,
                p_ShipmentDate = input.ShipmentDate
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdShipmentDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdShipmentToExcel(GetProdShipmentExportInput input)
        {
            string _sql = "Exec INV_PROD_SHIPMENT_SEARCH @p_ShipmentNo, @p_SupplierNo, @p_FromPort, @p_ToPort, @p_ShipmentDate";

            IEnumerable<ProdShipmentDto> result = await _dapperRepo.QueryAsync<ProdShipmentDto>(_sql, new
            {
                p_ShipmentNo = input.ShipmentNo,
                p_SupplierNo = input.SupplierNo,
                p_FromPort = input.FromPort,
                p_ToPort = input.ToPort,
                p_ShipmentDate = input.ShipmentDate
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task DeleteShipment(int? Id)
        {
            string _sql = "UPDATE ProdShipment SET IsDeleted = 1 WHERE Id = @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id
            });
        }

        public async Task AddShipment(ProdShipmentDto input)
        {
            string _sql = "Exec INV_PROD_SHIPMENT_ADD @p_ShipmentNo, @p_SupplierNo, @p_FromPort, @p_ToPort, @p_Buyer, @p_Forwarder, @p_Etd, @p_Eta, @p_OceanVesselName, @p_ShipmentDate, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_ShipmentNo = input.ShipmentNo,
                p_SupplierNo = input.SupplierNo,
                p_FromPort = input.FromPort,
                p_ToPort = input.ToPort,
                p_Buyer = input.Buyer,
                p_Forwarder = input.Forwarder,
                p_Etd = input.Etd,
                p_Eta = input.Eta,
                p_OceanVesselName = input.OceanVesselName,
                p_ShipmentDate = input.ShipmentDate,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task EditShipment(ProdShipmentDto input)
        {
            string _sql = "Exec INV_PROD_SHIPMENT_EDIT @p_ShipmentId, @p_Buyer, @p_FromPort, @p_ToPort, @p_ShipmentDate, " +
                "@p_Etd, @p_Eta, @p_OceanVesselName, @p_Status, @p_Forwarder, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_ShipmentId = input.Id,
                p_Buyer = input.Buyer,
                p_FromPort = input.FromPort,
                p_ToPort = input.ToPort,
                p_ShipmentDate = input.ShipmentDate,
                p_Etd = input.Etd,
                p_Eta = input.Eta,
                p_OceanVesselName = input.OceanVesselName,
                p_Status = input.Status,
                p_Forwarder = input.Forwarder,
                p_UserId = AbpSession.UserId
            });

        }

        public async Task UpdateStatusShipment(int? Id, string Status)
        {
            string _sql = "Exec INV_PROD_SHIPMENT_UPDATE_STATUS @p_ShipmentId, @p_Status, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_ShipmentId = Id,
                p_Status = Status,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
