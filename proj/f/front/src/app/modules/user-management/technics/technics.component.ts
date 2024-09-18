import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { TechnicModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-technics',
  templateUrl: './technics.component.html',
  styleUrls: ['./technics.component.scss']
})
export class TechnicsComponent implements OnInit {

  public editDialog: boolean;

  public items: TechnicModel[] = [];

  public selectedItems: TechnicModel[];
  public item: TechnicModel;

  public loading: boolean;
  public totalRecords: number;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private translate: TranslateService) {

  }

  ngOnInit(): void {
    this.loading = false;
  }

  add() {
    this.item = { id: 0 } as TechnicModel;
    this.item.birthDate = new Date();
    this.editDialog = true;
  }

  deleteSelectedItems() {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRIES'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.post('api/Technic/DeleteSelected', this.selectedItems)
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

  edit(technic: TechnicModel) {
    this.apiService.get('api/Technic/GetById?id=' + technic.id)
      .toPromise()
      .then(th => {
        this.item = th;
        this.item.birthDate = new Date(th.birthDate);
        this.editDialog = true;
      }).catch(error => {
      }).finally(() => {
      });
  }

  delete(technic: TechnicModel) {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRY'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.delete('api/Technic?id=' + technic.id)
          .toPromise()
          .then(th => {
            this.items = this.items.filter(val => val.id !== technic.id);
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRY'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  onClosed() {
    this.item = null;
    this.editDialog = false;
  }

  onSaved(result) {
    if (result.isNew) {
      this.items.push(result.item);
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_CREATED'), life: 3000 });
    } else {
      this.items[this.findIndexById(result.item.id)] = result.item;
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
    }
    this.items = [...this.items];
    this.editDialog = false;
  }



  public loadItems(event) {
    const params = new HttpParams().set('filter', JSON.stringify(event));
    this.apiService.get('api/Technic', params).toPromise().then(th => {
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
