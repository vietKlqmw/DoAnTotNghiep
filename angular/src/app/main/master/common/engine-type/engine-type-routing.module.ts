import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EngineTypeComponent } from './engine-type.component';

const routes: Routes = [{
    path: '',
    component: EngineTypeComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class EngineTypeRoutingModule { }
