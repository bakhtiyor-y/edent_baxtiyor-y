import { DatePipe } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { AppointmentStatus } from 'src/app/core/enums';
import { AppointmentManageModel } from 'src/app/core/models/common';
import { ScheduleEventModel } from 'src/app/core/models/common/schedule-event.model';
import { CityModel, CountryModel, PartnerModel, RegionModel } from 'src/app/core/models/manuals';
import { DoctorModel, PatientManageModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { getTimeString } from 'src/app/core/util/get-date-from-calendar';

@Component({
  selector: 'app-appoint-patient',
  templateUrl: './appoint-patient.component.html',
  styleUrls: ['./appoint-patient.component.scss']
})
export class AppointPatientComponent implements OnInit {

  public emailRegex = '^\\w+[\\w-\\.]*\\@\\w+((-\\w+)|(\\w*))\\.[a-z]{2,3}$';
  public editForm = this.fb.group({
    id: this.fb.control(0),
    firstName: this.fb.control('', Validators.required),
    lastName: this.fb.control('', Validators.required),
    patronymic: this.fb.control(''),
    email: this.fb.control(''),
    phoneNumber: this.fb.control('', Validators.required),
    birthDate: this.fb.control(new Date(), Validators.required),
    cityId: this.fb.control(null, Validators.required),
    addressLine1: this.fb.control(''),
    addressLine2: this.fb.control(''),
  });

  public appointmentId;

  public countries: CountryModel[] = [];
  public regions: RegionModel[] = [];
  public cities: CityModel[] = [];
  public patients: PatientManageModel[] = [];
  public doctors: DoctorModel[] = [];
  public scheduleEvents: ScheduleEventModel[] = [];
  public partners: PartnerModel[] = [];
  public dentalChairs: any[] = [];

  public selectedCountryId = 0;
  public selectedRegionId = 0;
  public selectedPatient: PatientManageModel;
  public selectedEvent: ScheduleEventModel;
  public selectedPartner: PartnerModel;
  public selectedDoctor: DoctorModel;
  public selectedDoctors: DoctorModel[] = [];
  public selectedChair: any;
  public appointmentDate: Date = new Date();
  public isPatientEditMode: boolean;
  public description: string;

  @ViewChild('patientForm') patientForm: ElementRef;

  constructor(private fb: FormBuilder,
    private apiService: ApiService,
    private router: Router,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.apiService.get('api/Partner/GetAll')
      .toPromise()
      .then(th => {
        this.partners = th;
      })
      .catch(error => { })
      .finally(() => { });

    this.apiService.get('api/Country/GetAll')
      .toPromise()
      .then(th => {
        this.countries = th;
        this.selectedCountryId = this.countries.find(f => f.name.toUpperCase() === 'УЗБЕКИСТАН')?.id;
        this.onCountryChange(this.selectedCountryId);
      }).catch(error => { })
      .finally(() => { });
  }

  public onCountryChange(e) {
    if (e) {
      this.apiService.get('api/Region/GetByCountry?countryId=' + e)
        .toPromise()
        .then(th => {
          this.regions = th;
          if (this.selectedRegionId === 0) {
            this.selectedRegionId = this.regions[0].id;
          }
          this.onRegionChange(this.selectedRegionId);
        })
        .catch(error => { })
        .finally(() => { });
    }
  }

  public onRegionChange(e) {
    if (e) {
      this.apiService.get('api/City/GetByRegion?regionId=' + e)
        .toPromise()
        .then(th => {
          this.cities = th;
        })
        .catch(error => { })
        .finally(() => { });
    }
  }

  public onAppointmentTimeChange(e: ScheduleEventModel) {
    const unixTime = new Date(e.starting).getTime();
    this.apiService.get(`api/DentalChair/GetDoctorDentalChairsByDate?doctorId=${this.selectedDoctor.id}&date=${unixTime}`)
      .toPromise()
      .then(th => {
        this.dentalChairs = th;
      }).catch(error => { })
      .finally(() => { });
  }

  public save() {
    if (!this.selectedChair) {
      this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('DENTAL_CHAIR_NOT_SELECTED'), life: 3000 });
      return;
    }
    const appointmentTime = new Date(this.selectedEvent.starting);
    const appointment = {} as AppointmentManageModel;
    appointment.appointmentDate = new Date(this.appointmentDate.getFullYear(),
      this.appointmentDate.getMonth(),
      this.appointmentDate.getDate(),
      appointmentTime.getHours(),
      appointmentTime.getMinutes());
    appointment.appointmentStatus = AppointmentStatus.Appointed;
    appointment.description = this.description;
    appointment.partnerId = this.selectedPartner?.id;
    appointment.doctorId = this.selectedDoctor.id;
    appointment.dentalChairId = this.selectedChair?.id;
    this.selectedDoctors.forEach(d => {
      appointment.jointDoctors.push(d.id);
    });
    if (this.isPatientEditMode) {
      if (!this.editForm.valid) {
        this.editForm.markAllAsTouched();
        return;
      }
      const patient = this.editForm.value;
      patient.birthDate = new Date(patient.birthDate);
      appointment.patient = patient;
    } else {
      appointment.patient = this.selectedPatient;
    }
    if (!appointment.patient) {
      this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('PATIENT_NOT_SELECTED'), life: 3000 });
      return;
    }

    this.apiService.post('api/Appointment/AppointByReception', appointment)
      .toPromise().then(th => {
        this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_CREATED'), life: 3000 });
        this.router.navigateByUrl('/dashboard/reception/appointments');
      })
      .catch(error => {
        if (error.error.message) {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: error.error.message, life: 3000 });
        } else {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        }
      })
      .finally(() => { });

  }

  public cancel() {
    this.router.navigateByUrl('/dashboard/reception/appointments');
  }

  public searchPatient(event) {
    this.apiService.get('api/Patient/SearchByName?name=' + event.query)
      .toPromise()
      .then(th => {
        this.patients = th;
      })
      .catch(error => { })
      .finally(() => { });
  }

  public onPatientClear() {
    this.selectedPatient = null;
    this.editForm.reset();
  }

  public onPatientSelect(event) {
    this.selectedPatient.birthDate = new Date(this.selectedPatient.birthDate);
  }

  public searchDoctor(event) {
    this.apiService.get('api/Doctor/SearchByName?name=' + event.query)
      .toPromise()
      .then(th => {
        this.doctors = th;
      })
      .catch(error => { })
      .finally(() => { });
  }

  public onDoctorClear() {
    this.scheduleEvents = [];
    this.selectedDoctor = null;
    this.selectedChair = null;
    this.dentalChairs = [];
  }

  public onDoctorSelect(event) {
    if (this.appointmentDate) {
      this.scheduleEvents = [];
      const unixTime = new Date(this.appointmentDate).getTime();
      this.apiService.get(`api/Schedule/GetDoctorSchedule?doctorId=${event.id}&date=${unixTime}`)
        .toPromise()
        .then(th => {
          th.forEach(item => {
            item.startingText = `${getTimeString(new Date(item.starting))} - ${item.name}`;
            this.scheduleEvents.push(item);
          });
        })
        .catch(error => { })
        .finally(() => { });
    }
  }

  public newPatient() {
    this.isPatientEditMode = true;
    this.selectedCountryId = this.selectedCountryId = this.countries.find(f => f.name.toUpperCase() === 'УЗБЕКИСТАН')?.id;
    this.onCountryChange(this.selectedCountryId);
    this.patientForm.nativeElement.removeAttribute('hidden');
    if (this.selectedPatient) {
      this.editForm.patchValue(this.selectedPatient);
    }
  }

  public onAppointmentDateChange(event) {
    if (this.selectedDoctor) {
      this.scheduleEvents = [];
      const unixTime = new Date(this.appointmentDate).getTime();
      this.apiService.get(`api/Schedule/GetDoctorSchedule?doctorId=${this.selectedDoctor.id}&date=${unixTime}`)
        .toPromise()
        .then(th => {
          th.forEach(item => {
            item.startingText = `${getTimeString(new Date(item.starting))} - ${item.name}`;
            this.scheduleEvents.push(item);
          });
        })
        .catch(error => { })
        .finally(() => { });
    }
  }
}
