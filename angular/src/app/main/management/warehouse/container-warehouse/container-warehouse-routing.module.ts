import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContainerWarehouseComponent } from './container-warehouse.component';
const routes: Routes = [{
    path: '',
    component: ContainerWarehouseComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContainerWarehouseRoutingModule { }
