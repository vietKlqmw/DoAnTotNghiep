import { NgModule } from '@angular/core';
import { UomComponent } from './uom.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { UomRoutingModule } from './uom-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        UomRoutingModule
    ],
    declarations: [UomComponent]
})
export class UomModule { }
