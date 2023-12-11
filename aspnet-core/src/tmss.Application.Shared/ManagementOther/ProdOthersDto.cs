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
        public virtual string ListContId { get; set; }
        public virtual string ReceiveDate { get; set; }
        public virtual string GoodsReceivedNoteNo { get; set; }
        public virtual string ListForwarder { get; set; }
        public virtual string ListInvoice { get; set; }
        public virtual string Warehouse { get; set; }
        public virtual string Address { get; set; }
        public virtual bool IsExcel { get; set; }
        public virtual DateTime? WorkingDate { get; set; }
    }
}
