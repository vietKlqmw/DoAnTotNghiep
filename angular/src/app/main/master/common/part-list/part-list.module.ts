import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { PartListRoutingModule } from './part-list-routing.module';
import { PartListComponent } from './part-list.component';

@NgModule({
    imports: [
        AppSharedModule,
        PartListRoutingModule
    ],
    declarations: [
        PartListComponent,
        // EditPartListModalComponent,
        // ImportPartListComponent,
        // ListErrorImportPartListModalComponent
    ]
})
export class PartListModule { }
