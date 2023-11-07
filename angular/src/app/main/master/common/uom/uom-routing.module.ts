import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UomComponent } from './uom.component';

const routes: Routes = [{
    path: '',
    component: UomComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class UomRoutingModule { }
