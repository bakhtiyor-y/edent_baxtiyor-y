import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { InvoiceModel, PaymentModel, ReceptModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-invoice',
  templateUrl: './invoice.component.html',
  styleUrls: ['./invoice.component.scss']
})
export class InvoiceComponent implements OnInit {

  public viewReceptDialog: boolean;
  public provideDiscountDialog: boolean;
  public providePaymentDialog: boolean;
  public paymentDialog: boolean;
  public isSearchClear = false;
  public lastTableLazyLoadEvent;


  public items: InvoiceModel[];
  public editItem: InvoiceModel;

  public invoicePayments: PaymentModel[] = [];

  public currentRecept: ReceptModel;

  public loading: boolean;
  public totalRecords: number;


  constructor(private apiService: ApiService, private messageService: MessageService,
    private confirmationService: ConfirmationService) {

  }

  ngOnInit(): void {
    this.loading = false;
  }

  public loadItems(event, name = '') {
    this.lastTableLazyLoadEvent = event;
    let params = new HttpParams()
      .set('filter', JSON.stringify(event));

    if (name) {
      params = new HttpParams()
        .set('filter', JSON.stringify(event))
        .set('name', name);
    }

    this.apiService.get('api/Invoice', params).toPromise().then(th => {
      this.items = th.data;
      this.totalRecords = th.total;
    }).catch(error => {
    }).finally(() => {
      this.loading = false;
    });
  }

  public onPageChange(event) {
    this.loading = true;
  }

  public viewDetails(item: InvoiceModel) {
    this.apiService.get('api/Recept/GetById/' + item.receptId)
      .toPromise()
      .then(th => {
        this.viewReceptDialog = true;
        this.currentRecept = th;
      })
      .catch(error => {

      })
      .finally(() => { });
  }

  public closeRecept() {
    this.viewReceptDialog = false;
    this.currentRecept = null;
  }



  public provideDiscount(item: InvoiceModel) {
    this.editItem = item;
    this.provideDiscountDialog = true;
  }

  public onDiscountProvided(item: InvoiceModel) {
    this.items[this.findIndexById(item.id)] = item;
    this.items = [...this.items];
    this.provideDiscountDialog = false;
    this.editItem = null;
  }

  public onDiscountClosed() {
    this.provideDiscountDialog = false;
    this.editItem = null;
  }

  public createPayment(item: InvoiceModel) {
    this.editItem = item;
    this.providePaymentDialog = true;
  }

  public onPaymentProvided(item: InvoiceModel) {
    this.items[this.findIndexById(item.id)] = item;
    this.items = [...this.items];
    this.providePaymentDialog = false;
    this.editItem = null;
  }

  public onPaymentClosed() {
    this.providePaymentDialog = false;
    this.editItem = null;
  }

  public viewPayments(item: InvoiceModel) {
    this.apiService.get('api/Payment/GetInvoicePayments?invoiceId=' + item.id)
      .toPromise()
      .then(th => {
        this.paymentDialog = true;
        this.invoicePayments = th;
      })
      .catch(errpr => { })
      .finally(() => { });
  }

  public onClosePayments() {
    this.paymentDialog = false;
    this.invoicePayments = [];
  }

  findIndexById(id: number): number {
    let index = -1;
    for (let i = 0; i < this.items.length; i++) {
      if (this.items[i].id === id) {
        index = i;
        break;
      }
    }
    return index;
  }

  onSearch(value) {
    if (value && value.length > 3) {
      this.isSearchClear = true;
      this.loadItems(this.lastTableLazyLoadEvent, value);

    } else {
      if (this.isSearchClear) {
        this.isSearchClear = false;
        this.loadItems(this.lastTableLazyLoadEvent, '');
      }
    }
  }
}
