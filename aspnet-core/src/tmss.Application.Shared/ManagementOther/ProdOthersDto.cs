namespace tmss.ManagementOther
{
    public class ProdOthersDto
    {
    }

    public class ListSupplierDto
    {
        public virtual string SupplierNo { get; set; }
        public virtual string SupplierName { get; set; }
    }

    public class ListForwarderBySupplierIdDto
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
    }

    public class ListStatusContDto
    {
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
    }
}
