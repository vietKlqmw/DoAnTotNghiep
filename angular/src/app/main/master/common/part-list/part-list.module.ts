import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { PartListRoutingModule } from './part-list-routing.module';
import { PartListComponent } from './part-list.component';
import { ViewMaterialModule } from '../../other/view-material/view-material.module';
import { EditPartListModalComponent } from './edit-part-list-modal.component';
import { ImportPartListComponent } from './import-part-list-modal.component';

@NgModule({
    imports: [
        AppSharedModule,
        PartListRoutingModule,
        ViewMaterialModule
    ],
    declarations: [
        PartListComponent,
        EditPartListModalComponent,
        ImportPartListComponent,
        // ListErrorImportPartListModalComponent
    ]
})
export class PartListModule { }
