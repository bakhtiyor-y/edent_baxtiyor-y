import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PaymentModel } from 'src/app/core/models/common';
import { EnumService } from 'src/app/core/services/enum.service';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.scss']
})
export class PaymentComponent implements OnInit {

  @Input() public isOpen;
  @Input() public payments: PaymentModel[] = [];
  @Output() closed: EventEmitter<any> = new EventEmitter<any>();

  constructor(private enumService: EnumService) { }

  ngOnInit(): void {
  }


  public onClose() {
    this.closed.emit();
    this.payments = [];
  }

  public getPaymentTypeText(type) {
    return this.enumService.getPaymentTypeText(type);
  }

  public getPaymentModeText(mode) {
    return this.enumService.getPaymentModeText(mode);
  }

}
