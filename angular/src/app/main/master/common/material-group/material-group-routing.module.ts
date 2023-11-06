import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MaterialGroupComponent } from './material-group.component';

const routes: Routes = [{
    path: '',
    component: MaterialGroupComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MaterialGroupRoutingModule { }
