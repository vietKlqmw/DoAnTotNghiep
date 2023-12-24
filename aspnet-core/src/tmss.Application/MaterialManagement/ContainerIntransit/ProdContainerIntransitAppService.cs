using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tmss.Authorization;
using tmss.Common;
using tmss.Dto;
using tmss.MaterialManagement.ContainerIntransit.Exporting;

namespace tmss.MaterialManagement.ContainerIntransit
{
    [AbpAuthorize(AppPermissions.Pages_Management_Intransit_ContainerIntransit_View)]
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
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_SEARCH @p_ContainerNo, @p_ShippingDateFrom, @p_ShippingDateTo, @p_PortDateFrom, @p_PortDateTo, @p_Status";

            IEnumerable<ProdContainerIntransitDto> result = await _dapperRepo.QueryAsync<ProdContainerIntransitDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_ShippingDateFrom = input.ShippingDateFrom,
                p_ShippingDateTo = input.ShippingDateTo,
                p_PortDateFrom = input.PortDateFrom,
                p_PortDateTo = input.PortDateTo,
                p_Status = input.Status
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdContainerIntransitDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdContainerIntransitToExcel(GetProdContainerIntransitExportInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_SEARCH @p_ContainerNo, @p_ShippingDateFrom, @p_ShippingDateTo, @p_PortDateFrom, @p_PortDateTo, @p_Status";

            IEnumerable<ProdContainerIntransitDto> result = await _dapperRepo.QueryAsync<ProdContainerIntransitDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_ShippingDateFrom = input.ShippingDateFrom,
                p_ShippingDateTo = input.ShippingDateTo,
                p_PortDateFrom = input.PortDateFrom,
                p_PortDateTo = input.PortDateTo,
                p_Status = input.Status
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task DeleteContainerIntransit(int? Id)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_DELETE @p_Id, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task EditContainerIntransit(ProdContainerIntransitDto input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_EDIT @p_ContId, @p_ContainerNo, @p_SupplierNo, @p_PartListId, @p_UsageQty, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_ContId = input.Id,
                p_ContainerNo = input.ContainerNo,
                p_SupplierNo = input.SupplierNo,
                p_PartListId = input.PartListId,
                p_UsageQty = input.UsageQty,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task<List<ProdContainerIntransitImportDto>> ImportProdContainerIntransitFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ProdContainerIntransitImportDto> listImport = new List<ProdContainerIntransitImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    for (int i = 1; i < v_worksheet.Rows.Count; i++)
                    {
                        var row = new ProdContainerIntransitImportDto();
                        row.Guid = strGUID;
                        string v_ContainerNo = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                        string v_SupplierNo = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                        string v_PartNo = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                        string v_Cfc = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                        string v_UsageQty = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";

                        if (v_ContainerNo != "")
                        {
                            if (string.IsNullOrEmpty(v_ContainerNo)) row.ErrorDescription += "Container No không được để trống! ";
                            else if (v_ContainerNo.Length > 20) row.ErrorDescription += "Độ dài Container No: " + v_ContainerNo + " không hợp lệ! ";
                            else row.ContainerNo = v_ContainerNo;

                            if (string.IsNullOrEmpty(v_SupplierNo)) row.ErrorDescription += "Supplier No không được để trống! ";
                            else if (v_SupplierNo.Length > 10) row.ErrorDescription += "Độ dài Supplier No:" + v_SupplierNo + " không hợp lệ! ";
                            else row.SupplierNo = v_SupplierNo;

                            if (string.IsNullOrEmpty(v_PartNo)) row.ErrorDescription += "Part No không được để trống! ";
                            else if (v_PartNo.Length > 15) row.ErrorDescription += "Độ dài Part No:" + v_PartNo + " không hợp lệ! ";
                            else row.PartNo = v_PartNo;

                            if (string.IsNullOrEmpty(v_Cfc)) row.ErrorDescription += "Carfamily Code không được để trống! ";
                            else if (v_Cfc.Length > 4) row.ErrorDescription += "Độ dài Carfamily Code:" + v_Cfc + " không hợp lệ! ";
                            else row.CarfamilyCode = v_Cfc;

                            try
                            {
                                if (string.IsNullOrEmpty(v_UsageQty)) row.UsageQty = null;
                                else row.UsageQty = int.Parse(v_UsageQty);
                                if (row.UsageQty < 0)
                                {
                                    row.ErrorDescription += "UsageQty không được âm! ";
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "UsageQty phải là số! ";
                            }

                            row.CreatorUserId = AbpSession.UserId;
                            listImport.Add(row);
                        }
                    }
                }

                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<ProdContainerIntransitImportDto> dataE = listImport.AsEnumerable();
                    DataTable table = new DataTable();
                    using (var reader = ObjectReader.Create(dataE))
                    {
                        table.Load(reader);
                    }
                    string connectionString = Commons.getConnectionString();
                    using (Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();

                        using (Microsoft.Data.SqlClient.SqlTransaction tran = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            using (var bulkCopy = new Microsoft.Data.SqlClient.SqlBulkCopy(conn, Microsoft.Data.SqlClient.SqlBulkCopyOptions.Default, tran))
                            {
                                bulkCopy.DestinationTableName = "ProdContainerIntransit_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("ContainerNo", "ContainerNo");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("CarfamilyCode", "CarfamilyCode");
                                bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");
                                bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");
                                bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                bulkCopy.WriteToServer(table);
                                tran.Commit();
                            }
                        }
                        await conn.CloseAsync();
                    }
                }
                return listImport;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public async Task MergeData(string v_Guid)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_MERGE @Guid";
            await _dapperRepo.QueryAsync<ProdContainerIntransitImportDto>(_sql, new { Guid = v_Guid });
        }

        public async Task<PagedResultDto<ProdContainerIntransitImportDto>> GetListErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_PROD_CONTAINER_INTRANSIT_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<ProdContainerIntransitImportDto> result = await _dapperRepo.QueryAsync<ProdContainerIntransitImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<ProdContainerIntransitImportDto>(totalCount, listResult);
        }
    }
}
