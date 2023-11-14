import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransmissionTypeComponent } from './transmission-type.component';

const routes: Routes = [{
    path: '',
    component: TransmissionTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TransmissionTypeRoutingModule { }
