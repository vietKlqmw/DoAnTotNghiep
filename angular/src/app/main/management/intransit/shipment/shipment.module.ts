import { NgModule } from '@angular/core';
import { ShipmentComponent } from './shipment.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ShipmentRoutingModule } from './shipment-routing.module';
import { EditShipmentModalComponent } from './edit-shipment-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        ShipmentRoutingModule
    ],
    declarations: [
        ShipmentComponent,
        EditShipmentModalComponent
    ]
})
export class ShipmentModule { }
