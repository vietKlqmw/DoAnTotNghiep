using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdCustomsDeclare")]
    public class ProdCustomsDeclare : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCustomsDeclareNoLength = 20;

        public const int MaxStatusLength = 4;

        public const int MaxForwarderLength = 10;

        public const int MaxToWarehouseLength = 20;


        [StringLength(MaxCustomsDeclareNoLength)]
        public virtual string CustomsDeclareNo { get; set; }

        public virtual DateTime? DeclareDate { get; set; }

        public virtual decimal? Tax { get; set; }

        public virtual decimal? Vat { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxForwarderLength)]
        public virtual string Forwarder { get; set; }

        public virtual long? BillId { get; set; }

        public virtual long? InvoiceId { get; set; }

        [StringLength(MaxToWarehouseLength)]
        public virtual string ToWarehouse { get; set; }
    }
}
