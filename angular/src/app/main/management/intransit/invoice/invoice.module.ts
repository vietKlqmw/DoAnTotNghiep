import { NgModule } from '@angular/core';
import { InvoiceComponent } from './invoice.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { InvoiceRoutingModule } from './invoice-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        InvoiceRoutingModule
    ],
    declarations: [InvoiceComponent]
})
export class InvoiceModule { }
