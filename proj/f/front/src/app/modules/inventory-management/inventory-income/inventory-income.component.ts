import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { InventoryIncomeModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-inventory-income',
  templateUrl: './inventory-income.component.html',
  styleUrls: ['./inventory-income.component.scss']
})
export class InventoryIncomeComponent implements OnInit {

  public addDialog: boolean;
  public viewDialog: boolean;


  public items: InventoryIncomeModel[];

  public editItem: InventoryIncomeModel;
  public viewItem: InventoryIncomeModel;

  public loading: boolean;

  public totalRecords: number;
  public lastTableLazyLoadEvent;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngOnInit(): void {
  }

  add() {
    this.editItem = { id: 0, createdDate: new Date() } as InventoryIncomeModel;
    this.addDialog = true;
  }

  onAddClosed() {
    this.editItem = null;
    this.addDialog = false;
  }

  onSaved(item) {
    // this.items.push(item);
    this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('INCOME_ADDED'), life: 3000 });
    // this.items = [...this.items];
    this.editItem = null;
    this.addDialog = false;
    this.loadData(this.lastTableLazyLoadEvent);
  }

  public loadData(event) {
    this.lastTableLazyLoadEvent = event;
    const params = new HttpParams().set('filter', JSON.stringify(event));
    this.apiService.get('api/InventoryIncome', params).toPromise().then(th => {
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

  public viewDetails(item: InventoryIncomeModel) {
    this.viewDialog = true;
    this.viewItem = item;
  }

  public onViewClose() {
    this.viewDialog = false;
    this.viewItem = null;
  }

}
