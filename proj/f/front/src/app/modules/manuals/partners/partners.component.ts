import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ProfitType } from 'src/app/core/enums';
import { PartnerModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';

@Component({
  selector: 'app-partners',
  templateUrl: './partners.component.html',
  styleUrls: ['./partners.component.scss']
})
export class PartnersComponent implements OnInit {


  public items: PartnerModel[];

  public clonedItems: { [s: string]: PartnerModel; } = {};

  public selectedItems: PartnerModel[];

  public loading: boolean;

  public totalRecords: number;

  public profitTypes: any[] = [];

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private enumService: EnumService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.profitTypes = this.enumService.getProfitTypes();
  }

  public getTypeText(type: ProfitType) {
    return this.enumService.getProfitTypeText(type);
  }

  add() {
    return { id: 0 } as PartnerModel;
  }

  edit(item: PartnerModel) {
    this.clonedItems[item.id] = { ...item };
  }

  onEdit(item: PartnerModel) {
    if (!item) {
      return;
    }

    if (item.id > 0) {
      delete this.clonedItems[item.id];
      this.apiService.put('api/Partner', item).toPromise()
        .then(th => {
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.post('api/Partner', item).toPromise()
        .then(th => {
          this.items[this.findIndexById(0)] = th;
          this.items = [...this.items];
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_CREATED'), life: 3000 });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    }
  }

  onEditCancel(item: PartnerModel, index: number) {
    if (item.id === 0) {
      this.items.splice(index, 1);
    } else {
      this.items[index] = this.clonedItems[item.id];
      delete this.clonedItems[item.id];
    }
  }

  delete(item: PartnerModel) {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRY'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.delete('api/Partner?id=' + item.id)
          .toPromise()
          .then(th => {
            this.items = this.items.filter(val => val.id !== item.id);
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRY'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  deleteSelected() {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRIES'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.post('api/Partner/DeleteSelected', this.selectedItems)
          .toPromise()
          .then(th => {
            this.items = this.items.filter(val => !this.selectedItems.includes(val));
            this.selectedItems = null;
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRIES_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRIES'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  onClosed() {

  }

  public loadData(event) {
    const params = new HttpParams().set('filter', JSON.stringify(event));
    this.apiService.get('api/Partner', params).toPromise().then(th => {
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

}
