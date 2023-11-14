import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { EngineModelRoutingModule } from './engine-model-routing.module';
import { EngineModelComponent } from './engine-model.component';

@NgModule({
    imports: [
        AppSharedModule,
        EngineModelRoutingModule
    ],
    declarations: [EngineModelComponent]
})
export class EngineModelModule { }
