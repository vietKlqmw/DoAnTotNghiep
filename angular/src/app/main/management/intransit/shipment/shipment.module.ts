import { NgModule } from '@angular/core';
import { ShipmentComponent } from './shipment.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ShipmentRoutingModule } from './shipment-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        ShipmentRoutingModule
    ],
    declarations: [ShipmentComponent]
})
export class ShipmentModule { }
