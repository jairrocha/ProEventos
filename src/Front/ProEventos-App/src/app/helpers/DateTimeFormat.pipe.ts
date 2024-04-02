import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Constants } from '../util/constants';

@Pipe({
  name: 'DateTimeFormat'
})
export class DateTimeFormatPipe extends DatePipe implements PipeTransform {

  override transform(value: any, args?: any): any {

    //const data = dataParam.replace(/(\d{2})\/(\d{2})\/(\d{4}) (\d{2}:\d{2}:\d{2})/, '$3-$2-$1T$4');

    return super.transform(value, Constants.DATE_TIME_FMT);
  }

}
