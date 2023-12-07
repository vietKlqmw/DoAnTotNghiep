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
}
