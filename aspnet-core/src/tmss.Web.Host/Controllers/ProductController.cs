using tmss.Master.Material;
using tmss.MaterialManagement.ContainerWH;

namespace tmss.Web.Controllers
{
    public class ProductController : ProductControllerBase
    {
        public ProductController(
            IMasterMaterialAppService importMasterMaterial,
            IProdContainerRentalWHPlanAppService importContainerWarehouse
        ) : base(
            importMasterMaterial,
            importContainerWarehouse
        )
        { }
    }
}
