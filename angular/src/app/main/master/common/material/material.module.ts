import { NgModule } from '@angular/core';
import { MaterialComponent } from './material.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { MaterialRoutingModule } from './material-routing.module';
import { ViewMaterialModalComponent } from './view-material-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        MaterialRoutingModule
    ],
    declarations: [
        MaterialComponent,
        ViewMaterialModalComponent
    ]
})
export class MaterialModule { }
