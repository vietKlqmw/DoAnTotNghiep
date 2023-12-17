using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using tmss.Master.Material;
using tmss.Master.PartList;
using tmss.MaterialManagement.ContainerIntransit;
using tmss.MaterialManagement.ContainerWH;

namespace tmss.Web.Controllers
{
    public class ProductControllerBase : tmssControllerBase
    {
        private readonly IMasterMaterialAppService _importMasterMaterial;
        private readonly IProdContainerRentalWHPlanAppService _importContainerWarehouse;
        private readonly IMasterPartListAppService _importMasterPartList;
        private readonly IProdContainerIntransitAppService _importContainerIntransit;

        protected ProductControllerBase(
            IMasterMaterialAppService importMasterMaterial,
            IProdContainerRentalWHPlanAppService importContainerWarehouse,
            IMasterPartListAppService importMasterPartList,
            IProdContainerIntransitAppService importContainerIntransit
        )
        {
            _importMasterMaterial = importMasterMaterial;
            _importContainerWarehouse = importContainerWarehouse;
            _importMasterPartList = importMasterPartList;
            _importContainerIntransit = importContainerIntransit;
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

        [HttpPost]
        public async Task<JsonResult> ImportContainerWarehouseFromExcel()
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
                var material = await _importContainerWarehouse.ImportProdContainerRentalWHPlanFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { material }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportPartListMasterFromExcel()
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
                var material = await _importMasterPartList.ImportPartListFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { material }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportContainerIntransitFromExcel()
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
                var material = await _importContainerIntransit.ImportProdContainerIntransitFromExcel(fileBytes, file.FileName);
                return Json(new AjaxResponse(new { material }));

            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}
