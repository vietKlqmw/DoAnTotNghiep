﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace tmss.MaterialManagement.ContainerWH
{
    public interface IProdContainerRentalWHPlanAppService : IApplicationService
    {
        Task<PagedResultDto<ProdContainerRentalWHPlanDto>> GetProdContainerRentalWHPlanSearch(GetProdContainerRentalWHPlanInput input);
    }
}
