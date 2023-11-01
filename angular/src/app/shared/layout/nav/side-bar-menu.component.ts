import { PermissionCheckerService } from 'abp-ng2-module';
import { Injector, ElementRef, Component, OnInit, ViewEncapsulation, Inject, Renderer2, AfterViewInit, ViewChild, Input } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AppMenu } from './app-menu';
import { AppNavigationService } from './app-navigation.service';
import { DOCUMENT } from '@angular/common';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';
import { MenuOptions } from '@metronic/app/core/_base/layout/directives/menu.directive';
import { FormattedStringValueExtracter } from '@shared/helpers/FormattedStringValueExtracter';
import { BusyIfDirective } from '@shared/utils/busy-if.directive';
import { AppMenuItem } from './app-menu-item';
import { TABS } from '@app/shared/constants/tab-keys';
import { EventBusService } from '@app/shared/services/event-bus.service';
import * as objectPath from 'object-path';

@Component({
    templateUrl: './side-bar-menu.component.html',
    selector: 'side-bar-menu',
    encapsulation: ViewEncapsulation.None
})
export class SideBarMenuComponent extends AppComponentBase implements OnInit, AfterViewInit {
    @Input() menuClass = 'menu menu-column menu-rounded menu-sub-indention px-3';
    @Input() iconMenu = false;

    @ViewChild('sidebar') sidebar: BusyIfDirective;
    menu: AppMenu = null;

    currentRouteUrl = '';
    insideTm: any;
    outsideTm: any;
    isLoading: boolean = false;

    menuOptions: MenuOptions = {
        // vertical scroll
        scroll: null,

        // submenu setup
        submenu: {
            desktop: {
                default: 'dropdown',
                state: {
                    body: 'kt-aside--minimize',
                    mode: 'dropdown'
                }
            },
            tablet: 'accordion', // menu set to accordion in tablet mode
            mobile: 'accordion' // menu set to accordion in mobile mode
        },

        // accordion setup
        accordion: {
            expandAll: false // allow having multiple expanded accordions in the menu
        }
    };

    constructor(
        injector: Injector,
        private el: ElementRef,
        private router: Router,
        public permission: PermissionCheckerService,
        private _appNavigationService: AppNavigationService,
        @Inject(DOCUMENT) private document: Document,
        private eventBus: EventBusService,
        private render: Renderer2) {
        super(injector);
    }

    ngOnInit() {
        this.menu = this._appNavigationService.getMenu();

        this.currentRouteUrl = this.router.url.split(/[?#]/)[0];

        this.router.events
            .pipe(filter(event => event instanceof NavigationEnd))
            .subscribe(event => this.currentRouteUrl = this.router.url.split(/[?#]/)[0]);
    }

    ngAfterViewInit(): void {
        this.scrollToCurrentMenuElement();
    }

    showMenuItem(menuItem): boolean {
        return this._appNavigationService.showMenuItem(menuItem);
    }

    isMenuItemIsActive(item): boolean {
        if (item.items.length) {
            return this.isMenuRootItemIsActive(item);
        }

        if (!item.route) {
            return false;
        }

        let urlTree = this.router.parseUrl(this.currentRouteUrl.replace(/\/$/, ''));
        let urlString = '/' + urlTree.root.children.primary.segments.map(segment => segment.path).join('/');
        let exactMatch = urlString === item.route.replace(/\/$/, '');
        if (!exactMatch && item.routeTemplates) {
            for (let i = 0; i < item.routeTemplates.length; i++) {
                let result = new FormattedStringValueExtracter().Extract(urlString, item.routeTemplates[i]);
                if (result.IsMatch) {
                   return true;
                }
            }
        }
        return exactMatch;
    }

    isMenuRootItemIsActive(item): boolean {
        let result = false;

        for (const subItem of item.items) {
            result = this.isMenuItemIsActive(subItem);
            if (result) {
                return true;
            }
        }

        return false;
    }

    /**
	 * Use for fixed left aside menu, to show menu on mouseenter event.
	 * @param e Event
	 */
    mouseEnter(e: Event) {
        if (!this.currentTheme.baseSettings.menu.allowAsideMinimizing) {
            return;
        }

        // check if the left aside menu is fixed
        if (document.body.classList.contains('kt-aside--fixed')) {
            if (this.outsideTm) {
                clearTimeout(this.outsideTm);
                this.outsideTm = null;
            }

            this.insideTm = setTimeout(() => {
                // if the left aside menu is minimized
                if (document.body.classList.contains('kt-aside--minimize') && KTUtil.isInResponsiveRange('desktop')) {
                    // show the left aside menu
                    this.render.removeClass(document.body, 'kt-aside--minimize');
                    this.render.addClass(document.body, 'kt-aside--minimize-hover');
                }
            }, 50);
        }
    }

    /**
     * Use for fixed left aside menu, to show menu on mouseenter event.
     * @param e Event
     */
    mouseLeave(e: Event) {
        if (!this.currentTheme.baseSettings.menu.allowAsideMinimizing) {
            return;
        }

        if (document.body.classList.contains('kt-aside--fixed')) {
            if (this.insideTm) {
                clearTimeout(this.insideTm);
                this.insideTm = null;
            }

            this.outsideTm = setTimeout(() => {
                // if the left aside menu is expand
                if (document.body.classList.contains('kt-aside--minimize-hover') && KTUtil.isInResponsiveRange('desktop')) {
                    // hide back the left aside menu
                    this.render.removeClass(document.body, 'kt-aside--minimize-hover');
                    this.render.addClass(document.body, 'kt-aside--minimize');
                }
            }, 100);
        }
    }

    scrollToCurrentMenuElement(): void {
        const path = location.pathname;
        const menuItem = document.querySelector('a[href=\'' + path + '\']');
        if (menuItem) {
            menuItem.scrollIntoView({ block: 'center' });
        }
    }

    getItemCssClasses(item: AppMenuItem, parentItem: AppMenuItem) {
        let classes = 'menu-item';

        if (item.items.length) {
            if (!this.iconMenu) { classes += ' menu-accordion'; }
            else {
                if (parentItem == null) { classes += ' menu-dropdown'; }
                else { classes += ' menu-accordion'; }
            }
        }
        // custom class for menu item
        const customClass = objectPath.get(item, 'custom-class');
        if (customClass) { classes += ' ' + customClass; }
        if (this.iconMenu && parentItem == null) { classes += ' pb-3'; }
        if (!this.iconMenu && this.isMenuItemIsActive(item)) { classes += ' show'; }

        return classes;
    }

    /**
     * Open the selected component. Display in placeholder of dynamic tab,
     * or open a dedicated tab, or open a specific tab.
     *
     * @param event
     * @param item
     */
    openComponent(event, item: AppMenuItem) {
        // if (item.route.startsWith('SERVICE_REPORT') || item.name.startsWith('SALE_REPORT') || item.route.startsWith('CRM_REPORT') || item.route.startsWith('CRAM_IMPORT') || item.route.startsWith('NPS_EXPORT')) {
        //     if (item.parameters && item.parameters['viewChildName']) {
        //         this.parameters = item.parameters;
        //         setTimeout(() => {
        //             this[item.parameters['viewChildName']].show();
        //         });
        //     }
        //     else this[item.name.charAt(0).toLowerCase() + item.name.slice(1)].show();
        //     return;
        // }
        // const functionCode = (item.parameters && item.parameters.functionCode) || item.route; // origin
        const functionCode = item.route;
        if (!functionCode) { return; }

        // Only emit event for TABS
        if (Object.values(TABS).indexOf(functionCode) < 0) { return; }
        event.stopPropagation();
        // TODO: Review Code. Replace this.eventBus.
        // TODO:
        // - 1st Show Modal Filter Tabs
        // - 2nd Show a new browser tab
        // - 3rd emit event 'openComponent' to immediatly show tab

        // if (functionCode === TABS.SERVICE_QUOTATION_AGGREGATE_APPOINTMENT) {
        //     window.open('/app/main/aggregate-appointment', '_blank');
        // } else if (functionCode === TABS.SERVICES_REPAIR_PROGRESS_GENERAL_REPAIR) {
        //     localStorage.removeItem(StorageKeys.Open_General_Repair);
        //     window.open('/screens/gj', StorageKeys.General_Repair_Opened).focus();
        switch(functionCode) {
            // case TABS.ASSY_SPS_ASSEMBLYDATASCREENA1:
            //     window.open('/screens/assemblydatascreen?screen_code=A1', 'ASSY_SPS_ASSEMBLYDATASCREENA1').focus();
            //     break;
            // case TABS.ASSY_SPS_ASSEMBLYDATASCREENA2:
            //     window.open('/screens/assemblydatascreen?screen_code=A2', 'ASSY_SPS_ASSEMBLYDATASCREENA2').focus();
            //     break;
            /*LogA */
            //Lds
            // case TABS.LOGA_LDS_LOTDIRECTSUPPLYANDONA1:
            //     window.open("/screens/lotdirectsupplyandon?pline=A1", 'LOGA_LDS_LOTDIRECTSUPPLYANDONA1').focus();
            //     break;
            // case TABS.LOGA_LDS_LOTDIRECTSUPPLYANDONA2:
            //     window.open("/screens/lotdirectsupplyandon?pline=A2", 'LOGA_LDS_LOTDIRECTSUPPLYANDONA2').focus();
            //     break;
            // case TABS.LOGA_LDS_LOTDIRECTSUPPLYJIRIRIKOKUA1:
            //     window.open("/screens/jiririkokuscreen?pline=A1", 'LOGA_LDS_LOTDIRECTSUPPLYJIRIRIKOKUA1').focus();
            //     break;
            // case TABS.LOGA_LDS_LOTDIRECTSUPPLYJIRIRIKOKUA2:
            //     window.open("/screens/jiririkokuscreen?pline=A2", 'LOGA_LDS_LOTDIRECTSUPPLYJIRIRIKOKUA2').focus();
            //     break;

            //BP2
            // case TABS.LOGA_BP2_BIGPARTPXPUP:
            //     window.open("/screens/bigpartpxpup", 'LOGA_BP2_BIGPARTPXPUP').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTJIRIRIKOKUA1NEW:
            //     window.open("/screens/LgaBp2ProgressMonitorScreen?pline=A1", 'LOGA_BP2_BIGPARTJIRIRIKOKUA1NEW').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTJIRIRIKOKUA2NEW:
            //     window.open("/screens/LgaBp2ProgressMonitorScreen?pline=A2", 'LOGA_BP2_BIGPARTJIRIRIKOKUA2NEW').focus();
            //     break;

            // case TABS.LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA1NEW:
            //     window.open("/screens/LgaBp2ProgressScreen?pline=A2", 'LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA1NEW').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA2NEW:
            //     window.open("/screens/LgaBp2ProgressScreen?pline=A2", 'LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA2NEW').focus();
            //     break;

            // case TABS.LOGA_BP2_BIGPARTJIRIRIKOKUA1:
            //     window.open("/screens/bigpartjiririkokuscreen?pline=A2", 'LOGA_BP2_BIGPARTJIRIRIKOKUA1').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTJIRIRIKOKUA2:
            //     window.open("/screens/bigpartjiririkokuscreen?pline=A2", 'LOGA_BP2_BIGPARTJIRIRIKOKUA2').focus();
            //     break;

            // case TABS.LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA1:
            //     window.open("/screens/bigpartdirectdeliveryprogressandon?pline=A1", 'LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA1').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA2:
            //     window.open("/screens/bigpartdirectdeliveryprogressandon?pline=A2", 'LOGA_BP2_BIGPARTDIRECTDELIVERYPROGRESSA2').focus();
            //     break;

            // case TABS.LOGA_BP2_BIGPARTTABLETA1:
            //     window.open("/screens/bigparttablet2?prod_line=A1&ecar_id=1", 'LOGA_BP2_BIGPARTTABLETA1').focus();
            //     break;
            // case TABS.LOGA_BP2_BIGPARTTABLETA2:
            //     window.open("/screens/bigparttablet2?prod_line=A2&ecar_id=3", 'LOGA_BP2_BIGPARTTABLETA2').focus();
            //     break;

            //EKB
            // case TABS.LOGA_EKB_PROGRESSSCREEN:
            //     window.open("/screens/progressscreen", 'LOGA_EKB_PROGRESSSCREEN').focus();
            //     break;
            // case TABS.LOGA_EKB_EKANBAN:
            //     window.open("/app/main/loga/ekb/ekanban", 'LOGA_EKB_EKANBAN').focus();
            //     break;
            // case TABS.LOGA_EKB_EKANBANPROGRESSSCREENA1:
            //     window.open("/screens/ekanbanprogressscreen?pline=A2", 'LOGA_EKB_EKANBANPROGRESSSCREENA1').focus();
            //     break;
            // case TABS.LOGA_EKB_EKANBANPROGRESSSCREENA2:
            //     window.open("/screens/ekanbanprogressscreen?pline=A2", 'LOGA_EKB_EKANBANPROGRESSSCREENA2').focus();
            //     break;

            // LogW
            // case TABS.LGW_MWH_LOGW_ADO_CALLINGLIGHTW1:
            //     window.open("/screens/callinglight?prod_line=W1", 'LGW_MWH_LOGW_ADO_CALLINGLIGHTW1').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_ADO_CALLINGLIGHTW2:
            //     window.open("/screens/callinglight?prod_line=W2", 'LGW_MWH_LOGW_ADO_CALLINGLIGHTW2').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_ADO_CALLINGLIGHTW3:
            //     window.open("/screens/callinglight?prod_line=W3", 'LGW_MWH_LOGW_ADO_CALLINGLIGHTW3').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_ADO_CALLINGLIGHTIP:
            //     window.open("/screens/callinglight?prod_line=IP", 'LGW_MWH_LOGW_ADO_CALLINGLIGHTIP').focus();
            //     break;
            // // Mwh
            // case TABS.LGW_MWH_LOGW_MWH_MWHSMALLPART:
            //     window.open("/screens/pxpsmallpartinput", 'LGW_MWH_LOGW_MWH_MWHSMALLPART').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_MWH_MWHBIGPART:
            //     window.open("/screens/pxpbigpartinput", 'LGW_MWH_LOGW_MWH_MWHBIGPART').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_MWH_STOCKATWH:
            //     window.open("/screens/stockatwh", 'LGW_MWH_LOGW_MWH_STOCKATWH').focus();
            //     break;

            //Lup
            // case TABS.LGW_MWH_LOGW_PUP_LOTUNPACKINGW1:
            //     window.open("/screens/lotupackingandon?Line=W1", 'LGW_MWH_LOGW_PUP_LOTUNPACKINGW1').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PUP_LOTUNPACKINGW2:
            //     window.open("/screens/lotupackingandon?Line=W2", 'LGW_MWH_LOGW_PUP_LOTUNPACKINGW2').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PUP_LOTUNPACKINGW3:
            //     window.open("/screens/lotupackingandon?Line=W3", 'LGW_MWH_LOGW_PUP_LOTUNPACKINGW3').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_LUP_CONTMODULE:
            //     window.open("/app/main/logw/lup/contmodule", 'LGW_MWH_LOGW_LUP_CONTMODULE').focus();
            //     break;

            //Dvn
            // case TABS.LGW_MWH_LOGW_DVN_DEVANNINGSCREEN:
            //     window.open("/screens/devanningscreen", 'LGW_MWH_LOGW_DVN_DEVANNINGSCREEN').focus();
            //     break;

            //Pup
            // case TABS.LGW_MWH_LOGW_PUP_MODULEUNPACKINGANDON:
            //     window.open("/screens/moduleunpackingandon", 'LGW_MWH_LOGW_PUP_MODULEUNPACKINGANDON').focus();
            //     break;
            //Pik
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETUBW1:
            //     window.open("/screens/pickingtabletandon?PickingPosition=UB_W1&TabletId=PIK_LW_UB_W1_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETUBW1').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETSMW1:
            //     window.open("/screens/pickingtabletandon?PickingPosition=SM_W1&TabletId=PIK_LW_SM_W1_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETSMW1').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETUBW2:
            //     window.open("/screens/pickingtabletandon?PickingPosition=UB_W2&TabletId=PIK_LW_UB_W2_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETUBW2').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETSMW2:
            //     window.open("/screens/pickingtabletandon?PickingPosition=SM_W2&TabletId=PIK_LW_SM_W2_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETSMW2').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETUBW3:
            //     window.open("/screens/pickingtabletandon?PickingPosition=UB_W3&TabletId=PIK_LW_UB_W3_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETUBW3').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGTABLETSMW3:
            //     window.open("/screens/pickingtabletandon?PickingPosition=SM_W3&TabletId=PIK_LW_SM_W3_01", 'LGW_MWH_LOGW_PIK_PICKINGTABLETSMW3').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGMONITORW1:
            //     window.open("/screens/pickingmonitoringscreen?pline=W1", 'LGW_MWH_LOGW_PIK_PICKINGMONITORW1').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGMONITORW2:
            //     window.open("/screens/pickingmonitoringscreen?pline=W2", 'LGW_MWH_LOGW_PIK_PICKINGMONITORW2').focus();
            //     break;
            // case TABS.LGW_MWH_LOGW_PIK_PICKINGMONITORW3:
            //     window.open("/screens/pickingmonitoringscreen?pline=W3", 'LGW_MWH_LOGW_PIK_PICKINGMONITORW3').focus();
            //     break;

            /*Painting Andon*/
            // case TABS.PTA_ADO_CCRMONITOR:
            //     window.open("/screens/ccrmonitor", 'PTA_ADO_CCRMONITOR').focus();
            //     break;
            // case TABS.PTA_ADO_LINEREALTIMECONTROL:
            //     window.open("/screens/linerealtimecontrols", 'PTA_ADO_LINEREALTIMECONTROL').focus();
            //     break;
            // case TABS.PTA_ADO_DELAYCONTROLSCREEN:
            //     window.open("/screens/delaycontrolscreen",'PTA_ADO_DELAYCONTROLSCREEN').focus();
            //     break;
            // case TABS.PTA_ADO_BUMPERCLRINDICATOR:
            //     window.open("/screens/bumpergetdataclrindicator",'PTA_ADO_BUMPERCLRINDICATOR' ).focus();
            //     break;
            // case TABS.PTA_ADO_NEXTEDIN:
            //     window.open("/screens/nextedin" ,'PTA_ADO_NEXTEDIN' ).focus();
            //     break;
            // case TABS.PTA_ADO_BUMPERGETDATASMALLSUBASSY:
            //     window.open("/screens/bumpergetdatasmallsubassy", 'PTA_ADO_BUMPERGETDATASMALLSUBASSY' ).focus();
            //     break;
            // case TABS.PTA_ADO_BUMPERIN:
            //     window.open("/screens/bumperin" , 'PTA_ADO_BUMPERIN' ).focus();
            //     break;
            // case TABS.PTA_ADO_BUMPERSUBASSYA1:
            //     window.open("/screens/bumpersubassy?pline=A1", 'PTA_ADO_BUMPERSUBASSYA1' ).focus();
            //     break;
            // case TABS.PTA_ADO_BUMPERSUBASSYA2:
            //     window.open("/screens/bumpersubassy?pline=A2", 'PTA_ADO_BUMPERSUBASSYA2' ).focus();
            //     break;

            //Welding Andon
            // case TABS.WEL_ADO_PROCESSINSTRUCTION:
            //     window.open("/screens/processinstruction?Line=W1&Screen=BODY_EC" ,'WEL_ADO_PROCESSINSTRUCTION').focus();
            //     break;
            // case TABS.WEL_ADO_PUNCHQUEUEINDICATOR:
            //     window.open("/app/main/welding/andon/punchqueueindicator",'WEL_ADO_PUNCHQUEUEINDICATOR' ).focus();
            //     break;


            default:
                this.eventBus.emit({
                    type: 'openComponent',
                    functionCode: functionCode,
                    tabHeader: this.l(item.name),
                    params: item.parameters
                });
                break;
        }

    }
}
