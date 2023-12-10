using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdContainerRentalWHPlan")]
    public class ProdContainerRentalWHPlan : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxContainerNoLength = 15;

        public const int MaxSupplierNoLength = 10;

        public const int MaxTransportLength = 50;

        public const int MaxStatusLength = 10;

        public const int MaxWarehouseLength = 2;


        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual DateTime? DeliveryDate { get; set; }

        [StringLength(MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxWarehouseLength)]
        public virtual string Warehouse { get; set; }

        public virtual long? InvoiceId { get; set; }

        public virtual long? BillId { get; set; }
    }
}
