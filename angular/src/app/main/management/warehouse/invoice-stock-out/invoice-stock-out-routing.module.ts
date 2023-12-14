import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvoiceStockOutComponent } from './invoice-stock-out.component';

const routes: Routes = [{
    path: '',
    component: InvoiceStockOutComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class InvoiceStockOutRoutingModule { }
