import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ContainerIntransitRoutingModule } from './container-intransit-routing.module';
import { ContainerIntransitComponent } from './container-intransit.component';

@NgModule({
    imports: [
        AppSharedModule,
        ContainerIntransitRoutingModule
    ],
    declarations: [ContainerIntransitComponent]
})
export class ContainerIntransitModule { }
