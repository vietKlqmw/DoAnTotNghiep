using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Dto;

namespace tmss.Master.PartList.Exporting
{
    public interface IMasterPartListExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MasterPartListDto> listdata);
    }
}
