import { PermissionCheckerService } from 'abp-ng2-module';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';

@Injectable()
export class AppNavigationService {

    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) {

    }

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [
            new AppMenuItem('Dashboard', 'Pages.Administration.Host.Dashboard', 'flaticon-line-graph', '/app/admin/hostDashboard'),
            new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'flaticon-line-graph', '/app/main/dashboard'),

//MASTER
            new AppMenuItem('MASTER', '', 'flaticon-user-settings', '',[],[
                new AppMenuItem('COMMON', '', 'flaticon-user-settings', '',[],[
                    new AppMenuItem('Container Status', '', 'flaticon-car', '/app/main/master/common/container-status'),
                    new AppMenuItem('Customs Status', '', 'flaticon-car', '/app/main/master/common/customs-status'),
                    new AppMenuItem('Invoice Status', '', 'flaticon-car', '/app/main/master/common/invoice-status'),
                    new AppMenuItem('Storage Location', '', 'flaticon-car', '/app/main/master/common/storage-location'),
                    new AppMenuItem('Material Group', '', 'flaticon-car', '/app/main/master/common/material-group'),
                    new AppMenuItem('Material Type', '', 'flaticon-car', '/app/main/master/common/material-type'),
                    new AppMenuItem('Product Type', '', 'flaticon-car', '/app/main/master/common/product-type'),
                    new AppMenuItem('Unit Of Measure', '', 'flaticon-car', '/app/main/master/common/uom'),
                    new AppMenuItem('Carfamily', '', 'flaticon-car', '/app/main/master/common/carfamily'),
                    new AppMenuItem('Supplier List', '', 'flaticon-car', '/app/main/master/common/supplier-list'),
                    new AppMenuItem('Forwarder', '', 'flaticon-car', '/app/main/master/common/forwarder'),
                ]),
                new AppMenuItem('Material', '', 'flaticon-car', '/app/main/master/common/material'),
                new AppMenuItem('Part List', '', 'flaticon-car', '/app/main/master/common/part-list')
            ]),
//MAIN
            new AppMenuItem('MANAGEMENT', '', 'flaticon-user-settings', '',[],[
                new AppMenuItem('INTRANSIT', '', 'flaticon-user-settings', '',[],[
                    new AppMenuItem('Shipment', '', 'flaticon-car', '/app/main/management/intransit/shipment'),
                    new AppMenuItem('Bill Of Lading', '', 'flaticon-car', '/app/main/management/intransit/billoflading'),
                    new AppMenuItem('Container Intransit', '', 'flaticon-car', '/app/main/management/intransit/container-intransit'),
                    new AppMenuItem('Container Invoice', '', 'flaticon-car', '/app/main/management/intransit/container-invoice'),
                    new AppMenuItem('Invoice', '', 'flaticon-car', '/app/main/management/intransit/invoice'),
                ]),
                new AppMenuItem('WAREHOUSE', '', 'flaticon-user-settings', '',[],[
                    new AppMenuItem('Container List', '', 'flaticon-car', '/app/main/management/warehouse/container-list'),
                    new AppMenuItem('Container At Warehouse', '', 'flaticon-car', '/app/main/management/warehouse/container-warehouse'),
                    new AppMenuItem('Stock Receiving', '', 'flaticon-car', '/app/main/management/warehouse/stock-receiving'),
                ]),
            ]),
            // new AppMenuItem('Tenants', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),
            // new AppMenuItem('Editions', 'Pages.Editions', 'flaticon-app', '/app/admin/editions'),
            new AppMenuItem('Administration', '', 'flaticon-interface-8', '', [], [
                new AppMenuItem('OrganizationUnits', 'Pages.Administration.OrganizationUnits', 'flaticon-map', '/app/admin/organization-units'),
                new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                new AppMenuItem('Languages', 'Pages.Administration.Languages', 'flaticon-tabs', '/app/admin/languages', ['/app/admin/languages/{name}/texts']),
                new AppMenuItem('AuditLogs', 'Pages.Administration.AuditLogs', 'flaticon-folder-1', '/app/admin/auditLogs'),
                new AppMenuItem('Maintenance', 'Pages.Administration.Host.Maintenance', 'flaticon-lock', '/app/admin/maintenance'),
                new AppMenuItem('Subscription', 'Pages.Administration.Tenant.SubscriptionManagement', 'flaticon-refresh', '/app/admin/subscription-management'),
                //new AppMenuItem('VisualSettings', 'Pages.Administration.UiCustomization', 'flaticon-medical', '/app/admin/ui-customization'),
                new AppMenuItem('Settings', 'Pages.Administration.Host.Settings', 'flaticon-settings', '/app/admin/hostSettings'),
                new AppMenuItem('Settings', 'Pages.Administration.Tenant.Settings', 'flaticon-settings', '/app/admin/tenantSettings'),
                // new AppMenuItem('WebhookSubscriptions', 'Pages.Administration.WebhookSubscription', 'flaticon2-world', '/app/admin/webhook-subscriptions'),
                // new AppMenuItem('DynamicParameters', '', 'flaticon-interface-8', '', [], [
                //     new AppMenuItem('Definitions', 'Pages.Administration.DynamicParameters', '', '/app/admin/dynamic-parameter'),
                //     new AppMenuItem('EntityDynamicParameters', 'Pages.Administration.EntityDynamicParameters', '', '/app/admin/entity-dynamic-parameter'),
                // ])
            ]),
            // new AppMenuItem('DemoUiComponents', 'Pages.DemoUiComponents', 'flaticon-shapes', '/app/admin/demo-ui-components')
        ]);
    }

    checkChildMenuItemPermission(menuItem): boolean {

        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null) {
                if (subMenuItem.route) {
                    return true;
                }
            } else if (this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                let isAnyChildItemActive = this.checkChildMenuItemPermission(subMenuItem);
                if (isAnyChildItemActive) {
                    return true;
                }
            }
        }
        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' && this._appSessionService.tenant && !this._appSessionService.tenant.edition) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): AppMenuItem[] {
        let menu = this.getMenu();
        let allMenuItems: AppMenuItem[] = [];
        menu.items.forEach(menuItem => {
            allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
        });

        return allMenuItems;
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach(subMenu => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }
}
