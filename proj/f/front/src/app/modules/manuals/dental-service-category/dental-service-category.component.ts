import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DentalServiceCategoryModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-dental-service-category',
  templateUrl: './dental-service-category.component.html',
  styleUrls: ['./dental-service-category.component.scss']
})
export class DentalServiceCategoryComponent implements OnInit {

  public items: DentalServiceCategoryModel[] = [];

  public selectedItems: DentalServiceCategoryModel[];

  public loading: boolean;

  public clonedItems: { [s: string]: DentalServiceCategoryModel; } = {};


  public totalRecords: number;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private translate: TranslateService) { }

  ngOnInit(): void {
  }

  add() {
    return { id: 0 } as DentalServiceCategoryModel;
  }

  edit(item: DentalServiceCategoryModel) {
    this.clonedItems[item.id] = { ...item };
  }

  delete(item: DentalServiceCategoryModel) {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRY'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.delete('api/DentalServiceCategory?id=' + item.id)
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
        this.apiService.post('api/DentalServiceCategory/DeleteSelected', this.selectedItems)
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

  onEdit(item: DentalServiceCategoryModel) {
    if (!item) {
      return;
    }

    if (item.id > 0) {
      delete this.clonedItems[item.id];
      this.apiService.put('api/DentalServiceCategory', item).toPromise()
        .then(th => {
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
        })
        .catch(error => { })
        .finally(() => { });
    } else {
      this.apiService.post('api/DentalServiceCategory', item).toPromise()
        .then(th => {
          this.items[this.findIndexById(0)] = th;
          this.items = [...this.items];
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_CREATED'), life: 3000 });
        })
        .catch(error => { })
        .finally(() => { });
    }
  }

  onEditCancel(item: DentalServiceCategoryModel, index: number) {
    if (item.id === 0) {
      this.items.splice(index, 1);
    } else {
      this.items[index] = this.clonedItems[item.id];
      delete this.clonedItems[item.id];
    }
  }

  public loadData(event) {

    const params = new HttpParams().set('filter', JSON.stringify(event));
    this.apiService.get('api/DentalServiceCategory', params).toPromise().then(th => {
      this.items = [];
      th.data.forEach(dataItem => {
        this.items.push(dataItem);
      });
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
