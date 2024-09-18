import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ReceptInventorySettingModel } from 'src/app/core/models/appsettings';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-recept-inventory-setting',
  templateUrl: './recept-inventory-setting.component.html',
  styleUrls: ['./recept-inventory-setting.component.scss']
})
export class ReceptInventorySettingComponent implements OnInit {

  public editDialog: boolean;

  public items: ReceptInventorySettingModel[];

  public selectedItems: ReceptInventorySettingModel[];

  public editItem: ReceptInventorySettingModel;

  public loading: boolean;

  public totalRecords: number;

  constructor(private apiService: ApiService, private messageService: MessageService,
    private confirmationService: ConfirmationService, private translate: TranslateService) { }

  ngOnInit(): void {
  }

  add() {
    this.editItem = { id: 0 } as ReceptInventorySettingModel;
    this.editDialog = true;
  }

  edit(item: ReceptInventorySettingModel) {

    this.editItem = item;
    this.editDialog = true;
  }

  delete(item: ReceptInventorySettingModel) {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRY'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.delete('api/ReceptInventorySetting?id=' + item.id)
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
        this.apiService.post('api/ReceptInventorySetting/DeleteSelected', this.selectedItems)
          .toPromise()
          .then(th => {
            this.items = this.items.filter(val => !this.selectedItems.includes(val));
            this.selectedItems = null;
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRIES_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRY'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  onClosed() {
    this.editItem = null;
    this.editDialog = false;
  }

  onSaved(result) {
    if (result.isNew) {
      this.items.push(result.item);
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_CREATED'), life: 3000 });
    } else {
      this.items[this.findIndexById(this.editItem.id)] = result.item;
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
    }
    this.items = [...this.items];
    this.editItem = null;
    this.editDialog = false;
  }

  public loadData(event) {
    const params = new HttpParams().set('filter', JSON.stringify(event));
    this.apiService.get('api/ReceptInventorySetting', params).toPromise().then(th => {
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
