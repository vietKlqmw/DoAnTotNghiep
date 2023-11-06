import { NgModule } from '@angular/core';
import { StorageLocationComponent } from './storage-location.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { StorageLocationRoutingModule } from './storage-location-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        StorageLocationRoutingModule
    ],
    declarations: [StorageLocationComponent]
})
export class StorageLocationModule { }
