import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { InventoryOutcomeModel, ReceptModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-inventory-outcome',
  templateUrl: './inventory-outcome.component.html',
  styleUrls: ['./inventory-outcome.component.scss']
})
export class InventoryOutcomeComponent implements OnInit {

  public addDialog: boolean;
  public viewDialog: boolean;
  public viewReceptDialog: boolean;
  public lastTableLazyLoadEvent;
  public receptItem: ReceptModel;


  public items: InventoryOutcomeModel[];

  public editItem: InventoryOutcomeModel;
  public viewItem: InventoryOutcomeModel;

  public loading: boolean;

  public totalRecords: number;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngOnInit(): void {
  }

  add() {
    this.editItem = { id: 0, createdDate: new Date() } as InventoryOutcomeModel;
    this.addDialog = true;
  }

  onAddClosed() {
    this.editItem = null;
    this.addDialog = false;
  }

  onSaved(item) {
    // this.items.push(item);
    this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('OUTCOME_ADDED'), life: 3000 });
    // this.items = [...this.items];
    this.editItem = null;
    this.addDialog = false;
    this.loadData(this.lastTableLazyLoadEvent);
  }

  public loadData(event) {
    this.lastTableLazyLoadEvent = event;
    const params = new HttpParams().set('filter', JSON.stringify(event));
    this.apiService.get('api/InventoryOutcome', params).toPromise().then(th => {
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

  public viewDetails(item: InventoryOutcomeModel) {
    this.viewDialog = true;
    this.viewItem = item;
  }

  public onViewClose() {
    this.viewDialog = false;
    this.viewItem = null;
  }

  openRecept(receptId: number) {
    this.apiService.get('api/Recept/GetById/' + receptId)
      .toPromise()
      .then(data => {
        this.viewReceptDialog = true;
        this.receptItem = data;
      })
      .catch(error => { })
      .finally(() => { });
  }

  closeRecept() {
    this.viewReceptDialog = false;
    this.receptItem = null;
  }
}
