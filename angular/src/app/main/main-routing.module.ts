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
                    {
                        path: 'master/common/storage-location',
                        loadChildren: () => import('./master/common/storage-location/storage-location.module').then(m => m.StorageLocationModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/factory',
                        loadChildren: () => import('./master/common/factory/factory.module').then(m => m.FactoryModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/material-group',
                        loadChildren: () => import('./master/common/material-group/material-group.module').then(m => m.MaterialGroupModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/material-type',
                        loadChildren: () => import('./master/common/material-type/material-type.module').then(m => m.MaterialTypeModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/product-group',
                        loadChildren: () => import('./master/common/product-group/product-group.module').then(m => m.ProductGroupModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/product-type',
                        loadChildren: () => import('./master/common/product-type/product-type.module').then(m => m.ProductTypeModule),
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
