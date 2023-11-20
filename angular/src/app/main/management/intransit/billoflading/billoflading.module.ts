import { NgModule } from '@angular/core';
import { BillofladingComponent } from './billoflading.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { BillofladingRoutingModule } from './billoflading-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        BillofladingRoutingModule
    ],
    declarations: [BillofladingComponent]
})
export class BillofladingModule { }
