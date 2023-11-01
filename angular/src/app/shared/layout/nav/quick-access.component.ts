import { Component, Injector, OnInit, Input, HostListener, ViewChild, ElementRef, DoCheck } from '@angular/core';
import { ThemesLayoutBaseComponent } from '../themes/themes-layout-base.component';
import { ChangeUserLanguageDto, ProfileServiceProxy } from '@shared/service-proxies/service-proxies';
import { Router } from '@angular/router';
import { AppNavigationService } from '../nav/app-navigation.service';
import { EventBusService } from '@app/shared/services/event-bus.service';
import { TABS } from '@app/shared/constants/tab-keys';

@Component({
    selector: 'quick-access',
    templateUrl: './quick-access.component.html',
    styleUrls: ['./quick-access.component.less'],
})
export class QuickAccessComponent extends ThemesLayoutBaseComponent implements DoCheck {
    @Input() isDropup = false;
    @Input() customStyle = 'btn btn-icon btn-custom btn-icon-muted btn-active-light btn-active-color-primary w-35px h-35px w-md-40px h-md-40px position-relative';
    @ViewChild('listItems', { static: false }) selectedListItem: ElementRef;
    @ViewChild('searchInput') searchInput: ElementRef;

    default = true;
    settingLookUp: any[] = [];
    filterSearchResult;
    showList = false;
    selectedIndex = -1;
    selectedValueIndexInput;
    tempFilterSearchResult: any[] = [];
    menuVisible = false;
    indexItemShow;
    listItem = [];
    resultMenu;

    public constructor(
        injector: Injector,
       // _dateTimeService: DateTimeService,
        private menu: AppNavigationService,
        private eventBus: EventBusService,
    ) {
        super(injector);
    }

    // Lắng nghe xem, nếu quick-access hiển thị thì focus vào input
    ngDoCheck() {
        const menuElement = document.getElementById('quick-access');
        if (menuElement) {
          const currentDisplay = window.getComputedStyle(menuElement).getPropertyValue('display');
          if (currentDisplay == "flex") {
            this.searchInput.nativeElement.focus();
          }
        }
      }

    ngOnInit() {
         this.getMenu();
    }

    @HostListener('document:keydown', ['$event'])
    handleKeyboardEvent(event: KeyboardEvent) {
        // if (event.ctrlKey && event.key === 'F1') {
        //     // this.getMenu();
        //     // this.accessControlAction();
        //     console.log('open quick search');
        // }
    }

    // Sự kiện Selected Item Result ListSearch
    handleKeyDown(event: any) {
        const key = event.key;
        var scrollItem = document.querySelector('.input-group ul.rounded.list-unstyled .quick-access-result-item.selected');

        if (this.showList && this.filterSearchResult.length > 0) {
            this.tempFilterSearchResult = [];
            for (let i = 0; i < this.filterSearchResult.length; i++) {
                this.tempFilterSearchResult.push({
                    item: this.filterSearchResult[i],
                    index: i,
                    isSelected: i === this.selectedIndex
                });
            }

            if (key === 'ArrowDown') {
                this.selectedIndex = (this.selectedIndex + 1) % this.filterSearchResult.length;
                // this.selectedValueIndexInput = this.tempFilterSearchResult[this.selectedIndex].item.name

                //Check scroll itemindex
                if (this.selectedIndex >= 9) {
                    scrollItem.scrollIntoView(true);
                }


            } else if (key === 'ArrowUp') {
                this.selectedIndex = (this.selectedIndex - 1 + this.filterSearchResult.length) % this.filterSearchResult.length;
                //this.selectedValueIndexInput = this.tempFilterSearchResult[this.selectedIndex].item.name

                //Check scroll itemindex
                scrollItem.scrollIntoView(false);

            }
            if (key === 'Enter') {
               // window.location.href = this.tempFilterSearchResult[this.selectedIndex].item.route
                this.checkTabKey();
            }
        }
        if (key === 'Escape') {
            this.closeQuickAccess();
        }
    }

    checkTabKey(url?,name?){
        if(url){
            this.openTab(url,name,{})
            this.closeQuickAccess();
        }
        else{
            var filterSearch = this.tempFilterSearchResult[this.selectedIndex].item.route
            this.openTab(filterSearch,this.tempFilterSearchResult[this.selectedIndex].item.name,{})
            this.closeQuickAccess();
        }
    }


    //Sự kiện khi ấn ra ngoài phạm vi --> tắt Quick-Access
    @HostListener('document:click', ['$event'])
    handleDocumentClick(event: MouseEvent) {
        const quickFocus = document.querySelector('.menu .access-form') as HTMLElement
        if (!quickFocus?.contains(event.target as HTMLElement)) {
            this.closeQuickAccess()
        }
    }

    //Xử lý ẩn hiện Quick-Access
    accessControlAction() {
        const menuElement = document.getElementById('quick-access');
        if (menuElement) {
            if (menuElement.style.display == 'none') {
                menuElement.style.display = 'flex'
                this.menuVisible = true;
            }
            else {
                menuElement.style.display = 'none'
                this.menuVisible = false;
            }
        }
        if (this.menuVisible) {
            const inputElement = menuElement.querySelector('input');
            if (inputElement) {
                inputElement.focus();
            }
        }

    }

    quickSearch(querry: KeyboardEvent) {
        if (querry) {
            const element = querry.target as HTMLInputElement
            this.accessFilterSearch(element.value)
            if (element.value.toString() != '') {
                this.showList = true
                this.default = false
            }
            else {
                this.showList = false
                this.default = true
            }
        }
    }

    accessFilterSearch(value) {
        this.filterSearchResult = this.fillterByValues(this.listItem, value, 'name', 'route')
    }

    getMenu() {
        let allItem = this.menu.getMenu();
        this.resultMenu = (allItem.items.length > 0) ? allItem.items : undefined
        this.listItem = [];
        if (this.resultMenu) this.getMenuItem(this.resultMenu, 1, "");
    }

    getMenuItem(_resultMenu, level, level_txt) {

        for (let i = 0; i < _resultMenu.length; i++) {

            this.indexItemShow = level_txt + (i + 1) + ".";

            // không có phân cấp con
            if (!_resultMenu[i].items.length && !_resultMenu[i].external) {
                _resultMenu[i].icon = this.indexItemShow.substring(0, this.indexItemShow.length - 1);
                this.listItem.push(_resultMenu[i]);
            }
            // có phân cấp con
            else if (_resultMenu[i].items.length && !_resultMenu[i].external) {
                this.getMenuItem(_resultMenu[i].items, level + 1, this.indexItemShow);
            }

        }
    }
    openTab(tabCode: string, tabHeader: string, params: any) {
        this.eventBus.emit({
            type: 'openComponent',
            functionCode: tabCode,
            tabHeader: tabHeader,
            params: params,
        });
    }

    showMenuItem(menuItem): boolean {
        return this.menu.showMenuItem(menuItem);
    }

    fillterByValues(_listItem, _keywordSearch, propertyName, propertyRoute) {
        _keywordSearch = _keywordSearch.toLowerCase();
        return _listItem.filter(item => {
            if (item) {
                const nameResult = item[propertyName].toLowerCase().includes(_keywordSearch);
                const routerResult = item[propertyRoute] !== '';
                if (nameResult && routerResult) {
                    return item;
                }
            }
        });
    }

    getDataMstCmmLookUp() {
        // this.lookUp.getsByDomainCode('SYSTEM').subscribe((result) => {
        //     this.settingLookUp = result ?? []
        // }
        // )

    }
    closeQuickAccess() {
        const quickAccessElement = document.getElementById('quick-access');
        document.querySelector<HTMLInputElement>('.access-form .input-group .form-control').value = ''
        if (quickAccessElement.style.display = 'flex') {
            quickAccessElement.style.display = 'none';
        }
        this.tempFilterSearchResult = []

    }
}
