using tmss.Master.Material;
using tmss.Master.PartList;
using tmss.MaterialManagement.ContainerIntransit;
using tmss.MaterialManagement.ContainerWH;

namespace tmss.Web.Controllers
{
    public class ProductController : ProductControllerBase
    {
        public ProductController(
            IMasterMaterialAppService importMasterMaterial,
            IProdContainerRentalWHPlanAppService importContainerWarehouse,
            IMasterPartListAppService importMasterPartList,
            IProdContainerIntransitAppService importContainerIntransit
        ) : base(
            importMasterMaterial,
            importContainerWarehouse,
            importMasterPartList,
            importContainerIntransit
        )
        { }
    }
}
