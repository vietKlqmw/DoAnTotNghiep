import { NgModule } from '@angular/core';
import { MaterialGroupComponent } from './material-group.component';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { MaterialGroupRoutingModule } from './material-group-routing.module';

@NgModule({
    imports: [
        AppSharedModule,
        MaterialGroupRoutingModule
    ],
    declarations: [MaterialGroupComponent]
})
export class MaterialGroupModule { }
