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

        [StringLength(MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(MaxPartNameLength)]
        public virtual string PartName { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? TransactionDatetime { get; set; }

        public virtual long? InvoiceDetailsId { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(MaxModelLength)]
        public virtual string Model { get; set; }
    }
}
