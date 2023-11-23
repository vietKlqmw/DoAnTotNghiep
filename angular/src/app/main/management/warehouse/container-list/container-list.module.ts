import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ContainerListRoutingModule } from './container-list-routing.module';
import { ContainerListComponent } from './container-list.component';

@NgModule({
    imports: [
        AppSharedModule,
        ContainerListRoutingModule
    ],
    declarations: [ContainerListComponent]
})
export class ContainerListModule { }
