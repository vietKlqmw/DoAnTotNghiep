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
using tmss.MaterialManagement.OrderPart.Exporting;

namespace tmss.MaterialManagement.OrderPart
{
    [AbpAuthorize(AppPermissions.Pages_Management_Intransit_OrderPart_View)]
    public class ProdOrderPartAppService : tmssAppServiceBase, IProdOrderPartAppService
    {
        private readonly IDapperRepository<ProdOrderPart, long> _dapperRepo;
        private readonly IProdOrderPartExcelExporter _excelExporter;

        public ProdOrderPartAppService(
            IDapperRepository<ProdOrderPart, long> dapperRepo,
            IProdOrderPartExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdOrderPartDto>> GetProdOrderPartSearch(GetProdOrderPartInput input)
        {
            string _sql = "Exec INV_PROD_ORDER_PART_SEARCH @p_PartNo, @p_SupplierNo, @p_Cfc, @p_ContainerNo, @p_ShimentNo, @p_Status, @p_OrderDateFrom, @p_OrderDateTo";

            IEnumerable<ProdOrderPartDto> result = await _dapperRepo.QueryAsync<ProdOrderPartDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_SupplierNo = input.SupplierNo,
                p_Cfc = input.CarfamilyCode,
                p_ContainerNo = input.ContainerNo,
                p_ShimentNo = input.ShipmentNo,
                p_Status = input.Status,
                p_OrderDateFrom = input.OrderDateFrom,
                p_OrderDateTo = input.OrderDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdOrderPartDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdOrderPartToExcel(GetProdOrderPartExportInput input)
        {
            string _sql = "Exec INV_PROD_ORDER_PART_SEARCH @p_PartNo, @p_SupplierNo, @p_Cfc, @p_ContainerNo, @p_ShimentNo, @p_Status, @p_OrderDateFrom, @p_OrderDateTo";

            IEnumerable<ProdOrderPartDto> result = await _dapperRepo.QueryAsync<ProdOrderPartDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_SupplierNo = input.SupplierNo,
                p_Cfc = input.CarfamilyCode,
                p_ContainerNo = input.ContainerNo,
                p_ShimentNo = input.ShipmentNo,
                p_Status = input.Status,
                p_OrderDateFrom = input.OrderDateFrom,
                p_OrderDateTo = input.OrderDateTo
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task DeleteOrder(int? Id)
        {
            string _sql = "Exec INV_PROD_ORDER_PART_DELETE @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id
            });
        }

        public async Task EditOrderPart(ProdOrderPartDto input)
        {
            string _sql = "Exec INV_PROD_ORDER_PART_EDIT @p_OrderId, @p_PartNo, @p_PartName, @p_SupplierNo, @p_Cfc, " +
                "@p_Status, @p_Remark, @p_Qty, @p_AmountUnit, @p_TotalAmount, @p_OrderDate, @p_BOM, @p_MaterialId, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_OrderId = input.Id,
                p_PartNo = input.PartNo,
                p_PartName = input.PartName,
                p_SupplierNo = input.SupplierNo,
                p_Cfc = input.CarfamilyCode,
                p_Status = input.Status,
                p_Remark = input.Remark,
                p_Qty = input.Qty,
                p_AmountUnit = input.AmountUnit,
                p_TotalAmount = input.TotalAmount,
                p_OrderDate = input.OrderDate,
                p_BOM = input.BaseUnitOfMeasure,
                p_MaterialId = input.MaterialId,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task<List<ProdOrderPartImportDto>> ImportProdOrderPartFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ProdOrderPartImportDto> listImport = new List<ProdOrderPartImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    for (int i = 1; i < v_worksheet.Rows.Count; i++)
                    {
                        var row = new ProdOrderPartImportDto();
                        row.Guid = strGUID;
                        string v_PartNo = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                        string v_PartName = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                        string v_Supplier = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                        string v_Cfc = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                        string v_OrderDate = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                        string v_Qty = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                        string v_Remark = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";

                        if (v_PartNo != "")
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(v_OrderDate))
                                {
                                    row.OrderDate = null;
                                }
                                else
                                {
                                    row.OrderDate = DateTime.Parse(v_OrderDate);
                                }
                            }
                            catch
                            {
                                row.ErrorDescription += "OrderDate không hợp lệ! ";
                            }

                            if (v_PartNo.Length > 12) row.ErrorDescription += "Độ dài PartNo: " + v_PartNo + " không hợp lệ! ";
                            else row.PartNo = v_PartNo;

                            if (string.IsNullOrEmpty(v_PartName)) row.ErrorDescription += "PartName không được để trống! ";
                            else row.PartName = v_PartName;

                            if (string.IsNullOrEmpty(v_Cfc)) row.ErrorDescription += "Cfc không được để trống! ";
                            else if (v_Cfc.Length > 4) row.ErrorDescription += "Độ dài Cfc: " + v_Cfc + " không hợp lệ! ";
                            else row.CarfamilyCode = v_Cfc;

                            if (string.IsNullOrEmpty(v_Supplier)) row.ErrorDescription += "Supplier không được để trống! ";
                            else if (v_Supplier.Length > 10) row.ErrorDescription += "Độ dài Supplier: " + v_Supplier + " không hợp lệ! ";
                            else row.SupplierNo = v_Supplier;

                            try
                            {
                                if (string.IsNullOrEmpty(v_Qty)) row.Qty = null;
                                else row.Qty = int.Parse(v_Qty);
                                if (row.Qty < 0)
                                {
                                    row.ErrorDescription += "Qty không được âm! ";
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Qty phải là số! ";
                            }

                            row.Remark = v_Remark;
                            row.CreatorUserId = AbpSession.UserId;
                            listImport.Add(row);
                        }
                    }
                }

                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<ProdOrderPartImportDto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "ProdOrderPart_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                bulkCopy.ColumnMappings.Add("CarfamilyCode", "CarfamilyCode");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                bulkCopy.ColumnMappings.Add("OrderDate", "OrderDate");
                                bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                bulkCopy.ColumnMappings.Add("Remark", "Remark");
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
            string _sql = "Exec INV_PROD_ORDER_PART_MERGE @Guid";
            await _dapperRepo.QueryAsync<ProdOrderPartImportDto>(_sql, new { Guid = v_Guid });
        }

        public async Task<PagedResultDto<ProdOrderPartImportDto>> GetListErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_PROD_ORDER_PART_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<ProdOrderPartImportDto> result = await _dapperRepo.QueryAsync<ProdOrderPartImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<ProdOrderPartImportDto>(totalCount, listResult);
        }
    }
}
