import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ForwarderComponent } from './forwarder.component';

const routes: Routes = [{
    path: '',
    component: ForwarderComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ForwarderRoutingModule { }
