﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdOrderPart_T")]
    public class ProdOrderPart_T : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxPartNoLength = 15;

        public const int MaxPartNameLength = 500;

        public const int MaxSupplierNoLength = 10;

        public const int MaxCfcLength = 4;

        public const int MaxRemarkLength = 5000;

        [StringLength(128)]
        public virtual string Guid { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxCfcLength)]
        public virtual string CarfamilyCode { get; set; }

        [StringLength(MaxRemarkLength)]
        public virtual string Remark { get; set; }

        public virtual int? Qty { get; set; }
        public virtual DateTime? OrderDate { get; set; }

        [StringLength(5000)]
        public string ErrorDescription { get; set; }
    }
}
