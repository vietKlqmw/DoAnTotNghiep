using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.ContainerIntransit
{
    public class ProdContainerIntransitDto : EntityDto<long?>
    {
        public virtual string ContainerNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? TransactionDate { get; set; }

        public virtual string Forwarder { get; set; }

        public virtual long? ShipmentId { get; set; }
    }

    public class GetProdContainerIntransitInput : PagedAndSortedResultRequestDto
    {
        public virtual string ContainerNo { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? TransactionDate { get; set; }

    }

    public class GetProdContainerIntransitExportInput
    {
        public virtual string ContainerNo { get; set; }

        public virtual DateTime? ShippingDate { get; set; }

        public virtual DateTime? PortDate { get; set; }

        public virtual DateTime? TransactionDate { get; set; }

    }
}
