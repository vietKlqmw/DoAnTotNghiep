import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CarSeriesComponent } from './car-series.component';

const routes: Routes = [{
    path: '',
    component: CarSeriesComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CarSeriesRoutingModule { }
