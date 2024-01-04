import { NgModule } from '@angular/core';
import { StorageLocationComponent } from './storage-location.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { StorageLocationRoutingModule } from './storage-location-routing.module';
import { EditWarehouseModalComponent } from './edit-storage-location-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        StorageLocationRoutingModule
    ],
    declarations: [
        StorageLocationComponent,
        EditWarehouseModalComponent
    ]
})
export class StorageLocationModule { }
