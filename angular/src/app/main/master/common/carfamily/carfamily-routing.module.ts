import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CarfamilyComponent } from './carfamily.component';

const routes: Routes = [{
    path: '',
    component: CarfamilyComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CarfamilyRoutingModule { }
