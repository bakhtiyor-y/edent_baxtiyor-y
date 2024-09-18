import { DatePipe } from '@angular/common';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { AppointmentStatus } from 'src/app/core/enums';
import { AppointmentManageModel, ScheduleEventModel } from 'src/app/core/models/common';
import { CityModel, CountryModel, PartnerModel, RegionModel } from 'src/app/core/models/manuals';
import { DoctorModel, PatientManageModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';
import { getDateFromCalendar, getTimeString } from 'src/app/core/util/get-date-from-calendar';

@Component({
  selector: 'app-doctor-appoint-patient',
  templateUrl: './doctor-appoint-patient.component.html',
  styleUrls: ['./doctor-appoint-patient.component.scss']
})
export class DoctorAppointPatientComponent implements OnInit {

  @Input() public isOpen: boolean;
  @Input() public set selectedDate(date: Date) {
    
    
    const unixTime = new Date(date).getTime();
    this.scheduleEvents = [];
    this.apiService.get(`api/Schedule/GetScheduleByDate?date=${unixTime}`)
      .toPromise()
      .then(th => {
        th.forEach(item => {
          item.startingText = `${getTimeString(new Date(item.starting))} - ${item.name}`;
          this.scheduleEvents.push(item);
        });
      })
      .catch(error => { })
      .finally(() => { });

    this.appointmentDate = date;
  }


  @Output() public cancelled: EventEmitter<any> = new EventEmitter<any>();
  @Output() public appointed: EventEmitter<any> = new EventEmitter<any>();


  public emailRegex = '^\\w+[\\w-\\.]*\\@\\w+((-\\w+)|(\\w*))\\.[a-z]{2,3}$';
  public editForm = this.fb.group({
    id: this.fb.control(0),
    firstName: this.fb.control('', Validators.required),
    lastName: this.fb.control('', Validators.required),
    patronymic: this.fb.control(''),
    email: this.fb.control(''),
    phoneNumber: this.fb.control('', Validators.required),
    birthDate: this.fb.control(new Date(), Validators.required),
    gender: this.fb.control(0),
    patientAgeType: this.fb.control(0),
    cityId: this.fb.control(null, Validators.required),
    addressLine1: this.fb.control(''),
    addressLine2: this.fb.control(''),
  });

  public countries: CountryModel[] = [];
  public regions: RegionModel[] = [];
  public cities: CityModel[] = [];
  public patients: PatientManageModel[] = [];
  public scheduleEvents: ScheduleEventModel[] = [];
  public partners: PartnerModel[] = [];
  public appointmentDate: Date;
  public doctors: DoctorModel[] = [];
  public selectedDoctors: DoctorModel[] = [];
  public dentalChairs: any[] = [];


  public selectedCountryId = 0;
  public selectedRegionId = 0;
  public selectedPatient: PatientManageModel;
  public selectedEvent: ScheduleEventModel;
  public selectedPartner: PartnerModel;
  public selectedChair: any;

  public isPatientEditMode: boolean;
  public description: string;
  public genders: any[] = [];
  public ageTypes: any[] = [];

  @ViewChild('patientForm') patientForm: ElementRef;

  constructor(private fb: FormBuilder, private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService,
    private enumService: EnumService) { }

  ngOnInit(): void {
    this.genders = this.enumService.getGenders();
    this.ageTypes = this.enumService.getPatientAgeTypes();
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

      console.log('*********-> ', this.selectedDate);
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
    this.apiService.get(`api/DentalChair/GetDentalChairsByDate?date=${unixTime}`)
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
    appointment.dentalChairId = this.selectedChair?.id;
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

    this.selectedDoctors.forEach(d => {
      appointment.jointDoctors.push(d.id);
    });
    this.apiService.post('api/Appointment/AppointByDoctor', appointment)
      .toPromise().then(th => {
        this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('APPOINT_CREATED'), life: 3000 });
        this.selectedDoctors = [];
        this.appointed.emit();
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
    this.isPatientEditMode = false;
    this.dentalChairs = [];
    this.selectedChair = null;
    this.scheduleEvents = [];
    this.selectedPatient = null;
    this.selectedDoctors = [];
    this.editForm.reset();
    this.cancelled.emit();
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

  public newPatient() {
    this.isPatientEditMode = true;
    this.selectedCountryId = this.selectedCountryId = this.countries.find(f => f.name.toUpperCase() === 'УЗБЕКИСТАН')?.id;
    this.onCountryChange(this.selectedCountryId);
    this.patientForm.nativeElement.removeAttribute('hidden');
    if (this.selectedPatient) {
      this.editForm.patchValue(this.selectedPatient);
    }
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

}
