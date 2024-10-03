import { HttpParams } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { AppointmentStatus } from 'src/app/core/enums';
import { AppointmentModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';

@Component({
  selector: 'app-patient-appointments',
  templateUrl: './patient-appointments.component.html',
  styleUrls: ['./patient-appointments.component.scss']
})
export class PatientAppointmentsComponent implements OnInit {
  
  
  
  
  public items: AppointmentModel[];

  public selectedItems: AppointmentModel[];

  public loading: boolean;
  public totalRecords: number;
  public editDialog: boolean;
  public editItem: AppointmentModel;
  public lastTableLazyLoadEvent;
  public isSearchClear = false;
  public searchValue = '';
  public AppointmentStatus = AppointmentStatus

  public appointmentStatuses = [
    { text: this.translate.instant('ALL'), value: 99 },
    { text: this.translate.instant('APPOINTED'), value: AppointmentStatus.Appointed },
    { text: this.translate.instant('POSTPONED'), value: AppointmentStatus.Postponed },
    { text: this.translate.instant('CANCELLED'), value: AppointmentStatus.Cancelled },
    { text: this.translate.instant('CARRIED_OUT'), value: AppointmentStatus.CarriedOut }];

  public selectedStatus: any = 99;

  @ViewChild('dtAppointments') appointmentsDt;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private router: Router,
    private enumService: EnumService,
    private translate: TranslateService) {

  }

  ngOnInit(): void {
    this.loading = false;
  }

  add() {
    this.router.navigateByUrl('/dashboard/reception/new-appointment/0');
  }

  edit(item: AppointmentModel) {
    this.editDialog = true;
    this.editItem = item;
    this.editItem.appointmentDate = new Date(item.appointmentDate);
  }

  date1: string = new Date("09/25/2024").toISOString();
  date2: string = new Date("09/26/2024").toISOString();
  // .set('fromDate', this.date1).set('toDate', this.date2);
  public loadItems(event, name = null, status = 99) {
    this.lastTableLazyLoadEvent = event;
    let params = new HttpParams()
      .set('filter', JSON.stringify(event)).set('fromDate', this.date1).set('toDate', this.date2);
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

    this.apiService.get('api/Appointment/GetReceptionAppointments', params).toPromise().then(th => {
      this.items = th.data;
      this.totalRecords = th.total;
      console.log( 'data ', th);
      
      
    }).catch(error => {
    }).finally(() => {
      this.loading = false;
    });
  }
  public onPageChange(event) {
    this.loading = true;
  }

  public onEdit(item: AppointmentModel) {
    if (item) {
      this.items[this.findIndexById(item.id)] = item;
    }
    this.items = [...this.items];
    this.editDialog = false;
    this.editItem = null;
  }

  public onEditClose() {
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
