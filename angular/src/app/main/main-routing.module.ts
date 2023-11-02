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
