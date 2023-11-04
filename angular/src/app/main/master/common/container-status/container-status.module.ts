import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ContainerStatusRoutingModule } from './container-status-routing.module';
import { ContainerStatusComponent } from './container-status.component';

@NgModule({
    imports: [
        AppSharedModule,
        ContainerStatusRoutingModule
    ],
    declarations: [ContainerStatusComponent]
})
export class ContainerStatusModule { }
