import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'capitalize'
})

export class CapitalizePipe implements PipeTransform {
  transform(value: string): string {
    // If the value exists
    if (!value)
      return value;

    // Change the string to captilize the first word!
    return value.charAt(0).toUpperCase() + value.slice(1);
  }
}
