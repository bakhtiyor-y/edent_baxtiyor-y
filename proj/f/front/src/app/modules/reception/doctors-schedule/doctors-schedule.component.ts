import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { AppointmentStatus } from 'src/app/core/enums';
import { AppointmentModel, DoctorScheduleModel } from 'src/app/core/models/common';
import { DoctorModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { getTimeString } from 'src/app/core/util/get-date-from-calendar';

@Component({
  selector: 'app-doctors-schedule',
  templateUrl: './doctors-schedule.component.html',
  styleUrls: ['./doctors-schedule.component.scss']
})
export class DoctorsScheduleComponent implements OnInit {

  public doctors: DoctorScheduleModel[] = [];
  public selectedDate: Date;

  public selectedDoctor: any;
  public selectedAppointmentTime: Date;
  public appointDialog: boolean;
  public editDialog: boolean;
  public selectedAppointment: AppointmentModel;
  public AppointmentStatus = AppointmentStatus
  public items = [];

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.selectedDate = new Date();
    this.loadDoctors(this.selectedDate);
  }

  public loadDoctors(selectedDate: Date) {
    const unixTime = selectedDate.getTime().valueOf();    
    this.apiService.get(`api/Doctor/GetDoctorsWithSchedule?date=${unixTime}`)
      .toPromise()
      .then(th => {
        this.doctors = th as DoctorScheduleModel[];
        let minDuration: number = 1440;
        let maxDate = new Date(selectedDate.getFullYear(), selectedDate.getMonth(), selectedDate.getDate(), 0, 0, 0);
        let minDate = new Date(selectedDate.getFullYear(), selectedDate.getMonth(), selectedDate.getDate(), 23, 59, 59);
        this.doctors.forEach(d => {
          if (d.admissionDuration < minDuration) {
            minDuration = d.admissionDuration;
          }
          d.events.forEach(e => {
            const std = new Date(e.starting);
            if (std > maxDate) {
              maxDate = new Date(std);
            }
            if (std < minDate) {
              minDate = new Date(std);
            }
          });
        });

        const ers = [];
        let nextDate = new Date(minDate);
        while (nextDate < maxDate) {
          ers.push({ time: getTimeString(nextDate), duration: minDuration });
          nextDate = new Date(nextDate.getTime() + minDuration * 60000);
        }
        this.items = ers;
       
        
      });
  }

  onDateSelect(event) {
    this.selectedDate = new Date(event);
    this.loadDoctors(this.selectedDate);
  }

  onPrevDate() {
    const newDate = new Date(this.selectedDate);
    newDate.setDate(newDate.getDate() - 1);
    this.selectedDate = newDate;

    this.loadDoctors(this.selectedDate);
  }

  onNextDate() {
    const newDate = new Date(this.selectedDate);
    newDate.setDate(newDate.getDate() + 1);
    this.selectedDate = newDate;

    this.loadDoctors(this.selectedDate);
  }

  appointPatient(doctor, event) {
    this.appointDialog = true;
    this.selectedDoctor = doctor;
    this.selectedAppointmentTime = new Date(event.starting);
  }

  editAppointment(appointmentId) {
    this.apiService.get('api/appointment/GetById?id=' + appointmentId)
      .toPromise()
      .then(th => {
        this.selectedAppointment = th;
        this.selectedAppointment.appointmentDate = new Date(th.appointmentDate);
        this.editDialog = true;
      })
      .catch(error => {
        this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_READ'), life: 3000 });
      })
      .finally(() => {
      });
  }

  onAppointmentClosed() {
    this.appointDialog = false;
    this.selectedDoctor = undefined;
    this.selectedAppointmentTime = undefined;
  }

  onAppointed() {
    this.loadDoctors(this.selectedDate);
    this.appointDialog = false;
    this.selectedDoctor = undefined;
    this.selectedAppointmentTime = undefined;
  }

  onEditClose() {
    this.editDialog = false;
    this.selectedAppointment = undefined;
  }

  onEdit(event) {
    this.loadDoctors(this.selectedDate);
    this.editDialog = false;
    this.selectedAppointment = undefined;
  }

  onAppointmentCancelled() {
    this.loadDoctors(this.selectedDate);
    this.editDialog = false;
    this.selectedAppointment = undefined;
  }

  getEventSpan(event, doctor): number {
    return doctor.admissionDuration / event.duration;
  }

  findEvent(event, doctor: DoctorScheduleModel) {
    console.log('((((((((((()))))))))))');
        
    console.log(doctor.events.find(f => getTimeString(new Date(f.starting)) === event.time));
    console.log('(((((((())))))))');

    return doctor.events.find(f => getTimeString(new Date(f.starting)) === event.time);
  }

}
