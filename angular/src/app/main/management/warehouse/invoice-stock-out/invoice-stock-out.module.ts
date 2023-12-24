import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { InvoiceStockOutComponent } from './invoice-stock-out.component';
import { InvoiceStockOutRoutingModule } from './invoice-stock-out-routing.module';
import { ViewInvoiceStockModalComponent } from './view-invoice-stock-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        InvoiceStockOutRoutingModule
    ],
    declarations: [
        InvoiceStockOutComponent,
        ViewInvoiceStockModalComponent
    ]
})
export class InvoiceStockOutModule { }
