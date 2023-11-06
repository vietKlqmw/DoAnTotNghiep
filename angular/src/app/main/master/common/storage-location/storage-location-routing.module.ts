import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StorageLocationComponent } from './storage-location.component';

const routes: Routes = [{
    path: '',
    component: StorageLocationComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StorageLocationRoutingModule { }
