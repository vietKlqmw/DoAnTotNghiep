import { NgModule } from '@angular/core';
import { FuelTypeComponent } from './fuel-type.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { FuelTypeRoutingModule } from './fuel-type-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        FuelTypeRoutingModule
    ],
    declarations: [FuelTypeComponent]
})
export class FuelTypeModule { }
