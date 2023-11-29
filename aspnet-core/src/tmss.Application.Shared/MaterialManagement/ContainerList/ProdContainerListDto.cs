﻿using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.ContainerList
{
    public class ProdContainerListDto : EntityDto<long?>
    {
        public virtual string ContainerNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string BillOfLadingNo { get; set; }
        public virtual string SealNo { get; set; }
        public virtual int? ContainerSize { get; set; }
        public virtual long? ShipmentId { get; set; }
        public virtual DateTime? ShippingDate { get; set; }
        public virtual DateTime? PortDate { get; set; }
        public virtual DateTime? PortDateActual { get; set; }
        public virtual DateTime? PortTransitDate { get; set; }
        public virtual DateTime? ReceiveDate { get; set; }
        public virtual long? RequestId { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string ListLotNo { get; set; }
        public virtual string ListCaseNo { get; set; }
        public virtual string Transport { get; set; }
        public virtual DateTime? DevanningDate { get; set; }
        public virtual string DevanningTime { get; set; }
        public virtual string Remark { get; set; }
        public virtual string WhLocation { get; set; }
        public virtual DateTime? GateInDate { get; set; }
        public virtual string GateInTime { get; set; }
        public virtual long? TransitPortReqId { get; set; }
        public virtual DateTime? TransitPortReqDate { get; set; }
        public virtual string TransitPortReqTime { get; set; }
        public virtual decimal? Freight { get; set; }
        public virtual decimal? Insurance { get; set; }
        public virtual decimal? Cif { get; set; }
        public virtual decimal? Tax { get; set; }
        public virtual decimal? Amount { get; set; }
        public virtual string Status { get; set; }
        public virtual string LocationCode { get; set; }
        public virtual DateTime? LocationDate { get; set; }
        public virtual long? ReceivingPeriodId { get; set; }
        public virtual string OrdertypeCode { get; set; }
        public virtual string GoodstypeCode { get; set; }
        public virtual long? RentalWhId { get; set; }
        public virtual string RequestStatus { get; set; }
        public virtual DateTime? BillDate { get; set; }
        public virtual string ReceiveTime { get; set; }

        public virtual decimal? GrandFreight { get; set; }
        public virtual decimal? GrandInsurance { get; set; }
        public virtual decimal? GrandCif { get; set; }
        public virtual decimal? GrandTax { get; set; }
        public virtual decimal? GrandAmount { get; set; }
    }

    public class GetProdContainerListInput : PagedAndSortedResultRequestDto
    {
        public virtual string ContainerNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string BillOfLadingNo { get; set; }
        public virtual DateTime? PortDateFrom { get; set; }
        public virtual DateTime? PortDateTo { get; set; }
        public virtual DateTime? ReceiveDateFrom { get; set; }
        public virtual DateTime? ReceiveDateTo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }
        public virtual string ContainerStatus { get; set; }
    }

    public class GetProdContainerListExportInput
    {
        public virtual string ContainerNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string BillOfLadingNo { get; set; }
        public virtual DateTime? PortDateFrom { get; set; }
        public virtual DateTime? PortDateTo { get; set; }
        public virtual DateTime? ReceiveDateFrom { get; set; }
        public virtual DateTime? ReceiveDateTo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }
        public virtual string ContainerStatus { get; set; }
    }
}