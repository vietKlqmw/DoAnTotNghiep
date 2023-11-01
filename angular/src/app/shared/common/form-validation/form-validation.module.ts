import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ErrorMessageComponent } from './error-message.component';
import { ErrorDirective } from './error.directive';
import { HasErrorDirective } from './has-error.directive';
import { DebounceClickDirective } from './no-dbl-click.directive';
import { NumberCommaDirective } from './number-comma.directive';
import { SeparatorDirective } from './separator.directive';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    ErrorDirective,
    ErrorMessageComponent,
    HasErrorDirective,
    DebounceClickDirective,
    NumberCommaDirective,
    SeparatorDirective,
  ],
  exports: [
    ErrorDirective,
    ErrorMessageComponent,
    HasErrorDirective,
    DebounceClickDirective,
    NumberCommaDirective,
    SeparatorDirective
  ]
})

export class FormValidationModule {
}