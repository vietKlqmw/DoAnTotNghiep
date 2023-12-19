using Abp.Dapper.Repositories;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using tmss.Master.Material;
using tmss.MaterialManagement.ContainerIntransit;
using tmss.MaterialManagement.Invoice;
using tmss.MaterialManagement.InvoiceStock;
using tmss.MaterialManagement.StockReceiving;

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

        public async Task<List<ListShipmentNewOrPendingDto>> GetListShipmentNewOrPending()
        {
            string _sql = "Exec INV_PROD_SHIPMENT_GET_LIST_NEW_OR_PENDING";
            IEnumerable<ListShipmentNewOrPendingDto> result = await _dapperRepo.QueryAsync<ListShipmentNewOrPendingDto>(_sql);

            return result.ToList();
        }

        public async Task<List<ListShipmentNewOrPendingDto>> GetListShipmentById(long? Id)
        {
            string _sql = "Exec INV_PROD_SHIPMENT_GET_BY_ID @p_Id";

            IEnumerable<ListShipmentNewOrPendingDto> result = await _dapperRepo.QueryAsync<ListShipmentNewOrPendingDto>(_sql, new
            {
                p_Id = Id
            });

            return result.ToList();
        }

        public async Task<List<ProdContainerIntransitDto>> GetListContainerForShipment(string SupplierNo)
        {
            string _sql = "Exec INV_PROD_GET_LIST_CONTAINER_FOR_SHIPMENT @p_SupplierNo";

            IEnumerable<ProdContainerIntransitDto> result = await _dapperRepo.QueryAsync<ProdContainerIntransitDto>(_sql, new
            {
                p_SupplierNo = SupplierNo
            });

            return result.ToList();
        }

        public async Task<List<GetListPartDto>> GetListPart()
        {
            string _sql = "SELECT Id PartId, (CarfamilyCode + '/' + PartName) PartName, SupplierNo From MasterPartList WHERE IsDeleted = 0";

            IEnumerable<GetListPartDto> result = await _dapperRepo.QueryAsync<GetListPartDto>(_sql);

            return result.ToList();
        }

        public async Task<List<ProdInvoiceDto>> GetListInvociePreDeclared()
        {
            string _sql = "Exec INV_PROD_INVOICE_CUSTOMS_DECLARED";

            IEnumerable<ProdInvoiceDto> result = await _dapperRepo.QueryAsync<ProdInvoiceDto>(_sql);

            return result.ToList();
        }

        public async Task<List<ProdInvoiceDto>> GetListContForWarehouse()
        {
            string _sql = "Exec INV_PROD_GET_LIST_CONTAINER_TO_WAREHOUSE";

            IEnumerable<ProdInvoiceDto> result = await _dapperRepo.QueryAsync<ProdInvoiceDto>(_sql);

            return result.ToList();
        }

        public async Task<List<ProdStockReceivingDto>> GetListStockForDeliveryByWarehouse(string warehouse)
        {
            string _sql = "Exec INV_PROD_GET_STOCK_FOR_DELIVERY_BY_WAREHOUSE @p_Warehouse";

            IEnumerable<ProdStockReceivingDto> result = await _dapperRepo.QueryAsync<ProdStockReceivingDto>(_sql, new
            {
                p_Warehouse = warehouse
            });

            return result.ToList();
        }

        public async Task<List<GetListWarehouse>> GetListWarehouse()
        {
            string _sql = "Exec INV_PROD_GET_LIST_WAREHOUSE";

            IEnumerable<GetListWarehouse> result = await _dapperRepo.QueryAsync<GetListWarehouse>(_sql);

            return result.ToList();
        }

        public async Task<List<ListPartForOrderDto>> GetListPartForOrder(string listpart)
        {
            string _sql = "Exec INV_PROD_GET_LIST_ORDER_BY_ID @p_ListPartId";

            IEnumerable<ListPartForOrderDto> result = await _dapperRepo.QueryAsync<ListPartForOrderDto>(_sql, new
            {
                p_ListPartId = listpart
            });

            var listResult = result.ToList();

            if (listResult.Count > 0)
            {
                listResult[0].GrandOrderQty = listResult.Sum(e => e.OrderQty);
                listResult[0].GrandOrderAmount = listResult.Sum(e => e.OrderAmount);
            }

            return listResult;
        }

        public async Task<GetDataDashboardTop> GetDataForDashboardTop(string warehouse)
        {
            string _sql = "Exec INV_PROD_DASHBOARD_TOP @p_Warehouse";

            return (await _dapperRepo.QueryAsync<GetDataDashboardTop>(_sql, new { p_Warehouse = warehouse })).FirstOrDefault();
        }

        public async Task<List<GetDataDashboardNewContToWarehouse>> GetDataForDashboardNewContToWarehouse(string warehouse)
        {
            string _sql = "Exec INV_PROD_DASHBOARD_NEW_CONT_TO_WAREHOUSE @p_Warehouse";

            IEnumerable<GetDataDashboardNewContToWarehouse> result = await _dapperRepo.QueryAsync<GetDataDashboardNewContToWarehouse>(_sql, new
            {
                p_Warehouse = warehouse
            });

            return result.ToList();
        }

        public async Task<List<ProdInvoiceStockOutDto>> GetDataForDashboardStockOut(string warehouse)
        {
            string _sql = "Exec INV_PROD_DASHBOARD_STOCK_OUT @p_Warehouse";

            IEnumerable<ProdInvoiceStockOutDto> result = await _dapperRepo.QueryAsync<ProdInvoiceStockOutDto>(_sql, new
            {
                p_Warehouse = warehouse
            });

            return result.ToList();
        }

        public async Task<List<GetDataDashboardQtyOut>> GetDataForDashboardQtyOut(string type)
        {
            string _sql = "Exec INV_PROD_DASHBOARD_QTY_OUT @p_Type";

            IEnumerable<GetDataDashboardQtyOut> result = await _dapperRepo.QueryAsync<GetDataDashboardQtyOut>(_sql, new
            {
                p_Type = type
            });

            return result.ToList();
        }



        //excel to pdf
        public string ConvertExcelToPdf(string filePathSource, string filePathSave, string nameSave)
        {
            //kiểm tra định dạng file
            string name = Path.GetFileName(filePathSource);
            if (!Regex.IsMatch(name, ".(\\.xlsx|\\.xls)"))
            {
                return null;
            }

            //lấy ra tên file
            string nameFile = "";
            if (string.IsNullOrEmpty(nameSave))
            {
                nameFile = Path.GetFileNameWithoutExtension(filePathSource);
            }
            else
            {
                nameFile = nameSave;
            }

            //đường dẫn lưu file convert
            string pathToSave = "";
            if (string.IsNullOrEmpty(filePathSave))//nếu filePathSave null thì lưu cùng vị trí với file source
            {
                pathToSave = Path.Combine(Path.GetDirectoryName(filePathSource), nameFile);
            }
            else
            {
                pathToSave = Path.Combine(filePathSave, nameFile);
            }

            string result;
            ApplicationClass application = null;
            Workbook workBook = null;
            try
            {
                application = new ApplicationClass();
                object target = pathToSave;
                workBook = application.Workbooks.Open(filePathSource);//mở file cần convert

                ((_Worksheet)workBook.ActiveSheet).PageSetup.Orientation = XlPageOrientation.xlLandscape;// landscape

                workBook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, target);//convert
                result = pathToSave;
            }
            catch
            {
                result = null;
            }
            finally
            {
                if (workBook != null)
                {
                    workBook.Close(true);
                    workBook = null;
                }
                if (application != null)
                {
                    application.Quit();
                    application = null;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return result;

        }
    }
}
