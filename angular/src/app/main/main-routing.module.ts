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
                        data: { permission: 'Pages.Master.General.Warehouse.View' }
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
                        path: 'master/common/forwarder',
                        loadChildren: () => import('./master/common/forwarder/forwarder.module').then(m => m.ForwarderModule),
                        data: { permission: 'Pages.Master.General.Forwarder.View' }
                    },
                    {
                        path: 'master/common/product-type',
                        loadChildren: () => import('./master/common/product-type/product-type.module').then(m => m.ProductTypeModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/material',
                        loadChildren: () => import('./master/common/material/material.module').then(m => m.MaterialModule),
                        data: { permission: 'Pages.Master.General.Material.View' }
                    },
                    {
                        path: 'master/common/part-list',
                        loadChildren: () => import('./master/common/part-list/part-list.module').then(m => m.PartListModule),
                        data: { permission: 'Pages.Master.General.PartList.View' }
                    },
                    {
                        path: 'master/common/uom',
                        loadChildren: () => import('./master/common/uom/uom.module').then(m => m.UomModule),
                        //data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    },
                    {
                        path: 'master/common/carfamily',
                        loadChildren: () => import('./master/common/carfamily/carfamily.module').then(m => m.CarfamilyModule),
                        data: { permission: 'Pages.Master.General.Cfc.View' }
                    },
                    {
                        path: 'master/common/supplier-list',
                        loadChildren: () => import('./master/common/supplier-list/supplier-list.module').then(m => m.SupplierListModule),
                        data: { permission: 'Pages.Master.General.Supplier.View' }
                    },
//MANAGEMENT
                    //Intransit
                    {
                        path: 'management/intransit/shipment',
                        loadChildren: () => import('./management/intransit/shipment/shipment.module').then(m => m.ShipmentModule),
                        data: { permission: 'Pages.Management.Intransit.Shipment.View' }
                    },
                    {
                        path: 'management/intransit/billoflading',
                        loadChildren: () => import('./management/intransit/billoflading/billoflading.module').then(m => m.BillofladingModule),
                        data: { permission: 'Pages.Management.Intransit.Bill.View' }
                    },
                    {
                        path: 'management/intransit/invoice',
                        loadChildren: () => import('./management/intransit/invoice/invoice.module').then(m => m.InvoiceModule),
                        data: { permission: 'Pages.Management.Intransit.Invoice.View' }
                    },
                    {
                        path: 'management/intransit/customs-declare',
                        loadChildren: () => import('./management/intransit/customs-declare/customs-declare.module').then(m => m.CustomsDeclareModule),
                        data: { permission: 'Pages.Management.Intransit.CustomsDeclare.View' }
                    },
                    {
                        path: 'management/intransit/container-intransit',
                        loadChildren: () => import('./management/intransit/container-intransit/container-intransit.module').then(m => m.ContainerIntransitModule),
                        data: { permission: 'Pages.Management.Intransit.ContainerIntransit.View' }
                    },
                    {
                        path: 'management/intransit/order-part',
                        loadChildren: () => import('./management/intransit/order-part/order-part.module').then(m => m.OrderPartModule),
                        data: { permission: 'Pages.Management.Intransit.OrderPart.View' }
                    },
                    //Warehouse
                    {
                        path: 'management/warehouse/container-list',
                        loadChildren: () => import('./management/warehouse/container-list/container-list.module').then(m => m.ContainerListModule),
                        data: { permission: 'Pages.Management.Warehouse.ContainerList.View' }
                    },
                    {
                        path: 'management/warehouse/container-warehouse',
                        loadChildren: () => import('./management/warehouse/container-warehouse/container-warehouse.module').then(m => m.ContainerWarehouseModule),
                        data: { permission: 'Pages.Management.Warehouse.ContainerWarehouse.View' }
                    },
                    {
                        path: 'management/warehouse/stock-receiving',
                        loadChildren: () => import('./management/warehouse/stock-receiving/stock-receiving.module').then(m => m.StockReceivingModule),
                        data: { permission: 'Pages.Management.Warehouse.StockWarehouse.View' }
                    },
                    {
                        path: 'management/warehouse/invoice-stock-out',
                        loadChildren: () => import('./management/warehouse/invoice-stock-out/invoice-stock-out.module').then(m => m.InvoiceStockOutModule),
                        data: { permission: 'Pages.Management.Warehouse.InvoiceOut.View' }
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
