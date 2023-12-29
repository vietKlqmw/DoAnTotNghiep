import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { OrderPartRoutingModule } from './order-part-routing.module';
import { OrderPartComponent } from './order-part.component';
import { EditOrderPartModalComponent } from './edit-order-part-modal.component';
import { ListErrorImportModalComponent } from './list-error-import-order-part-modal.component';
import { ImportContainerWarehouseComponent } from './import-order-part-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        OrderPartRoutingModule
    ],
    declarations: [
        OrderPartComponent,
        EditOrderPartModalComponent,
        ListErrorImportModalComponent,
        ImportContainerWarehouseComponent
    ]
})
export class OrderPartModule { }
