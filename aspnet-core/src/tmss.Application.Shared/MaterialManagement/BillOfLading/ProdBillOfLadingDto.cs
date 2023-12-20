using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.BillOfLading
{
    public class ProdBillOfLadingDto : EntityDto<long?>
    {
        public virtual string BillofladingNo { get; set; }

        public virtual long? ShipmentId { get; set; }

        public virtual DateTime? BillDate { get; set; }

        public virtual string StatusCode { get; set; }

        public virtual string ShipmentNo { get; set; }

        public virtual DateTime? ShipmentDate { get; set; }

        //for view 
        public virtual string Forwarder { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string FromPort { get;set; }
        public virtual string ToPort { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string Cfc { get;set; }
        public virtual string OceanVesselName { get; set; }
    }

    public class GetProdBillOfLadingInput : PagedAndSortedResultRequestDto
    {
        public virtual string BillofladingNo { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }

        public virtual DateTime? BillDateTo { get; set; }
    }

    public class GetProdBillOfLadingExportInput
    {
        public virtual string BillofladingNo { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }

        public virtual DateTime? BillDateTo { get; set; }
    }
}
