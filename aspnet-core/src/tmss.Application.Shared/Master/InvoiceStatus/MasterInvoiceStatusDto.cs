using Abp.Application.Services.Dto;

namespace tmss.Master.InvoiceStatus
{
    public class MasterInvoiceStatusDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Description { get; set; }
    }

    public class GetInvoiceStatusInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }
    }
}
