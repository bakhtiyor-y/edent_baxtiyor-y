import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { AppointmentModel } from 'src/app/core/models/common';
import { UserModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { getTimeString } from 'src/app/core/util/get-date-from-calendar';

@Component({
  selector: 'app-rentgen-desk',
  templateUrl: './rentgen-desk.component.html',
  styleUrls: ['./rentgen-desk.component.scss']
})
export class RentgenDeskComponent implements OnInit {

  public employees: UserModel[] = [];
  public selectedDate: Date = new Date();

  public selectedEmployee: any;
  public selectedAppointmentTime: Date;
  public appointDialog: boolean;
  public editDialog: boolean;
  //public selectedAppointment: AppointmentModel;
  public appointments: AppointmentModel[] = [];
  public items = [];

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.selectedDate = new Date();
    this.loadEmployees();
    this.loadAppointments(this.selectedDate);
  }

  getDateItems(selectedDate: Date) {
    const ers = [];
    const year = selectedDate.getFullYear();
    const month = selectedDate.getMonth();
    const day = selectedDate.getDate();
    const maxDate = new Date(year, month, day, 21, 0, 0, 0);
    let nextDate = new Date(year, month, day, 8, 0, 0, 0);
    while (nextDate < maxDate) {
      ers.push({ time: getTimeString(nextDate), duration: 20, date: nextDate });
      nextDate = new Date(nextDate.getTime() + 20 * 60000);
    }
    return ers;
  }

  loadAppointments(selectedDate: Date) {
    const unixTime = selectedDate.getTime().valueOf();
    this.apiService.get(`api/Appointment/GetRentgenAppointmentsByDate?date=${unixTime}`)
      .toPromise()
      .then(th => {
        this.appointments = th as AppointmentModel[];
        this.appointments.forEach(appointment => {
          appointment.appointmentDate = new Date(appointment.appointmentDate);
        });
        this.items = this.getDateItems(this.selectedDate);
      });
  }

  loadEmployees() {
    this.apiService.get(`api/User/GetEmployees`)
      .toPromise()
      .then(th => {
        this.employees = th as UserModel[];
      });
  }


  onDateSelect(event) {
    this.selectedDate = new Date(event);
    this.getDateItems(this.selectedDate);
    this.loadAppointments(this.selectedDate);
  }

  onPrevDate() {
    const newDate = new Date(this.selectedDate);
    newDate.setDate(newDate.getDate() - 1);
    this.selectedDate = newDate;
    this.getDateItems(this.selectedDate);
    this.loadAppointments(this.selectedDate);
  }

  onNextDate() {
    const newDate = new Date(this.selectedDate);
    newDate.setDate(newDate.getDate() + 1);
    this.selectedDate = newDate;
    this.getDateItems(this.selectedDate);
    this.loadAppointments(this.selectedDate);
  }

  findEvent(event, emp: UserModel) {
    const app = this.appointments.find(f => f.employeeId === emp.employeeId
      && f.appointmentDate.getFullYear() === event.date.getFullYear()
      && f.appointmentDate.getMonth() === event.date.getMonth()
      && f.appointmentDate.getDate() === event.date.getDate()
      && f.appointmentDate.getHours() === event.date.getHours()
      && f.appointmentDate.getMinutes() === event.date.getMinutes());
    return app;
  }

  appointPatient(employee, event) {
    this.appointDialog = true;
    this.selectedEmployee = employee;
    this.selectedAppointmentTime = new Date(event.date);
  }

  editAppointment(appointmentId) {
  }

  onAppointmentClosed() {
    this.appointDialog = false;
    this.selectedEmployee = undefined;
    this.selectedAppointmentTime = undefined;
  }

  onAppointed() {
    this.getDateItems(this.selectedDate);
    this.loadAppointments(this.selectedDate);
    this.appointDialog = false;
    this.selectedEmployee = undefined;
    this.selectedAppointmentTime = undefined;
  }

}
