import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContainerIntransitComponent } from './container-intransit.component';
const routes: Routes = [{
    path: '',
    component: ContainerIntransitComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContainerIntransitRoutingModule { }
