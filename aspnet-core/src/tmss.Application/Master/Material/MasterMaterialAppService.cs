﻿using Abp.Application.Services.Dto;
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
using tmss.Master.Material.Exporting;

namespace tmss.Master.Material
{
    [AbpAuthorize(AppPermissions.Pages_Master_General_Material_View)]
    public class MasterMaterialAppService : tmssAppServiceBase, IMasterMaterialAppService
    {
        private readonly IDapperRepository<MasterMaterial, long> _dapperRepo;
        private readonly IMasterMaterialExcelExporter _excelExporter;

        public MasterMaterialAppService(
            IDapperRepository<MasterMaterial, long> dapperRepo,
            IMasterMaterialExcelExporter excelExporter
                                         )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<MasterMaterialDto>> GetMaterialSearch(GetMasterMaterialInput input)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_SEARCH @p_MaterialCode, @p_MaterialGroup";

            IEnumerable<MasterMaterialDto> result = await _dapperRepo.QueryAsync<MasterMaterialDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialGroup = input.MaterialGroup

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MasterMaterialDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetMaterialMasterToExcel(GetMasterMaterialExportInput input)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_SEARCH @p_MaterialCode, @p_MaterialGroup";

            IEnumerable<MasterMaterialDto> result = await _dapperRepo.QueryAsync<MasterMaterialDto>(_sql, new
            {
                p_MaterialCode = input.MaterialCode,
                p_MaterialGroup = input.MaterialGroup

            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task EditInfoMaterial(MasterMaterialDto input)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_EDIT @p_MaterialId, @p_MaterialType, " +
                "@p_MaterialCode, @p_Description, @p_MaterialGroup, @p_BaseUnitOfMeasure, " +
                "@p_StorageLocation, @p_ProductionType, @p_StandardPrice, @p_MovingPrice, " +
                "@p_MaterialOrigin, @p_EffectiveDateFrom, @p_EffectiveDateTo, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_MaterialId = input.Id,
                p_MaterialType = input.MaterialType,
                p_MaterialCode = input.MaterialCode,
                p_Description = input.Description,
                p_MaterialGroup = input.MaterialGroup,
                p_BaseUnitOfMeasure = input.BaseUnitOfMeasure,
                p_StorageLocation = input.StorageLocation,
                p_ProductionType = input.ProductionType,
                p_StandardPrice = input.StandardPrice,
                p_MovingPrice = input.MovingPrice,
                p_MaterialOrigin = input.MaterialOrigin,
                p_EffectiveDateFrom = input.EffectiveDateFrom,
                p_EffectiveDateTo = input.EffectiveDateTo,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task DeleteMaterial(int? MaterialId)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_DELETE @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = MaterialId
            });
        }

        public async Task<List<MasterMaterialImportDto>> ImportMaterialFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MasterMaterialImportDto> listImport = new List<MasterMaterialImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    for (int i = 1; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_MaterialCode = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                        if (v_MaterialCode != "")
                        {
                            string v_MaterialType = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_Description = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_MaterialGroup = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_BaseUOM = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_StorageLocation = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                            string v_ProductionType = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_StandardPrice = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                            string v_MovingPrice = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                            string v_MaterialOrigin = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                            string v_EffectiveDateFrom = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                            string v_EffectiveDateTo = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";

                            var row = new MasterMaterialImportDto();
                            row.Guid = strGUID;
                            row.ErrorDescription = "";
                            row.CreatorUserId = AbpSession.UserId;

                            if (string.IsNullOrEmpty(v_MaterialType))
                            {
                                row.ErrorDescription += "Material Type không được để trống! ";
                            }
                            else
                            {
                                row.MaterialType = v_MaterialType;
                            }

                            if (string.IsNullOrEmpty(v_MaterialCode))
                            {
                                row.ErrorDescription += "Material Code không được để trống! ";
                            }
                            else
                            {
                                row.MaterialCode = v_MaterialCode;
                            }

                            row.Description = v_Description;
                            row.MaterialGroup = v_MaterialGroup;
                            row.BaseUnitOfMeasure = v_BaseUOM;
                            row.StorageLocation = v_StorageLocation;
                            row.ProductionType = v_ProductionType;
                            try
                            {
                                if (string.IsNullOrEmpty(v_StandardPrice)) row.StandardPrice = null;
                                else row.StandardPrice = decimal.Parse(v_StandardPrice);
                                if (row.StandardPrice < 0)
                                {
                                    row.ErrorDescription += "Standard Price không được âm! ";
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Standard Price phải là số! ";
                            }
                            try
                            {
                                if (string.IsNullOrEmpty(v_MovingPrice)) row.MovingPrice = null;
                                else row.MovingPrice = decimal.Parse(v_MovingPrice);
                                if (row.MovingPrice < 0)
                                {
                                    row.ErrorDescription += "Moving Price không được âm! ";
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Moving Price phải là số! ";
                            }
                            row.MaterialOrigin = v_MaterialOrigin;
                            try
                            {
                                if (string.IsNullOrEmpty(v_EffectiveDateFrom)) row.EffectiveDateFrom = null;
                                else row.EffectiveDateFrom = DateTime.Parse(v_EffectiveDateFrom);
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Effective Date From không đúng định dạng! ";
                            }
                            try
                            {
                                if (string.IsNullOrEmpty(v_EffectiveDateTo)) row.EffectiveDateTo = null;
                                else row.EffectiveDateTo = DateTime.Parse(v_EffectiveDateTo);
                                if (row.EffectiveDateFrom.HasValue && row.EffectiveDateTo < row.EffectiveDateFrom)
                                {
                                    row.ErrorDescription += "Effective Date To phải sau Effective Date From! ";
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Effective Date To không đúng định dạng! ";
                            }

                            listImport.Add(row);
                        }
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<MasterMaterialImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "MasterMaterial_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("MaterialType", "MaterialType");
                                    bulkCopy.ColumnMappings.Add("MaterialCode", "MaterialCode");
                                    bulkCopy.ColumnMappings.Add("Description", "Description");
                                    bulkCopy.ColumnMappings.Add("MaterialGroup", "MaterialGroup");
                                    bulkCopy.ColumnMappings.Add("BaseUnitOfMeasure", "BaseUnitOfMeasure");
                                    bulkCopy.ColumnMappings.Add("StorageLocation", "StorageLocation");
                                    bulkCopy.ColumnMappings.Add("ProductionType", "ProductionType");
                                    bulkCopy.ColumnMappings.Add("StandardPrice", "StandardPrice");
                                    bulkCopy.ColumnMappings.Add("MovingPrice", "MovingPrice");
                                    bulkCopy.ColumnMappings.Add("MaterialOrigin", "MaterialOrigin");
                                    bulkCopy.ColumnMappings.Add("EffectiveDateFrom", "EffectiveDateFrom");
                                    bulkCopy.ColumnMappings.Add("EffectiveDateTo", "EffectiveDateTo");
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

        public async Task MergeDataMaterial(string v_Guid)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_MERGE @Guid";
            await _dapperRepo.QueryAsync<MasterMaterialImportDto>(_sql, new { Guid = v_Guid });
        }

        public async Task<PagedResultDto<MasterMaterialImportDto>> GetListErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_MASTER_MATERIAL_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<MasterMaterialImportDto> result = await _dapperRepo.QueryAsync<MasterMaterialImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MasterMaterialImportDto>(totalCount, listResult);
        }
    }
}
