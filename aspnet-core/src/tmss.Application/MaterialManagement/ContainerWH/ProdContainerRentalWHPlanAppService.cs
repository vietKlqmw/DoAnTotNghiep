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
using tmss.MaterialManagement.ContainerWH.Exporting;

namespace tmss.MaterialManagement.ContainerWH
{
    public class ProdContainerRentalWHPlanAppService : tmssAppServiceBase, IProdContainerRentalWHPlanAppService
    {
        private readonly IDapperRepository<ProdContainerRentalWHPlan, long> _dapperRepo;
        private readonly IProdContainerRentalWHPlanExcelExporter _excelExporter;

        public ProdContainerRentalWHPlanAppService(
            IDapperRepository<ProdContainerRentalWHPlan, long> dapperRepo,
            IProdContainerRentalWHPlanExcelExporter excelExporter
            )
        {
            _dapperRepo = dapperRepo;
            _excelExporter = excelExporter;
        }

        public async Task<PagedResultDto<ProdContainerRentalWHPlanDto>> GetProdContainerRentalWHPlanSearch(GetProdContainerRentalWHPlanInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_WAREHOUSE_SEARCH @p_ContainerNo, @p_InvoiceNo, @p_BillOfLadingNo, @p_SupplierNo, @p_SealNo, @p_RequestDateFrom, @p_RequestDateTo";

            IEnumerable<ProdContainerRentalWHPlanDto> result = await _dapperRepo.QueryAsync<ProdContainerRentalWHPlanDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_InvoiceNo = input.InvoiceNo,
                p_BillOfLadingNo = input.BillofladingNo,
                p_SupplierNo = input.SupplierNo,
                p_SealNo = input.SealNo,
                p_RequestDateFrom = input.RequestDateFrom,
                p_RequestDateTo = input.RequestDateTo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ProdContainerRentalWHPlanDto>(totalCount, pagedAndFiltered);
        }

        public async Task<FileDto> GetProdContainerRentalWHPlanToExcel(GetProdContainerRentalWHPlanExportInput input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_WAREHOUSE_SEARCH @p_ContainerNo, @p_InvoiceNo, @p_BillOfLadingNo, @p_SupplierNo, @p_SealNo, @p_RequestDateFrom, @p_RequestDateTo";

            IEnumerable<ProdContainerRentalWHPlanDto> result = await _dapperRepo.QueryAsync<ProdContainerRentalWHPlanDto>(_sql, new
            {
                p_ContainerNo = input.ContainerNo,
                p_InvoiceNo = input.InvoiceNo,
                p_BillOfLadingNo = input.BillofladingNo,
                p_SupplierNo = input.SupplierNo,
                p_SealNo = input.SealNo,
                p_RequestDateFrom = input.RequestDateFrom,
                p_RequestDateTo = input.RequestDateTo
            });

            var exportToExcel = result.ToList();

            return _excelExporter.ExportToFile(exportToExcel);
        }

        public async Task DeleteContWH(int? Id)
        {
            string _sql = "Exec INV_PROD_CONTAINER_WAREHOUSE_DELETE @p_Id";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = Id
            });
        }

        public async Task EditContainerWH(ProdContainerRentalWHPlanDto input)
        {
            string _sql = "Exec INV_PROD_CONTAINER_WAREHOUSE_EDIT @p_Id, @p_ContainerNo, @p_RequestDate, @p_RequestTime," +
                "@p_InvoiceNo, @p_BillOfLadingNo, @p_SupplierNo, @p_SealNo, @p_DevanningDate," +
                "@p_DevanningTime, @p_ActualDevanningDate, @p_GateInPlanTime, @p_GateInActualDateTime, @p_Transport, @p_Status, @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                p_Id = input.Id,
                p_ContainerNo = input.ContainerNo,
                p_RequestDate = input.RequestDate,
                p_RequestTime = input.RequestTime,
                p_InvoiceNo = input.InvoiceNo,
                p_BillOfLadingNo = input.BillofladingNo,
                p_SupplierNo = input.SupplierNo,
                p_SealNo = input.SealNo,
                p_DevanningDate = input.DevanningDate,
                p_DevanningTime = input.DevanningTime,
                p_ActualDevanningDate = input.ActualDevanningDate,
                p_GateInPlanTime = input.GateInPlanTime,
                p_GateInActualDateTime = input.GateInActualDateTime,
                p_Transport = input.Transport,
                p_Status = input.Status,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task<List<ProdContainerRentalWHPlanImportDto>> ImportProdContainerRentalWHPlanFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ProdContainerRentalWHPlanImportDto> listImport = new List<ProdContainerRentalWHPlanImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string v_devanning_date = (v_worksheet.Cells[2, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");

                    for (int i = 8; i < v_worksheet.Rows.Count; i++)
                    {
                        var row = new ProdContainerRentalWHPlanImportDto();
                        row.Guid = strGUID;
                        string v_time_devanning = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                        string v_container = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                        string v_seal = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                        string v_transport = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";

                        if (v_container != "")
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(v_devanning_date))
                                {
                                    row.DevanningDate = null;
                                }
                                else
                                {
                                    row.DevanningDate = DateTime.Parse(v_devanning_date);
                                }
                            }
                            catch
                            {
                                row.ErrorDescription += "Ngày nhận không hợp lệ! ";
                            }
                            if (v_container.Length > 15) row.ErrorDescription += "Độ dài Container No: " + v_container + " không hợp lệ! ";
                            else row.ContainerNo = v_container;

                            if (v_seal.Length > 20) row.ErrorDescription += "Độ dài Seal No:" + v_seal + " không hợp lệ! ";
                            else row.SealNo = v_seal;

                            row.DevanningTime = v_time_devanning;
                            row.Transport = v_transport;
                            row.CreatorUserId = AbpSession.UserId;
                            listImport.Add(row);
                        }
                    }
                }

                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<ProdContainerRentalWHPlanImportDto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "ProdContainerRentalWHPlan_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("ContainerNo", "ContainerNo");
                                bulkCopy.ColumnMappings.Add("SealNo", "SealNo");
                                bulkCopy.ColumnMappings.Add("Transport", "Transport");
                                bulkCopy.ColumnMappings.Add("DevanningDate", "DevanningDate");
                                bulkCopy.ColumnMappings.Add("DevanningTime", "DevanningTime");
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
            string _sql = "Exec INV_PROD_CONTAINER_WAREHOUSE_MERGE @Guid";
            await _dapperRepo.QueryAsync<ProdContainerRentalWHPlanImportDto>(_sql, new { Guid = v_Guid });
        }

        public async Task<PagedResultDto<ProdContainerRentalWHPlanImportDto>> GetListErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_PROD_CONTAINER_WAREHOUSE_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<ProdContainerRentalWHPlanImportDto> result = await _dapperRepo.QueryAsync<ProdContainerRentalWHPlanImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<ProdContainerRentalWHPlanImportDto>(totalCount, listResult);
        }
    }
}
