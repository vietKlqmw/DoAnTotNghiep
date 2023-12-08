using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdInvoice")]
    public class ProdInvoice : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxInvoiceNoLength = 20;

        public const int MaxStatusLength = 10;

        public const int MaxForwarderLength = 10;

        [StringLength(MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; } //mã hóa đơn

        public virtual long? BillId { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        [StringLength(MaxForwarderLength)]
        public virtual string Forwarder { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }
    }
}
