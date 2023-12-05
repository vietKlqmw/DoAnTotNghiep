import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PartListComponent } from './part-list.component';

const routes: Routes = [{
    path: '',
    component: PartListComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class PartListRoutingModule { }
