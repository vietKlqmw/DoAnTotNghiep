import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: 'dashboard', component: DashboardComponent, data: { permission: 'Pages.Tenant.Dashboard' } },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
//MASTER
                    //Common
                    {
                        path: 'master/common/vehicle-cbu',
                        loadChildren: () => import('./master/common/vehicle-cbu/vehicle-cbu.module').then(m => m.VehicleCbuModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/container-status',
                        loadChildren: () => import('./master/common/container-status/container-status.module').then(m => m.ContainerStatusModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/customs-status',
                        loadChildren: () => import('./master/common/customs-status/customs-status.module').then(m => m.CustomsStatusModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/invoice-status',
                        loadChildren: () => import('./master/common/invoice-status/invoice-status.module').then(m => m.InvoiceStatusModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    { path: '**', redirectTo: 'dashboard' }
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class MainRoutingModule { }
