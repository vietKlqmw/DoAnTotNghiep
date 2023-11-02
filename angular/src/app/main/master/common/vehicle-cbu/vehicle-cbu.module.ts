import { NgModule } from '@angular/core';
import { VehicleCbuComponent } from './vehicle-cbu.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { VehicleCbuRoutingModule } from './vehicle-cbu-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        VehicleCbuRoutingModule
    ],
    declarations: [VehicleCbuComponent]
})
export class VehicleCbuModule { }
