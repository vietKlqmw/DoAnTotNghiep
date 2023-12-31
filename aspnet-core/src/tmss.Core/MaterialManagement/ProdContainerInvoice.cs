﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdContainerInvoice")]
    public class ProdContainerInvoice : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxContainerNoLength = 15;

        public const int MaxSupplierNoLength = 10;

        public const int MaxSealNoLength = 20;

        public const int MaxStatusLength = 10;

        [StringLength(MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        public virtual long? InvoiceId { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxSealNoLength)]
        public virtual string SealNo { get; set; } //mã niêm chì -->gg search

        public virtual int? ContainerSize { get; set; } //kích thước container

        public virtual DateTime? PlanDevanningDate { get; set; } //ngày dỡ container theo kế hoạch

        public virtual DateTime? ActualDevanningDate { get; set; } //ngày dỡ container thực tế

        public virtual decimal? Thc { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        public virtual decimal? ThcVn { get; set; }

        public virtual decimal? Freight { get; set; } // Cước vẫn chuyển

        public virtual decimal? Insurance { get; set; } //Bảo hiểm

        public virtual decimal? Tax { get; set; } //thuế

        public virtual decimal? Amount { get; set; } //số lượng

        public virtual decimal? TaxVnd { get; set; }

        public virtual decimal? VatVnd { get; set; }
    }
}
