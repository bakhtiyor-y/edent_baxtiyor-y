import { EventEmitter } from '@angular/core';
import { Component, Input, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AppointmentDateEditModel, AppointmentModel, ScheduleEventModel } from 'src/app/core/models/common';
import { DentalChairModel, PartnerModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';
import { getTimeString } from 'src/app/core/util/get-date-from-calendar';

@Component({
  selector: 'app-edit-appointment',
  templateUrl: './edit-appointment.component.html',
  styleUrls: ['./edit-appointment.component.scss']
})
export class EditAppointmentComponent implements OnInit {

  public appointment: AppointmentModel;
  public scheduleEvents: ScheduleEventModel[] = [];
  public partners: PartnerModel[] = [];

  public selectedEvent: ScheduleEventModel;
  public selectedPartner: PartnerModel;
  public dentalChairs: DentalChairModel[] = [];
  public selectedChair: DentalChairModel;

  @Input() public set model(item: AppointmentModel) {
    if (item) {
      this.appointment = item;
      this.onAppointmentDateChange(item.appointmentDate);
    }
  }
  @Input() public isOpen: boolean;
  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();
  @Output() public appointmentCancelled: EventEmitter<any> = new EventEmitter<any>();


  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService,
    private confirmationService: ConfirmationService) { }

  ngOnInit(): void {
    this.apiService.get('api/Partner/GetAll')
      .toPromise()
      .then(th => {
        this.partners = th;
      })
      .catch(error => { })
      .finally(() => { });
  }

  public save() {
    if (!this.selectedChair) {
      this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('DENTAL_CHAIR_NOT_SELECTED'), life: 3000 });
      return;
    }
    const appointmentTime = new Date(this.selectedEvent.starting);
    const editModel = { id: this.appointment.id, partnerId: this.selectedPartner?.id } as AppointmentDateEditModel;
    editModel.appointmentDate = new Date(this.appointment.appointmentDate.getFullYear(),
      this.appointment.appointmentDate.getMonth(),
      this.appointment.appointmentDate.getDate(),
      appointmentTime.getHours(),
      appointmentTime.getMinutes());
    editModel.dentalChairId = this.selectedChair.id;

    this.apiService.put('api/Appointment/EditAppointByReception', editModel)
      .toPromise().then(th => {
        this.saved.emit(th);
        this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
      })
      .catch(error => {
        if (error.error.message) {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: error.error.message, life: 3000 });
        } else {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        }
      })
      .finally(() => { });
  }

  public close() {
    this.closed.emit();
  }

  public onAppointmentDateChange(event) {
    if (this.appointment) {
      this.scheduleEvents = [];
      const unixTime = new Date(event).getTime();
      this.apiService.get(`api/Schedule/GetDoctorSchedule?doctorId=${this.appointment.doctorId}&date=${unixTime}`)
        .toPromise()
        .then(th => {
          const events = th as ScheduleEventModel[];
          events.forEach(item => {
            item.startingText = `${getTimeString(new Date(item.starting))} - ${item.name}`;
            this.scheduleEvents.push(item);
          });
        })
        .catch(error => { })
        .finally(() => { });
    }
  }

  onAppointmentTimeChange(e: ScheduleEventModel) {
    const unixTime = new Date(e.starting).getTime();
    this.apiService.get(`api/DentalChair/GetDoctorDentalChairsByDate?doctorId=${this.appointment.doctorId}&date=${unixTime}`)
      .toPromise()
      .then(th => {
        this.dentalChairs = th as DentalChairModel[];
      }).catch(error => { })
      .finally(() => { });
  }

  public cancelAppointment() {
    this.confirmationService.confirm({
      message: 'Вы действительно хотите отменить запись на прием №' + this.appointment.id + '?',
      header: this.translate.instant('CANCEL_APPOINTMENT'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.get('api/Appointment/CancelByReception?id=' + this.appointment.id)
          .toPromise()
          .then(th => {
            this.appointmentCancelled.emit();
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }
}
