import { HttpParams } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { AppointmentStatus } from 'src/app/core/enums';
import { AppointmentModel } from 'src/app/core/models/common';
import { ApiService, EnumService } from 'src/app/core/services';

@Component({
  selector: 'app-rentgen-recepts',
  templateUrl: './rentgen-recepts.component.html',
  styleUrls: ['./rentgen-recepts.component.scss']
})
export class RentgenReceptsComponent implements OnInit {

  public items: AppointmentModel[];

  public selectedItems: AppointmentModel[];

  public loading: boolean;
  public totalRecords: number;
  public editDialog: boolean;
  public editItem: AppointmentModel;
  public lastTableLazyLoadEvent;
  public isSearchClear = false;
  public searchValue = '';
  AppointmentStatus = AppointmentStatus;

  public appointmentStatuses = [
    { text: this.translate.instant('APPOINTED'), value: AppointmentStatus.Appointed },
    { text: this.translate.instant('CARRIED_OUT'), value: AppointmentStatus.CarriedOut },
    { text: this.translate.instant('ALL'), value: 99 },
  ];

  public selectedStatus: any = 0;

  @ViewChild('dtAppointments') appointmentsDt;

  constructor(private apiService: ApiService,
    private enumService: EnumService,
    private translate: TranslateService) {

  }

  ngOnInit(): void {
    this.loading = false;
  }

  recept(item: AppointmentModel) {
    this.editDialog = true;
    this.editItem = item;
    this.editItem.appointmentDate = new Date(item.appointmentDate);
  }

  loadItems(event, name = null, status = 99) {
    this.lastTableLazyLoadEvent = event;
    let params = new HttpParams()
      .set('filter', JSON.stringify(event));
    if (name) {
      params = new HttpParams()
        .set('filter', JSON.stringify(event))
        .set('name', name);
    }
    if (status !== 99) {
      params = new HttpParams()
        .set('filter', JSON.stringify(event))
        .set('name', name)
        .set('appointmentStatus', `${status}`);
    }

    this.apiService.get('api/Appointment/GetRentgenAppointments', params).toPromise().then(th => {
      this.items = th.data;
      this.totalRecords = th.total;
    }).catch(error => {
    }).finally(() => {
      this.loading = false;
    });
  }
  onPageChange(event) {
    this.loading = true;
  }

  onEdit(item: AppointmentModel) {
    this.loadItems(this.lastTableLazyLoadEvent, this.searchValue, this.selectedStatus);
    this.editDialog = false;
    this.editItem = null;
  }

  onEditClose() {
    this.editDialog = false;
    this.editItem = null;
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

  getAppointmentStatusText(status: AppointmentStatus) {
    return this.enumService.getAppointmentStatusText(status);
  }

  onAppointmentCancelled() {
    this.editDialog = false;
    this.editItem = null;
    this.loadItems(this.lastTableLazyLoadEvent);
  }

  onStatusChoosed(event) {
    this.loadItems(this.lastTableLazyLoadEvent, this.searchValue, this.selectedStatus);
  }

  onSearch(value) {
    if (value && value.length > 3) {
      this.isSearchClear = true;
      this.searchValue = value;
      this.loadItems(this.lastTableLazyLoadEvent, value, this.selectedStatus);

    } else {
      if (this.isSearchClear) {
        this.searchValue = '';
        this.isSearchClear = false;
        this.loadItems(this.lastTableLazyLoadEvent, '', this.selectedStatus);
      }
    }
  }
}
