import { NgModule } from '@angular/core';
import { CashierRoutingModule } from './cashier-routing.module';
import { SharedModule } from '../shared/shared.module';
import { PaymentComponent } from './payment/payment.component';
import { InvoiceComponent } from './invoice/invoice.component';
import { ProvidePaymentComponent } from './provide-payment/provide-payment.component';
import { ProvideDiscountComponent } from './provide-discount/provide-discount.component';
import { ReportsModule } from '../reports/reports.module';

@NgModule({
  declarations: [
    PaymentComponent,
    InvoiceComponent,
    ProvidePaymentComponent,
    ProvideDiscountComponent
  ],
  imports: [
    CashierRoutingModule,
    SharedModule
  ]
})
export class CashierModule { }
