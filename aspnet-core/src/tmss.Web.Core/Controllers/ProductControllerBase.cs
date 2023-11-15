using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using tmss.Master.Material;

namespace tmss.Web.Controllers
{
    public class ProductControllerBase : tmssControllerBase
    {
        private readonly IMasterMaterialAppService _importMasterMaterial;

        protected ProductControllerBase(
            IMasterMaterialAppService importMasterMaterial
        )
        {
            _importMasterMaterial = importMasterMaterial;
        }

        [HttpPost]
        public async Task<JsonResult> ImportMaterialMasterFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }
                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }
                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var material = await _importMasterMaterial.ImportMaterialFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { material }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}
