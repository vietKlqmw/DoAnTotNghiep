using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.OrderPart
{
    public class ProdOrderPartDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string CarfamilyCode { get; set; }
        public virtual long? SupplierId { get; set; }
        public virtual long? MaterialId { get; set; }
        public virtual string Status { get; set; }
        public virtual string Remark { get; set; }
        public virtual long? ShipmentId { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual int? Qty { get; set; }
        public virtual decimal? AmountUnit { get; set; }
        public virtual decimal? TotalAmount { get; set; }
        public virtual DateTime? OrderDate { get; set; }

        public virtual string ShipmentNo { get; set; }
    }

    public class GetProdOrderPartInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string CarfamilyCode { get; set; }
        public virtual string Status { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string ShipmentNo { get; set; }
        public virtual DateTime? OrderDateFrom { get; set; }
        public virtual DateTime? OrderDateTo { get; set; }
    }

    public class GetProdOrderPartExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string CarfamilyCode { get; set; }
        public virtual string Status { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string ShipmentNo { get; set; }
        public virtual DateTime? OrderDateFrom { get; set; }
        public virtual DateTime? OrderDateTo { get; set; }
    }
}
