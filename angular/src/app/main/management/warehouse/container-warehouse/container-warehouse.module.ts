import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ContainerWarehouseRoutingModule } from './container-warehouse-routing.module';
import { ContainerWarehouseComponent } from './container-warehouse.component';

@NgModule({
    imports: [
        AppSharedModule,
        ContainerWarehouseRoutingModule
    ],
    declarations: [ContainerWarehouseComponent]
})
export class ContainerWarehouseModule { }
