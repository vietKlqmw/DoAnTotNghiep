import { NgModule } from '@angular/core';
import { BillofladingComponent } from './billoflading.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { BillofladingRoutingModule } from './billoflading-routing.module';
import { EditBillOfLadingModalComponent } from './edit-billoflading-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        BillofladingRoutingModule
    ],
    declarations: [
        BillofladingComponent,
        EditBillOfLadingModalComponent
    ]
})
export class BillofladingModule { }
