using tmss.Master.Material;

namespace tmss.Web.Controllers
{
    public class ProductController : ProductControllerBase
    {
        public ProductController(
            IMasterMaterialAppService importMasterMaterial
        ) : base(
            importMasterMaterial
        )
        { }
    }
}
