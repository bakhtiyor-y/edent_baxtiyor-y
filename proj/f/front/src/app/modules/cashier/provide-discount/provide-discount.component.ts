import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { DiscountModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';

@Component({
  selector: 'app-provide-discount',
  templateUrl: './provide-discount.component.html',
  styleUrls: ['./provide-discount.component.scss']
})
export class ProvideDiscountComponent implements OnInit {

  @Input() public set openDialog(isOpen: boolean) {
    this.isOpen = isOpen;
    if (isOpen) {
      this.model = {} as DiscountModel;
    } else {
      this.model = null;
    }
  }
  @Input() public invoiceId: number;

  @Output() discountProvided: EventEmitter<any> = new EventEmitter<any>();
  @Output() closed: EventEmitter<any> = new EventEmitter<any>();


  public isOpen: boolean;
  public model: DiscountModel;
  public discountTypes = [];

  constructor(private apiService: ApiService, private enumService: EnumService,
    private messageService: MessageService, private translate: TranslateService) { }

  ngOnInit(): void {
    this.discountTypes = this.enumService.getDiscountTypes();
  }

  public onClose() {
    this.closed.emit();
  }

  public onProvideDiscount() {
    if (this.invoiceId) {
      this.model.invoiceId = this.invoiceId;
      this.apiService.post('api/Invoice/ProvideDiscount', this.model)
        .toPromise()
        .then(th => {
          this.discountProvided.emit(th);
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('DISCOUNT_PROVIDED'), life: 3000 });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_PROVIDE_DISCOUNT'), life: 3000 });
        })
        .finally(() => { });
    }
  }
}
