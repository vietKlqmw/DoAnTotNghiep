import { Component, ViewChild, Injector } from '@angular/core';
import { Table } from 'primeng/table';
import { HostDashboardServiceProxy, GetRecentTenantsOutput, GetDataDashboardNewContToWarehouse, ProdOthersServiceProxy } from '@shared/service-proxies/service-proxies';
import { WidgetComponentBase } from '../widget-component-base';

@Component({
    selector: 'app-widget-recent-tenants',
    templateUrl: './widget-recent-tenants.component.html',
    styleUrls: ['./widget-recent-tenants.component.css']
})
export class WidgetRecentTenantsComponent extends WidgetComponentBase {
    @ViewChild('RecentTenantsTable', { static: true }) recentTenantsTable: Table;
    constructor(injector: Injector,
        private _hostDashboardServiceProxy: HostDashboardServiceProxy,
        private _other: ProdOthersServiceProxy) {
        super(injector);
        this.loadRecentTenantsData();
    }

    loading = true;

    //recentTenantsData: GetRecentTenantsOutput;
    recentTenantsData = [];
    selectedWarehouse = 'A1';

    loadRecentTenantsData() {
        // this._hostDashboardServiceProxy.getRecentTenantsData().subscribe((data) => {
        //     this.recentTenantsData = data;
        //     this.loading = false;
        // });
        this._other.getDataForDashboardNewContToWarehouse(this.selectedWarehouse).subscribe((data) => {
            this.recentTenantsData = data;
            this.loading = false;
        });
    }

    warehouseChange(interval) {
        if (this.selectedWarehouse === interval) {
            return;
        }

        this.selectedWarehouse = interval;
        this.loadRecentTenantsData();
    }

    gotoAllRecentTenants(): void {
        window.open(abp.appPath + 'app/main/management/warehouse/container-warehouse');
    }

}
