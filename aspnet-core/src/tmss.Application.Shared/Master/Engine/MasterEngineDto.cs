using Abp.Application.Services.Dto;

namespace tmss.Master.Engine
{
    public class MasterEngineDto : EntityDto<long?>
    {
        public virtual string MaterialCode { get; set; }

        public virtual string ClassType { get; set; }

        public virtual string ClassName { get; set; }

        public virtual string TransmissionType { get; set; }

        public virtual string EngineModel { get; set; }

        public virtual string EngineType { get; set; }
    }

    public class GetMasterEngineInput : PagedAndSortedResultRequestDto
    {
        public virtual string MaterialCode { get; set; }

        public virtual string TransmissionType { get; set; }

        public virtual string EngineModel { get; set; }

        public virtual string EngineType { get; set; }
    }

    public class GetMasterEngineExportInput
    {
        public virtual string MaterialCode { get; set; }

        public virtual string TransmissionType { get; set; }

        public virtual string EngineModel { get; set; }

        public virtual string EngineType { get; set; }
    }
}
