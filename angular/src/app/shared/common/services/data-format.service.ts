import { Injectable } from '@angular/core';
import * as moment from 'moment';

@Injectable()
export class DataFormatService {

  constructor() { }

  convertStringToDate(value: string) {
    const [day, month, year] = value.split('/');
    return new Date(+year, +month - 1, +day);
  }

  // Tiền
  moneyFormat(value: Number | number | string) {
    return value ? Math.round(Number(value)).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,') : '0';
  }

  // Số
  numberFormat(value: string | number) {
    if (value === 0) {
      return '0';
    }
    // @ts-ignore
    // return value ? parseFloat(Math.round((+value) * 100) / 100).toFixed(2) : '';
    return value ? parseFloat(value).toString() : '';
  }

  numberValidate(value: string | number) {
    const NUMBER_REGEX = /^(-?\d+\.\d+)$|^(-?\d+)$|null|undefined/;
    //const NUMBER_REGEX = /^(0|[1-9]\d*)?(\.\d+)?(?<=\d)$/;
    return NUMBER_REGEX.test(value?.toString());
  }

  // Ngày
  dateFormat(val: string | moment.Moment | Date) {
    // if (val == '0001-01-01T00:00:00' || val == '9999-12-31T23:59:59.9999999') return '';
    return val ? moment(val).tz(this.getTimeZone())?.format('DD/MM/YYYY') : '';
  }

  monthFormat(val: string | number) {
    return val ? moment(val).format('MM-YYYY') : ''
  }

  timeFormat(val: string | moment.Moment | Date) {
    return val ? moment(val).format('HH:mm') : ''
  }

  getTimeZone() {
    const localeTimezone = abp.localization.currentLanguage;
    if (localeTimezone.name === 'vi') return 'Asia/Ho_Chi_Minh';
    if (localeTimezone.name === 'en') return 'America/New_York';
    if (localeTimezone.name === 'es-MX') return 'Mexico';
    if (localeTimezone.name === 'es') return 'Spain';
    return '';
  }

  // Ngày giờ
  dateTimeFormat(val: any) {
    // if (val == '0001-01-01T00:00:00' || val == '9999-12-31T23:59:59.9999999') return '';
    return val ? moment(val).tz(this.getTimeZone())?.format('DD/MM/YYYY HH:mm') : '';
  }
  dateTimeBtsFormat(val: any) {
    // if (val == '0001-01-01T00:00:00' || val == '9999-12-31T23:59:59.9999999') return '';
    return val ? moment(val).tz(this.getTimeZone())?.format('DD-MMM-YYYY') : '';
  }

  // Biển số xe
  registerNoValidate(registerNo: string) {
    const REGISTER_NO_REGEX = /^\d{2}\D{1}[-. ]?\d{4}[\d{1}]?$/g;
    return REGISTER_NO_REGEX.test(registerNo);
  }

  // Số điện thoại
  phoneNumberValidate(phoneNumber: string) {
    const PHONE_NUMBER_REGEX = /(0|[+]([0-9]{2})){1}[ ]?[0-9]{2}([-. ]?[0-9]){7}|((([0-9]{3}[- ]){2}[0-9]{4})|((0|[+][0-9]{2}[- ]?)(3|7|8|9|1)([0-9]{8}))|(^[\+]?[(][\+]??[0-9]{2}[)]?([- ]?[0-9]{2}){2}([- ]?[0-9]{3}){2}))$/gm;
    return !phoneNumber || PHONE_NUMBER_REGEX.test(phoneNumber);
  }

  // Validate Phone number
  phoneNumberCheckValidate(phoneNumber?: string) {
      const PHONE_NUMBER_VALIDATE = /(^\(?([0-9]{4})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{3})$)|(^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$)|(^\(?([0-9]{3}|[0-9]{4})\)?([0-9]{3}|[0-9]{4})?([0-9]{3}|[0-9]{4})$)/gm;
      return !phoneNumber || (phoneNumber.length > 9 && phoneNumber.length < 13 && PHONE_NUMBER_VALIDATE.test(phoneNumber));
  }
  // Mã màu
  colorValidate(color: string) {
    const COLOR_REGEX = /(?:#|0x)(?:[a-f0-9]{3}|[a-f0-9]{6})\b|(?:rgb|hsl)a?\([^\)]*\)/ig;
    return COLOR_REGEX.test(color);
  }

  // Không được là số âm
  notNegativeNumberValidate(params: number | string) {
    return !(params !== '' && (params && Number(params) < 0));
  }

  // Số nguyên dương
  positiveNumberValidate(params: string | number) {
    const NUMBER_REG = /^(?!(?:^[-+]?[0.]+(?:[Ee]|$)))(?!(?:^-))(?:(?:[+-]?)(?=[0123456789.])(?:(?:(?:[0123456789]+)(?:(?:[.])(?:[0123456789]*))?|(?:(?:[.])(?:[0123456789]+))))(?:(?:[Ee])(?:(?:[+-]?)(?:[0123456789]+))|))$/
    //const NUMBER_REG = /^\d*[1-9]+\d*$/;
    // return !(params.value !== '' && ((params.value && Number(params.value) <= 0) || !NUMBER_REG.test(params.value)));
    return NUMBER_REG.test(params.toString());
  }

  // Số nguyên không âm
  notNegativeIntNumberValidate(params: any) {
    const NUMBER_REG = /^\d+$/g;
    return NUMBER_REG.test(params);
  }

  // Số nguyên
  intNumberValidate(params: string) {
    const NUMBER_REG = /^([+-]?[1-9]\d*|0)$/;
    return !(params !== '' && !NUMBER_REG.test(params));
  }

  emailValidate(email: string) {
    const NUMBER_REG = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g;
    return NUMBER_REG.test(email);
  }

  /* For Sale */
  // Format input trong Cell Grid sang dạng DateTime để truyền xuống database
  formatInputToDateTime(input: string | Date) {
    const date = new Date(input);
    return date;
  }

  // format từ số giây sang giờ
  formatHoursSecond(seconds: number) {
    if (seconds && seconds > 0) {
      const hours = Math.floor(seconds / 3600) < 10 ? `0${Math.floor(seconds / 3600)}` : Math.floor(seconds / 3600);
      const minutes = Math.floor((seconds % 3600) / 60) < 10 ? `0${Math.floor((seconds % 3600) / 60)}` : Math.floor((seconds % 3600) / 60);
      // const second = seconds % 60 < 10 ? `0${seconds % 60}` : seconds % 60;
      return `${hours}:${minutes}`;
    } else if (seconds === 0) {
      return `00:00`;
    } else if (!seconds) {
      return '';
    }
  }

  //convert từ giờ nhập vào sang số giây
  convertTimeToSeconds(time: string): number {
    return time.split(':').reverse().reduce((prev, curr, i) => prev + +curr * Math.pow(60, i), 0);
  }

  //format Date for sale
  formatDateForSale(date: any) {
    const isFirefox = /Firefox[\/\s](\d+\.\d+)/.test(navigator.userAgent);
    if (date) {
      let convertDate;
      if (typeof date === 'string' && date.length === 1) {
        return date;
      }

      if (isFirefox && typeof date === 'string') {
        const dateArr = date.split('-');
        date = `${dateArr[1]} ${dateArr[0]}, ${dateArr[2]}`;
      }
      convertDate = new Date(date);
      const displayDate = convertDate.getDate() < 10 ? `0${convertDate.getDate()}` : convertDate.getDate();
      const formattedMonth = convertDate.getMonth() < 9 ? `0${convertDate.getMonth() + 1}` : convertDate.getMonth() + 1;
      const displayMonth = moment(formattedMonth, 'MM').format('MMM');

      return convertDate ? `${displayDate}-${displayMonth}-${convertDate.getFullYear()}` : '';
    }
    return '';
  }

  formatDisplayValue(val: any, hideDecimal?: boolean) {
    val = hideDecimal ? val : this.numberFormat(val);
    if (val) {
      if (typeof val === 'string') {
        let num = val.trim().replace(/,([0-9]{3})/g, '$1');
        if ((+num).toString() === num) {
          return (+num).toLocaleString();
        }

        num = val.trim().replace(/,/g, '');
        const NUMBER_REGEX = /^([0-9]*)$/g;
        if (NUMBER_REGEX.test(num)) {
          return (+num).toLocaleString();
        }
        return val;
      } else {
        const num = val.toString().replace(/,/g, '');
        return (+num) ? (+num).toLocaleString() : val;
      }
    }
    return '';
  }

  formatMoney(val: number | string) {
    if (val) {
      if (typeof val === 'string') {
        let num = val.trim().replace(/\,([0-9]{3})/g, '$1');

        num = val.trim().replace(/\,/g, '');
        const NUMBER_REGEX = /^([0-9]*)$/g;
        if (NUMBER_REGEX.test(num)) {
          return parseFloat(num).toLocaleString('en-US');
        }
        return val;
      } else {
        const num = val.toString().replace(/\,/g, '');
        return parseFloat(num) ? parseFloat(num).toLocaleString('en-US') : val;
      }
    }
    return 0;
  }

    /**
     *
     *
     * @param {*} val : Giá trị value truyền vào.
     * @param {number} [_decimalMax] : Max decimal hiển thị.
     * @param {boolean} [_decimalFull] : ex: 123.45 -> 123.4500. default: true
     * @param {boolean} [_decimalMath] : Làm tròn khi decimal của value vượt quá decimalMax. default: false
     * @param {string} [_mathType] : Kiểu làm tròn ex: "round", "floor", "ceil". default = "round"
     * @return {*}  {*}
     * @memberof DataFormatService
     */
  formatMoney_decimal(val: any, _decimalMax?:number, _decimalFull?:boolean, _decimalMath?:boolean, _mathType?: string): any {

    if(_decimalFull == undefined) _decimalFull = true;
    if(_decimalMath == undefined) _decimalMath = true;
    if(_mathType == undefined) _mathType = "round";

    if(_decimalMax == undefined) {
        try{
            if(val) {
                let num = val.toString().replace(/,/g, '');
                let _numVal = (+num) ? (+num).toLocaleString("en-US") : val;
                return _numVal;
            }
            return 0;
        } catch(e){
            console.error(e);
            return val;
        }
    }
    else if (val) {
        try{
        /*  //trường hợp giá trị đã format trong db
            if (typeof val === 'string') {
                let num = val.trim().replace(/,([0-9]{3})/g, '$1');
                if ((+num).toString() === num) {
                    return (+num).toLocaleString("en-US");  //lost decimal
                }
                num = val.trim().replace(/,/g, '');
                let NUMBER_REGEX = /^([0-9]*)$/g;
                if (NUMBER_REGEX.test(num)) {
                    return (+num).toLocaleString("en-US");  //lost decimal
                }
                return val;
            }
            else {
        */
                if(_decimalMax == 0) {
                    let num = val.toString().replace(/,/g, '');
                    let _numVal = (+num) ? (+num).toLocaleString("en-US") : val;
                    return _numVal.toString().split(".")[0];
                }
                else {

                    let _val = parseFloat(val);
                    if(Number.isNaN(_val)) return val;

                    // decimalMath = true
                    let _valTrim = val;
                    if(_decimalMath) {
                        let _decimaltmp = 1;
                        this.fornumbers(_decimalMax).forEach( function(value) {_decimaltmp = _decimaltmp * value; });

                        if(_mathType == "ceil")  _valTrim = Math.ceil(parseFloat(val) * _decimaltmp) / _decimaltmp;
                        else if (_mathType == "floor") _valTrim = Math.floor(parseFloat(val) * _decimaltmp) / _decimaltmp;
                        else _valTrim = Math.round(parseFloat(val) * _decimaltmp) / _decimaltmp;
                    }


                    let _nums = _valTrim.toString().split(".");

                    //[0]
                    let _num1Valtmp = _nums[0].toString().replace(/,/g, '');
                    let _num1Val = (+_num1Valtmp) ? (+_num1Valtmp).toLocaleString("en-US") : _nums[0];


                    //[1]
                    let _num2Val = "";
                    if(_nums.length > 1 ) {

                        _num2Val = _nums[1];
                        // decimalMath = false
                        if(!_decimalMath) {
                            let _decimalMaxtmp = _decimalMax > _nums[1].length ? _nums[1].length: _decimalMax;
                            _num2Val = _nums[1].substr(0, _decimalMaxtmp);
                        }

                        //decimalFull
                        if(_decimalFull) {
                            _num2Val = (_num2Val + "00000000000").substr(0, _decimalMax);
                        }
                        _num2Val = "." + _num2Val;
                    }
                    else {
                        _num2Val = "." + "00000000000000000".substr(0, _decimalMax);
                    }

                    let _numVal = _num1Val + _num2Val;
                    return _numVal;
                }
            // }
        } catch(e){
            console.error(e);
            return val;
        }
    }
    return 0;
}

fornumbers(num:number) {
  let numbers = Array.from({length:num},(v,k)=>10);
  return numbers;
}

}
