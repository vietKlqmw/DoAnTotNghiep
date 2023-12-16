using Abp.Application.Services.Dto;
using System;

namespace tmss.ManagementOther
{
    public class ProdOthersDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
    }

    public class ListSupplierDto
    {
        public virtual string SupplierNo { get; set; }
        public virtual string SupplierName { get; set; }
    }

    public class ListForwarderBySupplierIdDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
    }

    public class ListStatusContDto
    {
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
    }

    public class ListMaterialUsageDto
    {
        public virtual long? MaterialId { get; set; }
        public virtual string MaterialCode { get; set; }
    }

    public class ListShipmentNewOrPendingDto
    {
        public virtual long? ShipmentId { get; set; }
        public virtual string ShipmentNo { get; set; }

        public virtual string SupplierNo { get; set; }
        public virtual string Buyer { get; set; }
        public virtual string FromPort { get; set; }
        public virtual string ToPort { get; set; }
        public virtual DateTime? ShipmentDate { get; set; }
        public virtual DateTime? Etd { get; set; }
        public virtual DateTime? Eta { get; set; }
        public virtual DateTime? Ata { get; set; }
        public virtual string OceanVesselName { get; set; }
        public virtual DateTime? Atd { get; set; }
        public virtual string Status { get; set; }
    }


    public class GetListPartDto
    {
        public virtual int? PartId { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
    }

    public class GetListWarehouse
    {
        public virtual string StorageLocation { get; set; }

        public virtual string AddressLanguageVn { get; set; }
    }

    public class GoodsReceivedNoteExportInput
    {
        public virtual string ContId { get; set; }
        public virtual string ListContId { get; set; }
        public virtual string ReceiveDate { get; set; }
        public virtual string GoodsReceivedNoteNo { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual string Address { get; set; }
        public virtual bool IsExcel { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
        public virtual string[] ListActualQty { get; set; }
    }

    public class GoodsDeliveryNoteExportInput
    {
        public virtual string StockId { get; set; }
        public virtual string ListStockId { get; set; }
        public virtual string DeliveryDate { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
        public virtual string GoodsDeliveryNoteNo { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual string Address { get; set; }
        public virtual bool IsExcel { get; set; }
        public virtual string[] ListActualDeliveryQty { get; set; }
        public virtual string[] ListDeliveryQty { get; set; }
    }

    public class ListPartForOrderDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set;}
        public virtual string SupplierNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual int? Qty { get; set; }
        public virtual decimal? StandardPrice { get; set; }
        public virtual decimal? MovingPrice { get; set; }
        public virtual decimal? Amount { get; set; }

        public virtual string KeyRow
        {
            get { return string.Format("{0}_{1}", PartNo, PartName); }
            set { }
        }
        public virtual int? OrderQty
        {
            get { return Qty; }
            set { }
        }
        public virtual decimal? OrderAmount
        {
            get { return OrderQty == null ? Amount : OrderQty * StandardPrice + MovingPrice; }
            set { }
        }

        public virtual int? GrandOrderQty { get; set; }
        public virtual decimal? GrandOrderAmount { get; set; }
    }
}
