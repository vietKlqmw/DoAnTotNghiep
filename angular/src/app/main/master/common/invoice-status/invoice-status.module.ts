import { NgModule } from '@angular/core';
import { InvoiceStatusComponent } from './invoice-status.component';
import { InvoiceStatusRoutingModule } from './invoice-status-routing.module';
import { AppSharedModule } from '@app/shared/app-shared.module';

@NgModule({
    imports: [
        AppSharedModule,
        InvoiceStatusRoutingModule
    ],
    declarations: [InvoiceStatusComponent]
})
export class InvoiceStatusModule { }
