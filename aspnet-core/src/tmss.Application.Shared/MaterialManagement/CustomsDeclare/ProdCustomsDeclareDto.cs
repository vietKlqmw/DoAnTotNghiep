using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.CustomsDeclare
{
    public class ProdCustomsDeclareDto : EntityDto<long?>
    {
        public virtual string CustomsDeclareNo { get; set; }
        public virtual DateTime? DeclareDate { get; set; }
        public virtual decimal? Tax { get; set; }
        public virtual decimal? Vat { get; set; }
        public virtual string Status { get; set; }
        public virtual string Forwarder { get; set; }
        public virtual long? BillId { get; set; }

        public virtual string BillOfLadingNo { get; set; }
        public virtual DateTime? BillDate { get; set; }
        public virtual decimal? SumCustomsDeclare { get; set; }
    }

    public class GetProdCustomsDeclareInput : PagedAndSortedResultRequestDto
    {
        public virtual string CustomsDeclareNo { get; set; }
        public virtual DateTime? DeclareDate { get; set; }
        public virtual string BillOfLadingNo { get; set; }
    }

    public class GetProdCustomsDeclareExportInput
    {
        public virtual string CustomsDeclareNo { get; set; }
        public virtual DateTime? DeclareDate { get; set; }
        public virtual string BillOfLadingNo { get; set; }
    }
}
