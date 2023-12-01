import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockReceivingComponent } from './stock-receiving.component';
const routes: Routes = [{
    path: '',
    component: StockReceivingComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StockReceivingRoutingModule { }
