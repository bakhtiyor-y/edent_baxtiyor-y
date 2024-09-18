import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DentalServiceType } from 'src/app/core/enums';
import { DentalServiceModel } from 'src/app/core/models/manuals';
import { ApiService, EnumService } from 'src/app/core/services';

@Component({
  selector: 'app-dental-service',
  templateUrl: './dental-service.component.html',
  styleUrls: ['./dental-service.component.scss']
})
export class DentalServiceComponent implements OnInit {

  public editDialog: boolean;

  public editPriceDialog: boolean;

  public items: DentalServiceModel[];

  public selectedItems: DentalServiceModel[];

  public editItem: DentalServiceModel;

  public loading: boolean;

  public totalRecords: number;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private translate: TranslateService,
    private enumService: EnumService) {
  }

  ngOnInit(): void {

  }

  add() {
    this.editItem = { id: 0 } as DentalServiceModel;
    this.editDialog = true;
  }

  edit(item: DentalServiceModel) {
    this.editItem = item;
    this.editDialog = true;
  }

  editPrice(item: DentalServiceModel) {
    this.editItem = item;
    this.editPriceDialog = true;
  }

  delete(item: DentalServiceModel) {

    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRY'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.delete('api/DentalService?id=' + item.id)
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
        this.apiService.post('api/DentalService/DeleteSelected', this.selectedItems)
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
    this.editItem = null;
    this.editDialog = false;
  }

  onPriceClosed() {
    this.editItem = null;
    this.editPriceDialog = false;
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
    this.apiService.get('api/DentalService', params).toPromise().then(th => {
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

  getDentalServiceTypeText(e: DentalServiceType): string {
    return this.enumService.getDentalServiceTypeText(e);
  }

}
