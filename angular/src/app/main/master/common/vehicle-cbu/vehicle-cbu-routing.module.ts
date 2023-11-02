import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VehicleCbuComponent } from './vehicle-cbu.component';

const routes: Routes = [{
    path: '',
    component: VehicleCbuComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class VehicleCbuRoutingModule { }
