import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { SupplierListRoutingModule } from './supplier-list-routing.module';
import { SupplierListComponent } from './supplier-list.component';

@NgModule({
    imports: [
        AppSharedModule,
        SupplierListRoutingModule
    ],
    declarations: [SupplierListComponent]
})
export class SupplierListModule { }
