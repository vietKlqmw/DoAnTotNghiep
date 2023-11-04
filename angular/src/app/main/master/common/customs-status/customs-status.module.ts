import { NgModule } from '@angular/core';
import { CustomsStatusComponent } from './customs-status.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CustomsStatusRoutingModule } from './customs-status-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        CustomsStatusRoutingModule
    ],
    declarations: [CustomsStatusComponent]
})
export class CustomsStatusModule { }
