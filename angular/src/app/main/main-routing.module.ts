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
                    {
                        path: 'master/common/material',
                        loadChildren: () => import('./master/common/material/material.module').then(m => m.MaterialModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/uom',
                        loadChildren: () => import('./master/common/uom/uom.module').then(m => m.UomModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/fuel-type',
                        loadChildren: () => import('./master/common/fuel-type/fuel-type.module').then(m => m.FuelTypeModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/carfamily',
                        loadChildren: () => import('./master/common/carfamily/carfamily.module').then(m => m.CarfamilyModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/car-series',
                        loadChildren: () => import('./master/common/car-series/car-series.module').then(m => m.CarSeriesModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/engine-type',
                        loadChildren: () => import('./master/common/engine-type/engine-type.module').then(m => m.EngineTypeModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/engine-model',
                        loadChildren: () => import('./master/common/engine-model/engine-model.module').then(m => m.EngineModelModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/transmission-type',
                        loadChildren: () => import('./master/common/transmission-type/transmission-type.module').then(m => m.TransmissionTypeModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
//MANAGEMENT
                    //Intransit
                    {
                        path: 'management/intransit/shipment',
                        loadChildren: () => import('./management/intransit/shipment/shipment.module').then(m => m.ShipmentModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'management/intransit/billoflading',
                        loadChildren: () => import('./management/intransit/billoflading/billoflading.module').then(m => m.BillofladingModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'management/intransit/container-invoice',
                        loadChildren: () => import('./management/intransit/container-invoice/container-invoice.module').then(m => m.ContainerInvoiceModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'management/intransit/invoice',
                        loadChildren: () => import('./management/intransit/invoice/invoice.module').then(m => m.InvoiceModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    //Warehouse
                    {
                        path: 'management/warehouse/container-list',
                        loadChildren: () => import('./management/warehouse/container-list/container-list.module').then(m => m.ContainerListModule),
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
