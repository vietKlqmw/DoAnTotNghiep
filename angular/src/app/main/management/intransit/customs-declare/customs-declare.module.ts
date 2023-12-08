import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CustomsDeclareRoutingModule } from './customs-declare-routing.module';
import { EditCustomsDeclareModalComponent } from './edit-customs-declare-modal.component';
import { CustomsDeclareComponent } from './customs-declare.component';

@NgModule({
    imports: [
        AppSharedModule,
        CustomsDeclareRoutingModule
    ],
    declarations: [
        CustomsDeclareComponent,
        EditCustomsDeclareModalComponent
    ]
})
export class CustomsDeclareModule { }
