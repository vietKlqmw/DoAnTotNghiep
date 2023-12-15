using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdContainerList")]
    public class ProdContainerList : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxContainerNoLength = 20;

        public const int MaxSupplierNoLength = 10;

        public const int MaxTransportLength = 10;

        public const int MaxRemarkLength = 1000;

        public const int MaxStatusLength = 20;

        public const int MaxRequestStatusLength = 20;


        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual long? BillId { get; set; }

        public virtual long? ShipmentId { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual long? InvoiceId { get; set; }

        [StringLength(MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual decimal? Freight { get; set; }

        public virtual decimal? Insurance { get; set; }

        public virtual decimal? Cif { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? Amount { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxRequestStatusLength)]
        public virtual string RequestStatus { get; set; }
    }
}
