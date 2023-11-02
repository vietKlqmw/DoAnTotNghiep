
export class CommonFunction {

    setHeight() {

        let w = window.innerWidth;
        let objSearch = document.querySelectorAll<HTMLElement>("#container .form-group .input-group");
        let h_search = 0;
        if (objSearch.length > 0) { h_search = objSearch[0].offsetHeight }

        let h = (window.innerHeight - (55 + 10 + 32 + h_search + 28 + 39 + 35 )) + 'px'; //top bar header: 12 + 30 + (10)
        document.querySelector<HTMLElement>('simple-ag-grid ag-grid-angular').style.height = h;
    }

    // dùng trong simple-ag-grid, grid-table ...



    setHeight_notFullHeight(_heightAuto?:boolean, _count?:number) {

        // let w = window.innerWidth;
        // console.log('--------------- height------------');
        if(!_count) _count = 0;
        let h_menuTop = 0;  // menu top, chung cho tất cả các tab
        let objMenutop = document.querySelector("#kt_header.active");
        if (objMenutop) { h_menuTop = 44 + 6; }

        let h_tab = 0;  // height chung cho tất cả các Tab
        let objTab = document.querySelectorAll<HTMLElement>('tabset.tab-container ul[role="tablist"]');
        if (objTab.length > 0) { h_tab = objTab[0].offsetHeight + 6; }
        //console.log('objTab:'  +h_tab)

        let h_pagination = 0;


        // số lượng Tab
        let tab_count = document.querySelectorAll<HTMLElement>("tabset.tab-container:not(.sub-tab) .tab-content tab[role=tabpanel]");
        for(let _tab = 0; tab_count[_tab]; _tab++){
            // console.log(tab_count[_tab].getAttribute("ng-reflect-heading"));

            let objGridview = tab_count[_tab].querySelectorAll<HTMLElement>('div[id="container"] simple-ag-grid , div[id="container"]  grid-table:not(.grid-dropdown-list) ');
            if(objGridview.length > 0) {

                let h_titleGrid = 0;
                let objTitleGrid = tab_count[_tab].querySelectorAll<HTMLElement>("#container .card-header");
                if (objTitleGrid.length > 0) { h_titleGrid = (18 * objTitleGrid.length) + 8; }

                let h_search = 0;   // height Search -> theo từng tab
                let objSearch = document.querySelectorAll<HTMLElement>("#container .form-group.m-form__group.align-items-center > .input-group");
                // if (objSearch.length > 0) { h_search = objSearch[0].offsetHeight; }
                for(let ibtn=0; objSearch[ibtn]; ibtn++) { h_search = h_search + objSearch[ibtn].offsetHeight;}

                // Hr
                let h_hr = 0;
                let objHR = tab_count[_tab].querySelectorAll<HTMLElement>("#container hr");
                if(objHR.length > 0) h_hr = objHR.length * 20;

                // Button
                let h_btn = 0;
                let objbtn = tab_count[_tab].querySelectorAll<HTMLElement>("#container .panelBtn.text-right");
                for(let ibtn=0; objbtn[ibtn]; ibtn++) {
                    let _btntmp = objbtn[ibtn].querySelectorAll<HTMLElement>(".btn");
                    if(_btntmp.length > 0) h_btn = h_btn + 40;
                    else h_btn = h_btn + 10;
                }// default //objbtn[ibtn].offsetHeight + 10;}
                if(objbtn.length == 0) { h_btn = 30 + 10; }

                // Pagination
                h_pagination = 0;
                let objPagination = tab_count[_tab].querySelectorAll<HTMLElement>('div[id="container"] simple-ag-grid grid-pagination, div[id="container"]  grid-table:not(.grid-dropdown-list) grid-pagination');
                if(objPagination.length > 0) h_pagination = objPagination.length * 32;

                // padding
                let h_padding_page = 15;

                let h = (window.innerHeight - (h_menuTop + h_tab + h_titleGrid + h_search + h_hr + h_btn + h_pagination + h_padding_page)); //top bar header: 12 + 30 + (10)

                h = (h - (objGridview.length *10)); //objGridview.length *10 mỗi grid 1 paddingbottom
                h = Math.ceil(h / objGridview.length);

                // console.log('h_grid:'  +h + ' - h_pagination:'  +h_pagination + ' - h_search:'  +h_search + ' - h_btn:'  +h_btn + ' - h_hr:'  +h_hr );

                for(let i=0; i< objGridview.length; i++) {

                    // let _autoHeight = objGridview[i].getAttribute('ng-reflect-height-auto');
                    // if(_autoHeight == "false") { continue; }
                    // objGridview[i].querySelector<HTMLElement>("ag-grid-angular").style.height = h + 'px';

                    let _objGridview = objGridview[i].querySelector<HTMLElement>("ag-grid-angular.heightAutotrue");
                    if (_objGridview) _objGridview.style.height = h + 'px';
                }
            }
            else if (_count <=1){
                setTimeout(() => {
                    if(_count){ _count = _count + 1; }
                    else{ _count = 1; }
                    this.setHeight_notFullHeight(_heightAuto,_count);
                }, 2000);
            }
        }

        // trường hợp màn hinh không có tab
        if(tab_count.length == 0){

            let objGridview = document.querySelectorAll<HTMLElement>('div[id="container"] simple-ag-grid , div[id="container"] grid-table:not(.grid-dropdown-list)');
            if(objGridview.length > 0) {

                let h_titleGrid = 0;
                let objTitleGrid = document.querySelectorAll<HTMLElement>("#container .card-header");
                if (objTitleGrid.length > 0) { h_titleGrid = (18 * objTitleGrid.length) + 8; }

                let h_search = 0;   // height Search -> theo từng tab
                let objSearch = document.querySelectorAll<HTMLElement>("#container .form-group.m-form__group.align-items-center > .input-group");
                for(let ibtn=0; objSearch[ibtn]; ibtn++) { h_search = h_search + objSearch[ibtn].offsetHeight;}

                // Hr
                let h_hr = 0;
                let objHR = document.querySelectorAll<HTMLElement>("#container hr");
                if(objHR.length > 0) h_hr = objHR.length * 20;

                // Button
                let h_btn = 0;
                let objbtn = document.querySelectorAll<HTMLElement>("#container .panelBtn.text-right");
                for(let ibtn=0; objbtn[ibtn]; ibtn++) {
                    let _btntmp = objbtn[ibtn].querySelectorAll<HTMLElement>(".btn");
                    if(_btntmp.length > 0) h_btn = h_btn + 40;
                    else h_btn = h_btn + 10;
                }// default //objbtn[ibtn].offsetHeight + 10;}
                if(objbtn.length == 0) { h_btn = 30 + 10; }


                // Pagination
                let objPagination = document.querySelectorAll<HTMLElement>('div[id="container"] simple-ag-grid grid-pagination, div[id="container"]  grid-table:not(.grid-dropdown-list) grid-pagination');
                if(objPagination.length > 0) h_pagination = objPagination.length * 32;

                // padding
                let h_padding_page = 15;

                let h = (window.innerHeight - (h_menuTop + h_tab + h_titleGrid + h_search + h_hr + h_btn + h_pagination + h_padding_page)); //top bar header: 12 + 30 + (10)

                h = (h - (objGridview.length *10)); //objGridview.length *10 mỗi grid 1 paddingbottom
                h = Math.ceil(h / objGridview.length);

                // console.log('h_grid:'  +h + ' - h_pagination:'  +h_pagination + ' - h_search:'  +h_search + ' - h_btn:'  +h_btn + ' - h_hr:'  +h_hr );

                for(let i=0; i< objGridview.length; i++) {

                    // let _autoHeight = objGridview[i].getAttribute('ng-reflect-height-auto');
                    // if(_autoHeight == "false") { continue; }
                    // objGridview[i].querySelector<HTMLElement>("ag-grid-angular").style.height = h + 'px';

                    let _objGridview = objGridview[i].querySelector<HTMLElement>("ag-grid-angular.heightAutotrue");
                    if (_objGridview) _objGridview.style.height = h + 'px';
                }
            }
            else if (_count <=1){
                setTimeout(() => {
                    if(_count){ _count = _count + 1; }
                    else{ _count = 1; }
                    this.setHeight_notFullHeight(_heightAuto,_count);
                }, 2000);
            }
        }


        // if(!_heightAuto) return;
    }

    // // dùng khi click menu left
    // setHeight_actionmenuleft() {
    //    this.setHeight_actionmenuTop();
    // }

    // dùng khi click menu top
    // setHeight_actionmenuTop() {

    //     // let w = window.innerWidth;

    //     let h_menuTop = 0;  // menu top, chung cho tất cả các tab
    //     let objMenutop = document.querySelector("#kt_header.active");
    //     if (objMenutop) { h_menuTop = 44 + 6; }

    //     let h_tab = 0;  // height chung cho tất cả các Tab
    //     let objTab = document.querySelectorAll<HTMLElement>('tabset.tab-container ul[role="tablist"]');
    //     if (objTab.length > 0) { h_tab = objTab[0].offsetHeight + 6; }
    //     console.log('objTab:'  +h_tab)

    //     // let h_btn_hr = 50;
    //     let h_pagination = 32;

    //     // số lượng Tab
    //     let tab_count = document.querySelectorAll<HTMLElement>("tabset.tab-container:not(.sub-tab) .tab-content tab[role=tabpanel]");
    //     for(let _tab = 0; tab_count[_tab]; _tab++){
    //         // console.log(tab_count[_tab].getAttribute("ng-reflect-heading"));

    //         let objGridview = tab_count[_tab].querySelectorAll<HTMLElement>('div[id="container"] simple-ag-grid , div[id="container"] grid-table:not(.grid-dropdown-list) ');
    //         if(objGridview.length > 0) {

    //             let h_titleGrid = 26;
    //             let objTitleGrid = tab_count[_tab].querySelectorAll<HTMLElement>("#container .card-header.card-grid-table");
    //             if (objTitleGrid.length > 0) { h_titleGrid = h_titleGrid + (18 * objTitleGrid.length); }

    //             let h_search = 0;   // height Search -> theo từng tab
    //             let objSearch = document.querySelectorAll<HTMLElement>("#container .form-group.m-form__group.align-items-center > .input-group");
    //             // if (objSearch.length > 0) { h_search = objSearch[0].offsetHeight; }
    //             for(let ibtn=0; objSearch[ibtn]; ibtn++) { h_search = h_search + objSearch[ibtn].offsetHeight;}

    //             let h_btn_hr = 0;
    //             let objHR = tab_count[_tab].querySelectorAll<HTMLElement>("#container hr");
    //             if(objHR.length > 0) h_btn_hr = objHR.length * 20;


    //             let objbtn = tab_count[_tab].querySelectorAll<HTMLElement>("#container .panelBtn.text-right");
    //             h_pagination = (objbtn.length > 0) ? 0 : 32;
    //             for(let ibtn=0; objbtn[ibtn]; ibtn++) { h_btn_hr = h_btn_hr + objbtn[ibtn].offsetHeight + 10;}

    //             // h_pagination = h_pagination*objGridview.length;

    //             let h = (window.innerHeight - (h_menuTop + h_tab + h_titleGrid + h_search + h_btn_hr + h_pagination + 30)); //top bar header: 12 + 30 + (10)

    //             h = (h - (objGridview.length *10)); //objGridview.length *10 mỗi grid 1 paddingbottom
    //             h = Math.ceil(h / objGridview.length);

    //             for(let i=0; i< objGridview.length; i++) {

    //                 // let _autoHeight = objGridview[i].getAttribute('ng-reflect-height-auto');
    //                 // if(_autoHeight == "false") { continue; }
    //                 // objGridview[i].querySelector<HTMLElement>("ag-grid-angular").style.height = h + 'px';

    //                 let _objGridview = objGridview[i].querySelector<HTMLElement>("ag-grid-angular.heightAutotrue");
    //                 if (_objGridview) _objGridview.style.height = h + 'px';
    //             }
    //         }
    //     }

    //     // trường hợp màn hinh không có tab
    //     if(tab_count.length == 0){

    //         let objGridview = document.querySelectorAll<HTMLElement>('div[id="container"] simple-ag-grid , div[id="container"] grid-table:not(.grid-dropdown-list) ');
    //         if(objGridview.length > 0) {

    //             let h_titleGrid = 26;
    //             let objTitleGrid = document.querySelectorAll<HTMLElement>("#container .card-header.card-grid-table");
    //             if (objTitleGrid.length > 0) { h_titleGrid = h_titleGrid + (18 * objTitleGrid.length); }

    //             let h_search = 0;   // height Search -> theo từng tab
    //             let objSearch = document.querySelectorAll<HTMLElement>("#container .form-group.m-form__group.align-items-center > .input-group");
    //             // if (objSearch.length > 0) { h_search = objSearch[0].offsetHeight; }
    //             for(let ibtn=0; objSearch[ibtn]; ibtn++) { h_search = h_search + objSearch[ibtn].offsetHeight;}

    //             let h_btn_hr = 0;
    //             let objHR = document.querySelectorAll<HTMLElement>("#container hr");
    //             if(objHR.length > 0) h_btn_hr = objHR.length * 20;


    //             let objbtn = document.querySelectorAll<HTMLElement>("#container .panelBtn.text-right");
    //             h_pagination = 32; //(objbtn.length > 0) ? 0 : 32;
    //             for(let ibtn=0; objbtn[ibtn]; ibtn++) { h_btn_hr = h_btn_hr + objbtn[ibtn].offsetHeight + 10;}
    //             // h_pagination = h_pagination*objGridview.length;
    //             let h = (window.innerHeight - (h_menuTop + h_tab + h_titleGrid + h_search + h_btn_hr + h_pagination + 30 + 20)); //top bar header: 12 + 30 + (10)

    //             h = (h - (objGridview.length *10)); //objGridview.length *10 mỗi grid 1 paddingbottom
    //             h = Math.ceil(h / objGridview.length);

    //             for(let i=0; i< objGridview.length; i++) {

    //                 // let _autoHeight = objGridview[i].getAttribute('ng-reflect-height-auto');
    //                 // if(_autoHeight == "false") { continue; }
    //                 // objGridview[i].querySelector<HTMLElement>("ag-grid-angular").style.height = h + 'px';

    //                 let _objGridview = objGridview[i].querySelector<HTMLElement>("ag-grid-angular.heightAutotrue");
    //                 if (_objGridview) _objGridview.style.height = h + 'px';
    //             }
    //         }
    //     }

    // }

    isActive(_active: any){
        if(_active) return _active;
        return 'N';
    }

    isStatus(_active: any, _default: string){
        if(_active) return _active;
        return _default;
    }

    showtime(css_time:string){
        let _d = new Date();
        let _time = document.querySelector<HTMLElement>('.' + css_time);
        if(_time) _time.textContent = _d.getHours() + ":" + _d.getMinutes() + ":" + _d.getSeconds();
    }

    getPercentByQty(_total:number, _Qty:number) {
        return (_total / _Qty) * 100;
    }
    getPercentByQty2(percent:number, total:number) {
        return (((percent / 100) * total) / 100).toFixed(2)
    }


    getQtyByPercent(_total:number, _percent:number) {
        let _Qty = (_total * _percent) / 100;
        //_Qty = Math.floor(_Qty * 100) / 100;
        return _Qty;
    }
    getQtyByPercent2(_qty:number, _percent:number) {
        return (_qty * 100) / _percent;
    }


    numbers:Array<any> = [];
    fornumbers(num:number) {
        this.numbers = Array.from({length:num},(v,k)=>k+1);
        return this.numbers;
    }

    fornumbers_next(num:number, start:number) {
        this.numbers = Array.from({length:num},(v,k)=>k+start);
        return this.numbers;
    }

    numbers_desc:Array<any> = [];
    fornumbers_desc(num:number) {
        this.numbers_desc = Array.from({length:num},(v,k)=>num-k);
        return this.numbers_desc;
    }

    fornumbersRangeDesc(start:number, stop:number, step:number){
        let numRangeDesc: number[] = [];
        for (let i = start; i >= stop;) {
            numRangeDesc.push(i);
            i = i + step;
        }
        return numRangeDesc;
    }

    fornumbersRange(start:number, stop:number, step:number){
        let numRange: number[] = [];
        for (let i = start; i <= stop;) {
            numRange.push(i);
            i = i + step;
        }
        return numRange;
    }


    toHHMMSS(ttseconds:number) {
        let sec_num = ttseconds; // parseInt(ttseconds, 0); // don't forget the second param
        let hours = Math.floor(sec_num / 3600);
        let minutes = Math.floor((sec_num - (hours * 3600)) / 60);
        let seconds = sec_num - (hours * 3600) - (minutes * 60);
        let _hours = '', _minutes = '', _seconds = '';

        _hours = (hours < 10) ? ("0" + hours) : ("" + hours);
        _minutes = (minutes < 10) ? ("0" + minutes) : ("" + minutes);
        _seconds = (seconds < 10) ? ("0" + seconds) : ("" + seconds);
        var time = _hours + ':' + _minutes + ':' + _seconds;
        return time;
    }

    toMMSS(ttseconds:number) {
        let sec_num = ttseconds; //var sec_num = parseInt(ttseconds, 0); // don't forget the second param
        let _minutes = '', _seconds = '';

        var minutes = Math.floor(sec_num / 60);
        var seconds = sec_num - (minutes * 60);

        _minutes = (minutes < 10) ? ("0" + minutes) : ("" + minutes);
        _seconds = (seconds < 10) ? ("0" + seconds) : ("" + seconds);
        var time = _minutes + ':' + _seconds;
        return time;
    }

    getMonthEN(m:number) {
        switch (m) {
            case 0: return "January";
            case 1: return "February";
            case 2: return "March";
            case 3: return "April";
            case 4: return "May";
            case 5: return "June";
            case 6: return "July";
            case 7: return "August";
            case 8: return "September";
            case 9: return "October";
            case 10: return "November";
            case 11: return "December";
            default: return m;
        }
    }
    getTime(dt:any) {
        var strtime = (((dt.getHours() + "").length == 1) ? ("0" + dt.getHours()) : dt.getHours()) + " : " + (((dt.getMinutes() + "").length == 1) ? ("0" + dt.getMinutes()) : dt.getMinutes())
        return strtime;
    }

    loading(_isLoad:boolean){
        let _loading = document.querySelector<HTMLElement>('.controlMessage img._loading');
        if(_loading){
            if(_isLoad) _loading.style.display = "block";
            else _loading.style.display = "none";
        }

    }

    escapeRegExp(valstr) {
        return valstr.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); // $& means the whole matched string
    }

    replaceAll(val, oldValue, newValue) {
        return val.replace(new RegExp(this.escapeRegExp(oldValue), 'g'), newValue);
    }

    start_time:any;
    end_time:any;
    exportLoading(e, _isLoad?:boolean) {
        console.log(e);
        if (_isLoad) {
            this.start_time = new Date();
            e.srcElement.classList.add('exportExcel');
        }
        else {
            this.end_time = new Date();
            let s = this.end_time.getSeconds() - this.start_time.getSeconds();
            let ms = this.end_time.getMilliseconds() - this.start_time.getMilliseconds();
            let coundown = 2000-ms; coundown = (coundown > 0) ? coundown: 0;
            setTimeout(() => {
                e.srcElement.classList.remove('exportExcel');
            }, coundown);
            return coundown;
         }
    }


    isInt(value) {
        return !isNaN(value) &&
                    parseInt(Number(value).toString()) == value &&
                    !isNaN(parseInt(value, 10));
    }

}
