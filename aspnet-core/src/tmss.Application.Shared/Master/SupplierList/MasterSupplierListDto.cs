using Abp.Application.Services.Dto;

namespace tmss.Master.SupplierList
{
    public class MasterSupplierListDto : EntityDto<long?>
    {
        public virtual string SupplierNo { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string SupplierType { get; set; }

        public virtual string SupplierNameVn { get; set; }

        public virtual string Exporter { get; set; }
    }

    public class GetMasterSupplierListInput : PagedAndSortedResultRequestDto
    {
        public virtual string SupplierNo { get; set; }

        public virtual string SupplierName { get; set; }
    }
}
