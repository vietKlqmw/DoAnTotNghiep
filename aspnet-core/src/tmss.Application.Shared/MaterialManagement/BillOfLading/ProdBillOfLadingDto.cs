using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.BillOfLading
{
    public class ProdBillOfLadingDto : EntityDto<long?>
    {
        public virtual string BillofladingNo { get; set; }

        public virtual long? ShipmentId { get; set; }

        public virtual DateTime? BillDate { get; set; }

        public virtual string StatusCode { get; set; }
    }

    public class GetProdBillOfLadingInput : PagedAndSortedResultRequestDto
    {
        public virtual string BillofladingNo { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }

        public virtual DateTime? BillDateTo { get; set; }
    }

    public class GetProdBillOfLadingExportInput
    {
        public virtual string BillofladingNo { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }

        public virtual DateTime? BillDateTo { get; set; }
    }
}
