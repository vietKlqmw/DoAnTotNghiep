import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { ContainerIntransitRoutingModule } from './container-intransit-routing.module';
import { ContainerIntransitComponent } from './container-intransit.component';
import { EditContainerIntransitModalComponent } from './edit-container-intransit-modal.component';
import { ListErrorImportModalComponent } from './list-error-import-container-intransit-modal.component';
import { ImportContainerIntransitComponent } from './import-container-intransit-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        ContainerIntransitRoutingModule
    ],
    declarations: [
        ContainerIntransitComponent,
        EditContainerIntransitModalComponent,
        ListErrorImportModalComponent,
        ImportContainerIntransitComponent
    ]
})
export class ContainerIntransitModule { }
