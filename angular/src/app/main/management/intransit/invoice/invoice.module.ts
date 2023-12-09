import { NgModule } from '@angular/core';
import { InvoiceComponent } from './invoice.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { InvoiceRoutingModule } from './invoice-routing.module';
import { EditInvoiceModalComponent } from './edit-invoice-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        InvoiceRoutingModule
    ],
    declarations: [
        InvoiceComponent,
        EditInvoiceModalComponent
    ]
})
export class InvoiceModule { }
