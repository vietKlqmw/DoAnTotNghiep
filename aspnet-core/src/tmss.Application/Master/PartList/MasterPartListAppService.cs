using Abp.Application.Services.Dto;
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
using tmss.Common;
using tmss.Dto;
using tmss.Master.PartList.Exporting;

namespace tmss.Master.PartList
{
    public class MasterPartListAppService : tmssAppServiceBase, IMasterPartListAppService
    {
        private readonly IDapperRepository<MasterPartList, long> _dapperRepo;
        private readonly IMasterPartListExcelExporter _excelExporter;

        public MasterPartListAppService(
            IDapperRepository<MasterPartList, long> dapperRepo,
            IMasterPartListExcelExporter excelExporter
                                         )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }
        public async Task<PagedResultDto<MasterPartListDto>> GetPartListSearch(GetMasterPartListInput input)
        {
            string _sql = "Exec INV_MASTER_PART_LIST_SEARCH @p_PartNo, @p_SupplierNo, @p_Cfc";

            IEnumerable<MasterPartListDto> result = await _dapperRepo.QueryAsync<MasterPartListDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_SupplierNo = input.SupplierNo,
                p_Cfc = input.CarfamilyCode
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MasterPartListDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetPartListToExcel(GetMasterPartListExportInput input)
        {
            string _sql = "Exec INV_MASTER_PART_LIST_SEARCH @p_PartNo, @p_SupplierNo, @p_Cfc";

            IEnumerable<MasterPartListDto> result = await _dapperRepo.QueryAsync<MasterPartListDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_SupplierNo = input.SupplierNo,
                p_Cfc = input.CarfamilyCode
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task DeletePartList(int? Id)
        {
            string _sql = "UPDATE MasterPartList SET IsDeleted = 1 WHERE Id = @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id
            });
        }

        public async Task EditPartList(MasterPartListDto input)
        {
            string _sql = "Exec INV_MASTER_PART_LIST_EDIT @p_PartListId, @p_PartNo, @p_PartName, @p_SupplierNo, @p_Cfc, " +
                "@p_Remark, @p_StartProductionMonth, @p_EndProductionMonth, @p_MaterialId, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_PartListId = input.Id,
                p_PartNo = input.PartNo,
                p_PartName = input.PartName,
                p_SupplierNo = input.SupplierNo,
                p_Cfc = input.CarfamilyCode,
                p_Remark = input.Remark,
                p_StartProductionMonth = input.StartProductionMonth,
                p_EndProductionMonth = input.EndProductionMonth,
                p_MaterialId = input.MaterialId,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task<List<MasterPartListImportDto>> ImportPartListFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MasterPartListImportDto> listImport = new List<MasterPartListImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    for (int i = 1; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_Cfc = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";

                        if (v_Cfc != "")
                        {
                            string v_PartNo = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_PartName = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_SupplierNo = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_StartProductionMonth = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_EndProductionMonth = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                            string v_Remark = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";

                            var row = new MasterPartListImportDto();
                            row.Guid = strGUID;
                            row.ErrorDescription = "";
                            row.CreatorUserId = AbpSession.UserId;

                            if (string.IsNullOrEmpty(v_PartNo))
                            {
                                row.ErrorDescription += "PartNo không được để trống! ";
                            }
                            else
                            {
                                if (v_PartNo.Length > 15)
                                {
                                    row.ErrorDescription += "PartNo dài quá 15 kí tự! ";
                                }
                                else
                                {
                                    row.PartNo = v_PartNo;
                                }
                            }

                            if (v_Cfc.Length > 4)
                            {
                                row.ErrorDescription += "CarfamilyCode dài quá 4 kí tự! ";
                            }
                            else
                            {
                                row.CarfamilyCode = v_Cfc;
                            }

                            if (string.IsNullOrEmpty(v_PartName))
                            {
                                row.ErrorDescription += "PartName không được để trống! ";
                            }
                            else
                            {
                                row.PartName = v_PartName;
                            }

                            if (string.IsNullOrEmpty(v_SupplierNo))
                            {
                                row.ErrorDescription += "SupplierNo không được để trống! ";
                            }
                            else
                            {
                                if (v_SupplierNo.Length > 10)
                                {
                                    row.ErrorDescription += "SupplierNo dài quá 10 kí tự! ";
                                }
                                else
                                {
                                    row.SupplierNo = v_SupplierNo;
                                }
                            }

                            row.Remark = v_Remark;

                            try
                            {
                                if (string.IsNullOrEmpty(v_StartProductionMonth)) row.StartProductionMonth = null;
                                else row.StartProductionMonth = DateTime.Parse(v_StartProductionMonth);
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "StartProductionMonth không đúng định dạng! ";
                            }
                            try
                            {
                                if (string.IsNullOrEmpty(v_EndProductionMonth)) row.EndProductionMonth = null;
                                else row.EndProductionMonth = DateTime.Parse(v_EndProductionMonth);
                                if (row.StartProductionMonth.HasValue && row.EndProductionMonth < row.StartProductionMonth)
                                {
                                    row.ErrorDescription += "EndProductionMonth phải sau StartProductionMonth! ";
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "EndProductionMonth không đúng định dạng! ";
                            }

                            listImport.Add(row);
                        }
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<MasterPartListImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "MasterPartList_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("CarfamilyCode", "CarfamilyCode");
                                    bulkCopy.ColumnMappings.Add("Remark", "Remark");
                                    bulkCopy.ColumnMappings.Add("StartProductionMonth", "StartProductionMonth");
                                    bulkCopy.ColumnMappings.Add("EndProductionMonth", "EndProductionMonth");
                                    bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                    bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");

                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }
                    return listImport;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public async Task MergeDataPartList(string v_Guid)
        {
            string _sql = "Exec INV_MASTER_PART_LIST_MERGE @Guid";
            await _dapperRepo.QueryAsync<MasterPartListImportDto>(_sql, new { Guid = v_Guid });
        }

        public async Task<PagedResultDto<MasterPartListImportDto>> GetListErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_MASTER_PART_LIST_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<MasterPartListImportDto> result = await _dapperRepo.QueryAsync<MasterPartListImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MasterPartListImportDto>(totalCount, listResult);
        }
    }
}
