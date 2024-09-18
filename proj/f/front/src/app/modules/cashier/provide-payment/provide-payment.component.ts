import { EventEmitter, Input, Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { PaymentType } from 'src/app/core/enums';
import { InvoiceModel, PaymentModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';

@Component({
  selector: 'app-provide-payment',
  templateUrl: './provide-payment.component.html',
  styleUrls: ['./provide-payment.component.scss']
})
export class ProvidePaymentComponent implements OnInit {

  @Input() public set openDialog(isOpen: boolean) {
    this.isOpen = isOpen;
    if (isOpen) {
      this.model = { isFromBalance: false, paymentType: PaymentType.Cash } as PaymentModel;
    } else {
      this.model = null;
    }
  }
  @Input() public invoice: InvoiceModel;

  @Output() paymentProvided: EventEmitter<any> = new EventEmitter<any>();
  @Output() closed: EventEmitter<any> = new EventEmitter<any>();


  public isOpen: boolean;
  public model: PaymentModel;
  public paymentTypes = [];
  public paymentModes = [];

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private enumService: EnumService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.paymentTypes = this.enumService.getPaymentTypes();
    this.paymentModes = this.enumService.getPaymentModes();
  }

  public onClose() {
    this.closed.emit();
  }

  public onProvidePayment() {
    if (this.invoice && this.invoice.id > 0) {
      this.model.invoiceId = this.invoice.id;
      this.apiService.post('api/Payment/ProvidePayment', this.model)
        .toPromise()
        .then(th => {
          this.paymentProvided.emit(th);
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('PAYMENT_PROVIDED'), life: 3000 });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_PROVIDE_PAYMENT'), life: 3000 });
        })
        .finally(() => { });
    }
  }

}
