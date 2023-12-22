import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderPartComponent } from './order-part.component';

const routes: Routes = [{
    path: '',
    component: OrderPartComponent,
    pathMatch: 'full'
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class OrderPartRoutingModule { }
