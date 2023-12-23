import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ContainerWarehouseRoutingModule } from './container-warehouse-routing.module';
import { ContainerWarehouseComponent } from './container-warehouse.component';
import { EditContainerWarehouseComponent } from './edit-container-warehouse-modal.component';
import { ListErrorImportModalComponent } from './list-error-import-container-warehouse-modal.component';
import { ImportContainerWarehouseComponent } from './import-container-warehouse-modal.component';
import { AddGrnContWarehouseModalComponent } from './add-grn-container-warehouse-modal.component';
import { ViewHistoryReceiveModalComponent } from './view-history-receive-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        ContainerWarehouseRoutingModule
    ],
    declarations: [
        ContainerWarehouseComponent,
        EditContainerWarehouseComponent,
        ListErrorImportModalComponent,
        ImportContainerWarehouseComponent,
        AddGrnContWarehouseModalComponent,
        ViewHistoryReceiveModalComponent
    ]
})
export class ContainerWarehouseModule { }
