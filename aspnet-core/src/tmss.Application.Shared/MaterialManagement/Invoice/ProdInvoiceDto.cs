using Abp.Application.Services.Dto;
using System;

namespace tmss.MaterialManagement.Invoice
{
    public class ProdInvoiceDto : EntityDto<long?>
    {
        public virtual string InvoiceNo { get; set; } //mã hóa đơn
        public virtual long? BillId { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
        public virtual decimal? Freight { get; set; } // Cước vẫn chuyển
        public virtual decimal? FreightTotal { get; set; }
        public virtual decimal? Insurance { get; set; } //Bảo hiểm
        public virtual decimal? InsuranceTotal { get; set; }
        public virtual decimal? Cif { get; set; } //Cost + Insurance + Freight
        public virtual decimal? ThcTotal { get; set; } //phí xếp dỡ bến
        public virtual decimal? NetWeight { get; set; } //khối lượng tịnh
        public virtual decimal? GrossWeight { get; set; } //khối lượng toàn phần
        public virtual string Currency { get; set; } //Tiền tệ
        public virtual string SupplierNo { get; set; }
        public virtual int? Quantity { get; set; } //số lượng
        public virtual string Status { get; set; }
        public virtual decimal? FreightTotalVn { get; set; }
        public virtual decimal? InsuranceTotalVn { get; set; }
        public virtual decimal? CifVn { get; set; }
        public virtual decimal? ThcTotalVn { get; set; }

        public virtual string ShipmentNo { get; set; }
        public virtual string BillNo { get; set; }
        public virtual DateTime? BillDate { get; set; }
    }

    public class GetProdInvoiceInput : PagedAndSortedResultRequestDto
    {
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime? InvoiceDateFrom { get; set; }
        public virtual DateTime? InvoiceDateTo { get; set; }
        public virtual string BillNo { get; set; }
        public virtual string ShipmentNo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }
        public virtual string SupplierNo { get; set; }

    }

    public class GetProdInvoiceExportInput
    {
        public virtual string InvoiceNo { get; set; }
        public virtual DateTime? InvoiceDateFrom { get; set; }
        public virtual DateTime? InvoiceDateTo { get; set; }
        public virtual string BillNo { get; set; }
        public virtual string ShipmentNo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual DateTime? BillDateFrom { get; set; }
        public virtual DateTime? BillDateTo { get; set; }
        public virtual string SupplierNo { get; set; }

    }

    public class ProdInvoiceDetailsDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }
        public virtual string ModuleNo { get; set; }
        public virtual decimal? Insurance { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual long? InvoiceId { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual decimal? Freight { get; set; }
        public virtual decimal? Thc { get; set; }
        public virtual decimal? Cif { get; set; }
        public virtual decimal? Tax { get; set; } //thuế
        public virtual decimal? TaxRate { get; set; } //% thuế
        public virtual decimal? Vat { get; set; } //giá trị gia tăng
        public virtual decimal? VatRate { get; set; } // % GTGT
        public virtual int? UsageQty { get; set; }
        public virtual string PartName { get; set; }
        public virtual string CarfamilyCode { get; set; }
        public virtual decimal? PartNetWeight { get; set; } //khối lượng tịnh của part
        public virtual DateTime? PackagingDate { get; set; } //ngày đóng gói
        public virtual string Status { get; set; }
        public virtual decimal? FreightVn { get; set; }
        public virtual decimal? InsuranceVn { get; set; }
        public virtual decimal? ThcVn { get; set; }
        public virtual decimal? CifVn { get; set; }
        public virtual decimal? TaxVn { get; set; }
        public virtual decimal? VatVn { get; set; }
        public virtual string PartnameVn { get; set; }

        public virtual double? GrandQty { get; set; }
        public virtual decimal? GrandCif { get; set; }
        public virtual decimal? GrandFreight { get; set; }
        public virtual decimal? GrandInsurance { get; set; }
        public virtual decimal? GrandTax { get; set; }
        public virtual decimal? GrandVat { get; set; }
    }

    public class GetProdInvoiceDetailsInput : PagedAndSortedResultRequestDto
    {
        public virtual long? InvoiceId { get; set; }

    }

    public class GetProdInvoiceDetailsExportInput
    {
        public virtual long? InvoiceId { get; set; }

    }
}
