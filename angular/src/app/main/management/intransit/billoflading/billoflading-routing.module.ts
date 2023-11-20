import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BillofladingComponent } from './billoflading.component';

const routes: Routes = [{
    path: '',
    component: BillofladingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BillofladingRoutingModule { }
