import { NgModule } from '@angular/core';
import { CarfamilyComponent } from './carfamily.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CarfamilyRoutingModule } from './carfamily-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        CarfamilyRoutingModule
    ],
    declarations: [CarfamilyComponent]
})
export class CarfamilyModule { }
