import { NgModule } from '@angular/core';
import { ContainerInvoiceComponent } from './container-invoice.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ContainerInvoiceRoutingModule } from './container-invoice-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        ContainerInvoiceRoutingModule
    ],
    declarations: [ContainerInvoiceComponent]
})
export class ContainerInvoiceModule { }
