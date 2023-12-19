import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { Table } from 'primeng/table';
import { HostDashboardServiceProxy, GetExpiringTenantsOutput, ProdInvoiceStockOutDto, ProdOthersServiceProxy } from '@shared/service-proxies/service-proxies';
import { WidgetComponentBase } from '../widget-component-base';

@Component({
    selector: 'app-widget-subscription-expiring-tenants',
    templateUrl: './widget-subscription-expiring-tenants.component.html',
    styleUrls: ['./widget-subscription-expiring-tenants.component.css']
})
export class WidgetSubscriptionExpiringTenantsComponent extends WidgetComponentBase implements OnInit {

    @ViewChild('ExpiringTenantsTable', { static: true }) expiringTenantsTable: Table;

    dataLoading = true;
    //expiringTenantsData: GetExpiringTenantsOutput;
    expiringTenantsData = [];
    selectedWarehouse = 'A1';

    constructor(injector: Injector,
        private _hostDashboardServiceProxy: HostDashboardServiceProxy,
        private _other: ProdOthersServiceProxy) {
        super(injector);
    }

    ngOnInit() {
        this.getData();
    }

    getData() {
        // this._hostDashboardServiceProxy.getSubscriptionExpiringTenantsData().subscribe((data) => {
        //     this.expiringTenantsData = data;
        //     this.dataLoading = false;
        // });
        this._other.getDataForDashboardStockOut(this.selectedWarehouse).subscribe((data) => {
            this.expiringTenantsData = data;
            this.dataLoading = false;
        });
    }

    warehouseChange(interval) {
        if (this.selectedWarehouse === interval) {
            return;
        }

        this.selectedWarehouse = interval;
        this.getData();
    }

    gotoAllExpiringTenants(): void {
        const url = abp.appPath + 'app/main/management/warehouse/invoice-stock-out';

        window.open(url);
    }
}
