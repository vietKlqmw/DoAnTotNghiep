using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdContainerIntransit")]
    public class ProdContainerIntransit : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxContainerNoLength = 15;

        public const int MaxSupplierNoLength = 10;

        public const int MaxStatusLength = 10;

        public const int MaxForwarderLength = 10;


        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? TransactionDate { get; set; } 

        public virtual DateTime? TmvDate { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxForwarderLength)]
        public virtual string Forwarder { get; set; }

        public virtual long? ShipmentId { get; set; }
    }
}
