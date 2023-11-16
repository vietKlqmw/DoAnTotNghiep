using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.Shipment
{
    public class ProdShipmentDto : EntityDto<long?>
    {
        public virtual string ShipmentNo { get; set; }

        public virtual string ShippingcompanyCode { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Buyer { get; set; }

        public virtual string FromPort { get; set; }

        public virtual string ToPort { get; set; }

        public virtual string ShipmentDate { get; set; }

        public virtual DateTime? Etd { get; set; }

        public virtual DateTime? Eta { get; set; }

        public virtual DateTime? Ata { get; set; }

        public virtual string OceanVesselName { get; set; }

        public virtual DateTime? Atd { get; set; }

        public virtual string Status { get; set; }
    }

    public class GetProdShipmentInput : PagedAndSortedResultRequestDto
    {
        public virtual string ShipmentNo { get; set; }

        public virtual string ShippingcompanyCode { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string FromPort { get; set; }

        public virtual string ToPort { get; set; }

        public virtual string ShipmentDate { get; set; }
    }

    public class GetProdShipmentExportInput
    {
        public virtual string ShipmentNo { get; set; }

        public virtual string ShippingcompanyCode { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string FromPort { get; set; }

        public virtual string ToPort { get; set; }

        public virtual string ShipmentDate { get; set; }
    }
}
