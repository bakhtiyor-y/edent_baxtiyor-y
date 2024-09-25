import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { PatientManageModel, PatientModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-patients',
  templateUrl: './patients.component.html',
  styleUrls: ['./patients.component.scss']
})
export class PatientsComponent implements OnInit {

  public editDialog: boolean;
  public viewDialog: boolean;

  public items: PatientModel[];

  public selectedItems: PatientModel[];
  public editItem: PatientManageModel;

  public loading: boolean;
  public totalRecords: number;
  public isSearchClear = false;
  public lastTableLazyLoadEvent;

  public patientViewModel: any;


  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService) {

  }

  ngOnInit(): void {
    this.loading = false;
  }

  add() {
    this.editItem = { id: 0 } as PatientManageModel;
    this.editItem.birthDate = new Date();
    this.editDialog = true;
  }

  edit(item: PatientModel) {
    this.apiService.get('api/Patient/GetById?id=' + item.id)
      .toPromise()
      .then(th => {
        this.editItem = th;
        this.editItem.birthDate = new Date(th.birthDate);
        this.editDialog = true;
      }).catch(error => {
      }).finally(() => {
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
      this.items[this.findIndexById(result.item.id)] = result.item;
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
    }
    this.items = [...this.items];
    this.editDialog = false;
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

    this.apiService.get('api/Patient', params).toPromise().then(th => {
      console.log(`loadItems(event, name = '')`, th);
      
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

  public viewHistory(item: PatientModel) {
    console.log(' viewHistory(item: PatientModel) ', item);
    
    const params = new HttpParams()
      .set('patientId', `${item.id}`);
    this.apiService.get('api/Recept/GetPatientRecepts', params)
      .toPromise()
      .then(data => {
        this.patientViewModel = { patient: item, recepts: data };
        this.viewDialog = true;
      })
      .catch(error => { })
      .finally(() => { });
  }

  public onViewClosed() {
    this.viewDialog = false;
    this.patientViewModel = null;
  }

}
