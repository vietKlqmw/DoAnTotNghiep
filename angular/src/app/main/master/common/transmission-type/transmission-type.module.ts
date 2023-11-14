import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { TransmissionTypeRoutingModule } from './transmission-type-routing.module';
import { TransmissionTypeComponent } from './transmission-type.component';

@NgModule({
    imports: [
        AppSharedModule,
        TransmissionTypeRoutingModule
    ],
    declarations: [TransmissionTypeComponent]
})
export class TransmissionTypeModule { }
