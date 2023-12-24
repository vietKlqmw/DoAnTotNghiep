using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tmss.MaterialManagement
{
    [Table("ProdInvoiceStockOut")]
    public class ProdInvoiceStockOut : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxInvoiceNoOutLength = 20;

        public const int MaxStatusLength = 50;

        public const int MaxPartNoLength = 12;

        public const int MaxPartNameLength = 300;

        public const int MaxModelLength = 4;

        public const int MaxStockIdLength = 1000;

        public const int MaxGoodsDeliveryNoteLength = 20;

        [StringLength(MaxInvoiceNoOutLength)]
        public virtual string InvoiceNoOut { get; set; }

        public virtual DateTime? InvoiceDate { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxPartNoLength)]
        public virtual string ListPartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string ListPartName { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string ListCfc { get; set; }

        [StringLength(MaxStockIdLength)]
        public virtual string ListStockId { get; set; }

        public virtual int? TotalDeliveryQty { get; set; }

        public virtual int? TotalOrderQty { get; set; }

        public virtual decimal? TotalAmount { get; set; }

        [StringLength(MaxGoodsDeliveryNoteLength)]
        public virtual string GoodsDeliveryNoteNo { get; set; }
    }
}
