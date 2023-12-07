using tmss.Master.Material;
using tmss.Master.PartList;
using tmss.MaterialManagement.ContainerWH;

namespace tmss.Web.Controllers
{
    public class ProductController : ProductControllerBase
    {
        public ProductController(
            IMasterMaterialAppService importMasterMaterial,
            IProdContainerRentalWHPlanAppService importContainerWarehouse,
            IMasterPartListAppService importMasterPartList
        ) : base(
            importMasterMaterial,
            importContainerWarehouse,
            importMasterPartList
        )
        { }
    }
}
