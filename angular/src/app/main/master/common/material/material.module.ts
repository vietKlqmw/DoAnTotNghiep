import { NgModule } from '@angular/core';
import { MaterialComponent } from './material.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { MaterialRoutingModule } from './material-routing.module';
import { ViewMaterialModalComponent } from './view-material-modal.component';
import { EditMaterialModalComponent } from './edit-material-modal.component';
import { ImportMaterialComponent } from './import-material-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        MaterialRoutingModule
    ],
    declarations: [
        MaterialComponent,
        ViewMaterialModalComponent,
        EditMaterialModalComponent,
        ImportMaterialComponent
    ]
})
export class MaterialModule { }
