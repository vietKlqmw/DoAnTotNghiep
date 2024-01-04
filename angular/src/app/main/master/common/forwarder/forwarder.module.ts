import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ForwarderRoutingModule } from './forwarder-routing.module';
import { ForwarderComponent } from './forwarder.component';
import { EditForwarderModalComponent } from './edit-forwarder-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        ForwarderRoutingModule
    ],
    declarations: [
        ForwarderComponent,
        EditForwarderModalComponent
    ]
})
export class ForwarderModule { }
