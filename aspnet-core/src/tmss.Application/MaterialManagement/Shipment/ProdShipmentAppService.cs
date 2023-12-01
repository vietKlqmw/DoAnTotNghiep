using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
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
            string _sql = "Exec INV_PROD_SHIPMENT_ADD @p_ShipmentNo, @p_SupplierNo, @p_FromPort, @p_ToPort, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_ShipmentNo = input.ShipmentNo,
                p_SupplierNo = input.SupplierNo,
                p_FromPort = input.FromPort,
                p_ToPort = input.ToPort,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task CreateOrEdit(ProdShipmentDto input)
        {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            
        }
    }
}
