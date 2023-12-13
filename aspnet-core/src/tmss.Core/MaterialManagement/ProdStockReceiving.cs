using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdStockReceiving")]
    public class ProdStockReceiving : FullAuditedEntity<long>, IEntity<long>//hàng đã nhập về kho
    {
        public const int MaxPartNoLength = 12;

        public const int MaxPartNameLength = 300;

        public const int MaxSupplierNoLength = 15;

        public const int MaxModelLength = 4;

        public const int MaxContainerNoLength = 15;

        public const int MaxInvoiceNoOutLength = 20;

        public const int MaxRequestStatusLength = 20;

        public const int MaxWarehouseLength = 2;

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual long? InvoiceDetailsId { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }

        public virtual int? ActualQty { get; set; }

        public virtual int? OrderQty { get; set; }

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxInvoiceNoOutLength)]
        public virtual string InvoiceNoOut { get; set; }

        [StringLength(MaxRequestStatusLength)]
        public virtual string RequestStatus { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        public virtual DateTime? DeliveryDate { get; set; }

        [StringLength(MaxWarehouseLength)]
        public virtual string Warehouse { get; set; }

        public virtual int? OrderedQty { get; set; }

    }
}
