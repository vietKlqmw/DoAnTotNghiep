import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EngineModelComponent } from './engine-model.component';

const routes: Routes = [{
    path: '',
    component: EngineModelComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EngineModelRoutingModule { }
