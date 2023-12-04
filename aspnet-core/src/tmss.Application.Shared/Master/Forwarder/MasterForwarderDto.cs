using Abp.Application.Services.Dto;

namespace tmss.Master.Forwarder
{
    public class MasterForwarderDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual long? SupplierId { get; set; }

        public virtual string SupplierNo { get; set; }
    }

    public class GetMasterForwarderInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string SupplierNo { get; set; }
    }
}
