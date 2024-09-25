import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ApiService } from 'src/app/core/services';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import { Router } from '@angular/router';
import ruLocale from '@fullcalendar/core/locales/ru';
import { AfterViewInit } from '@angular/core';
import { AppointmentStatus } from 'src/app/core/enums';
import { MessageService } from 'primeng/api';
import { TranslateService } from '@ngx-translate/core';
import { AppointmentModel, ReceptModel } from 'src/app/core/models/common';

@Component({
  selector: 'app-appointment',
  templateUrl: './appointment.component.html',
  styleUrls: ['./appointment.component.scss']
})
export class AppointmentComponent implements OnInit, AfterViewInit, OnDestroy {

  events: any[];
  options: any;

  selectedDate: Date = new Date();
  appointDate: Date = new Date();

  public selectedAppointmentId: number;
  public selectedAppointment: AppointmentModel;
  public receptModel: ReceptModel;

  viewMode = 'month';

  @ViewChild('fullCalendar') fullCalendar: any;

  public appointDialog: boolean;
  public editAppointDialog: boolean;
  public viewReceptDialog: boolean;

  public currentDate: Date = new Date();


  constructor(private apiService: ApiService,
    private router: Router,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngAfterViewInit(): void {
    this.bindEvents();
  }

  ngOnInit(): void {
    this.loadEvents();    
    this.options = {
      height: '100%',
      minTime: "07:00",
      maxTime: "21:00",
      allDaySlot: false,
      locale: ruLocale,
      plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
      defaultDate: this.selectedDate,
      header: {
        left: 'prev,next',
        center: 'title',
        right: 'dayGridMonth,timeGridWeek,timeGridDay'
      },
      editable: false,
      eventClick: (e) => {
        const event = this.events.find(f => f.id === Number(e.event.id));
        if (event) {
          this.selectedAppointmentId = event.id;
        } else {
          this.selectedAppointmentId = 0;
          this.selectedAppointment = undefined;
        }

        if (event.appointmentStatus === AppointmentStatus.Appointed
          || event.appointmentStatus === AppointmentStatus.Postponed) {
          if (event.isJoint) {
            this.messageService.add({ severity: 'info', summary: this.translate.instant('INFORMATION'), detail: this.translate.instant('JOINT_APPOINTMENT_INFO'), life: 3000 });
          } else {
            this.editAppointment();
          }
        } else {
          this.viewRecept();
        }
      },
      dateClick: (e) => {
        this.appointDate = new Date(e.date);
        this.appointDialog = true;
      },
      eventRender: (event) => {
        const e = this.events.find(f => f.id === Number(event.event.id));
        if (!e) {
          return;
        }

        if (e.appointmentStatus === AppointmentStatus.CarriedOut) {
          event.el.setAttribute('style', 'background-color:green');
        } else if (e.isJoint) {
          event.el.setAttribute('style', 'background-color:orange');
        }
      }
    };

  }

  public datesSet(e) {

  }

  public loadEvents(date: Date = null) {

    if (!date) {
      date = this.selectedDate;
    }
  
    const unixTime = new Date(date).getTime();

    this.apiService.get(`api/Appointment/GetDoctorAppointments?date=${unixTime}`)
      .toPromise()
      .then(th => {
        this.events = th;
      })
      .catch(error => { })
      .finally(() => { });
  }

  bindEvents() {
    const prevButton = this.fullCalendar.el.nativeElement.getElementsByClassName('fc-prev-button');
    const nextButton = this.fullCalendar.el.nativeElement.getElementsByClassName('fc-next-button');
    const monthButton = this.fullCalendar.el.nativeElement.getElementsByClassName('fc-dayGridMonth-button');
    const weekButton = this.fullCalendar.el.nativeElement.getElementsByClassName('fc-timeGridWeek-button');
    const dayButton = this.fullCalendar.el.nativeElement.getElementsByClassName('fc-timeGridDay-button');

    nextButton[0].addEventListener('click', () => {
      if (this.viewMode === 'month') {
        const newDate = new Date(this.currentDate);
        newDate.setMonth(newDate.getMonth() + 1);
        
        this.currentDate = newDate;
        this.loadEvents(this.currentDate);
        
      }
      else if (this.viewMode === 'week') {
        const currentMonth = this.currentDate.getMonth();

        const newDate = new Date(this.currentDate);
        newDate.setDate(newDate.getDate() + 7);
        this.currentDate = newDate;

        if (this.currentDate.getMonth() !== currentMonth) {
          this.loadEvents(this.currentDate);
        }
      }
      else {
        const currentMonth = this.currentDate.getMonth();

        const newDate = new Date(this.currentDate);
        newDate.setDate(newDate.getDate() + 1);
        this.currentDate = newDate;

        if (this.currentDate.getMonth() !== currentMonth) {
          this.loadEvents(this.currentDate);
        }
      }
    });

    prevButton[0].addEventListener('click', () => {
      if (this.viewMode === 'month') {

        const newDate = new Date(this.currentDate);
        newDate.setMonth(newDate.getMonth() - 1);
        this.currentDate = newDate;

        this.loadEvents(this.currentDate);
      }
      else if (this.viewMode === 'week') {
        const currentMonth = this.currentDate.getMonth();

        const newDate = new Date(this.currentDate);
        newDate.setDate(newDate.getDate() - 7);
        this.currentDate = newDate;

        if (this.currentDate.getMonth() !== currentMonth) {
          this.loadEvents(this.currentDate);
        }
      }
      else {
        const currentMonth = this.currentDate.getMonth();

        const newDate = new Date(this.currentDate);
        newDate.setDate(newDate.getDate() - 1);
        this.currentDate = newDate;

        if (this.currentDate.getMonth() !== currentMonth) {
          this.loadEvents(this.currentDate);
        }
      }
    });

    monthButton[0].addEventListener('click', () => {
      this.viewMode = 'month';
    });

    weekButton[0].addEventListener('click', () => {
      this.viewMode = 'week';
    });

    dayButton[0].addEventListener('click', () => {
      this.viewMode = 'day';
    });
  }

  unBindEvents() {
    const prevButton = this.fullCalendar.el.nativeElement.getElementsByClassName('fc-prev-button');
    const nextButton = this.fullCalendar.el.nativeElement.getElementsByClassName('fc-next-button');
    const monthButton = this.fullCalendar.el.nativeElement.getElementsByClassName('fc-dayGridMonth-button');
    const weekButton = this.fullCalendar.el.nativeElement.getElementsByClassName('fc-timeGridWeek-button');
    const dayButton = this.fullCalendar.el.nativeElement.getElementsByClassName('fc-timeGridDay-button');

    nextButton[0]?.removeEventListener('click', () => {
    });

    prevButton[0]?.removeEventListener('click', () => {
    });

    monthButton[0]?.removeEventListener('click', () => {
    });

    weekButton[0]?.removeEventListener('click', () => {
    });

    dayButton[0]?.removeEventListener('click', () => {
    });

  }

  ngOnDestroy(): void {
    this.unBindEvents();
  }

  public onAppointCancelled() {

    this.appointDialog = false;
  }

  public onAppointed() {
    this.loadEvents();
    this.appointDialog = false;
  }

  public viewRecept() {
    this.apiService.get('api/Recept/GetByAppointment/' + this.selectedAppointmentId)
      .toPromise()
      .then(data => {
        this.viewReceptDialog = true;
        this.receptModel = data;
      })
      .catch(error => {
        this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_READ'), life: 3000 });
      })
      .finally(() => { });

  }

  public closeRecept() {
    this.viewReceptDialog = false;
    this.receptModel = undefined;
  }

  public editAppointment() {
    this.apiService.get('api/appointment/GetById?id=' + this.selectedAppointmentId)
      .toPromise()
      .then(th => {
        this.selectedAppointment = th;
        this.selectedAppointment.appointmentDate = new Date(th.appointmentDate);
        this.editAppointDialog = true;
      })
      .catch(error => {
        this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_READ'), life: 3000 });
      })
      .finally(() => {
      });
  }

  public onAppointmentCancelled() {
    this.loadEvents();
    this.editAppointDialog = false;
    this.selectedAppointmentId = 0;
    this.selectedAppointment = undefined;
  }

  public onAppointmentRecept() {
    if (this.selectedAppointmentId > 0) {
      this.router.navigateByUrl('/dashboard/doctor/recept-form/' + this.selectedAppointmentId);
    }
  }

  public onAppointmentClosed() {
    this.editAppointDialog = false;
    this.selectedAppointmentId = 0;
    this.selectedAppointment = undefined;
  }

  public onAppointmentEdited() {
    this.loadEvents();
    this.editAppointDialog = false;
    this.selectedAppointmentId = 0;
    this.selectedAppointment = undefined;
  }

}
