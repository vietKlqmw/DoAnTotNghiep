﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.Master.InvoiceStatus
{
    [Table("MasterInvoiceStatus")]
    public class MasterInvoiceStatus : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCodeLength = 20;

        public const int MaxDescriptionLength = 50;

        [StringLength(MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }
    }
}
