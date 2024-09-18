import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PriceListComponent } from './price-list.component';
import { SharedModule } from '../shared/shared.module';
import { NgxPrintModule } from 'ngx-print';



@NgModule({
  declarations: [
    PriceListComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    NgxPrintModule
  ],
  exports:[PriceListComponent]
})
export class PriceListModule { }
