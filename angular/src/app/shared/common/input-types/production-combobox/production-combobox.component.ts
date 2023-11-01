import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Component, Input, forwardRef, Output, EventEmitter, ViewChild, ElementRef, OnInit, SimpleChanges } from '@angular/core';

@Component({
    selector: 'production-combobox',
    templateUrl: './production-combobox.component.html',
    styleUrls: ['./production-combobox.component.less'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => ProductionComboboxComponent),
            multi: true,
        },
    ],
})
export class ProductionComboboxComponent implements ControlValueAccessor {

    // Giống như cách sử dụng tmss combobox
    @Input() className: string = '';
    @Input() value: any;
    @Input() items: any[] = [];
    @Input() text: string = '';
    @Input() isRequired: boolean = false;
    @Input() isValidate: boolean = false;
    @Input() isDisabled: boolean = false;
    @Input() isReadonly: boolean = false;
    @Input() selectedItem: any;
    @Input() label: string = 'label';
    @Input() key: string = 'value';
    @Input() type: string = 'text';
    @Input() placeholder: string = '';
    @Input() returnTarget: boolean = false;
    @Input() colorlabel: boolean = false;
    @Input() mlcombobox: string = '';
    //--------------------------------------

    @Output() onChangeValue = new EventEmitter();
    @ViewChild('input', { static: false }) input!: ElementRef;

    listVisible = false;
    isPlaceholder = true;
    selectedDate: any;

    datalabel = [{ value: '', label: ''}];
    filteredList = [];
    @Input() list = [];
    listHidden = true;
    selectedIndex = -1;
    valueDisplay;

    withOption: number

    // Xử lý lỗi, hiện chưa cần
    showError = false;

    private onChange: Function = new Function();

    constructor(
    ) {
    }

    // update chiều dài của option phụ thuộc vào input
     updateInputWidth() {
        const inputElements = document.querySelectorAll<HTMLElement>('.combobox-input');
        var inputWidth = [];

        inputElements.forEach((element: HTMLElement,index: number) => {
          inputWidth.push(element.offsetWidth);
         if (element === document.activeElement) {
            this.withOption = inputWidth[index];
          }
        });
      }

    // ngOnInit() {
    //     this.filteredList = this.list;
    // }

    // Đồng bộ giá trị

    writeValue(val: any): void {
        this.value = val ?? '';
        this.datalabel = this.list.filter(e => e.value == this.value);
        this.valueDisplay = this.datalabel.length > 0 ? this.datalabel[0].label : '';
    }

    // Đăng kí callback (Do kế thừa ControlValueAccessor )
    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    // Giống registerOnChange, xử lý khi chạm ra ngoài
    registerOnTouched(fn: any): void {
        this.filteredList = this.list;
    }

    setDisabledState(isDisabled: boolean): void {
        this.isDisabled = isDisabled;
    }


    // Tìm kiếm không phân biệt chữ hoa chữ thường
    getFilteredList() {
        // Xử lý array

        // this.listHidden = false;
        // if (!this.listHidden && this.value !== undefined) {
        //     this.filteredList = this.list.filter((item) => item.toLowerCase().startsWith(this.value.toLowerCase()));
        // }

        // Xử lý object
        this.listHidden = false;
        if (!this.listHidden && this.valueDisplay !== undefined) {
            this.filteredList = this.list.map(item => item[this.label]).filter(
                item => item.toLowerCase().includes(this.valueDisplay.toLowerCase())
            );
        }
    }

    selectItem(ind) {

        if (ind >= 0 && ind < this.filteredList.length) {

            const selectedItemValue = this.filteredList[ind];

            // Cập nhật giá trị và mục đã chọn
            // this.value = selectedItemValue;
            this.valueDisplay = this.filteredList[ind];
            this.selectedIndex = ind;
            this.value = this.list.filter(e => e.label == this.valueDisplay)[0].value;

            if (typeof this.onChange === 'function') {
                this.onChange(this.value);
              }
            this.onChangeValue.emit(this.value)
       }
        this.listHidden = true;
    }

    // Xử lý các thao tác từ bàn phím
    onKeyPress(event) {

        if (!this.listHidden) {
            if (event.key === 'Escape') {
                this.selectedIndex = -1;
                this.toggleListDisplay(0);
            }

            if (event.key === 'Backspace') {
                this.toggleListDisplay(1);
            }

            if (event.key === 'Enter') {

                this.selectItem(this.selectedIndex);
            }
            if (event.key === 'ArrowDown') {

                this.listHidden = false;
                this.selectedIndex = (this.selectedIndex + 1) % this.filteredList.length;
                if (this.filteredList.length > 0 && !this.listHidden) {
                    document.getElementsByTagName('list-item')[this.selectedIndex].scrollIntoView();
                }
            } else if (event.key === 'ArrowUp') {

                this.listHidden = false;
                if (this.selectedIndex <= 0) {
                    this.selectedIndex = this.filteredList.length;
                }
                this.selectedIndex = (this.selectedIndex - 1) % this.filteredList.length;

                if (this.filteredList.length > 0 && !this.listHidden) {

                    document.getElementsByTagName('list-item')[this.selectedIndex].scrollIntoView();
                }
            }
        }
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.list && changes.list.currentValue) {
            this.filteredList = this.list.map(item => item[this.label]);
        }
    }


    toggleListDisplay(sender: number) {
        this.updateInputWidth()
        // Xử lý array

        if (sender === 1) {
            this.listHidden = false;
            this.getFilteredList();
        } else {
                if (this.valueDisplay === '') {
                    this.selectedIndex = 0;
                }
            setTimeout(() => {
                this.selectItem(this.selectedIndex);
                this.listHidden = true;
                if (!this.list.includes(this.valueDisplay)) {
                    this.showError = true;
                    this.filteredList = this.list.map(item => item[this.label]);
                } else {
                    this.showError = false;
                }
            }, 300);
        }


        // Settimeout xử lý bất đồng bộ về index của selected Item

        // if (sender === 1) {
        //     this.listHidden = false;
        //     this.getFilteredList();
        // } else {
        //     setTimeout(() => {
        //         this.filteredList = this.list.map(item => item[this.label]);
        //         this.selectItem(this.selectedIndex);
        //         this.listHidden = true;
        //         if (!this.list.find(item => item[this.label] === this.value)) {
        //             this.showError = true;
        //             this.filteredList = this.list.map(item => item[this.label]);
        //         } else {
        //             this.showError = false;
        //         }
        //     }, 100);
        // }
    }

    getAllList(){
        this.listHidden = false;
        this.filteredList = this.list.map(item => item[this.label]);
    }
}

