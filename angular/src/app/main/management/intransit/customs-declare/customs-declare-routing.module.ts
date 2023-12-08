import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomsDeclareComponent } from './customs-declare.component';

const routes: Routes = [{
    path: '',
    component: CustomsDeclareComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomsDeclareRoutingModule { }
