import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { AppointmentStatus } from 'src/app/core/enums';
import { AppointmentManageWithLastModel } from 'src/app/core/models/common';

import { CityModel, CountryModel, PartnerModel, RegionModel } from 'src/app/core/models/manuals';
import { DoctorModel, PatientManageModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';
import { ScheduleEventModel } from 'src/app/core/models/common';
import { getTimeString } from 'src/app/core/util/get-date-from-calendar';

@Component({
  selector: 'app-appoint-patient-on-board',
  templateUrl: './appoint-patient-on-board.component.html',
  styleUrls: ['./appoint-patient-on-board.component.scss']
})
export class AppointPatientOnBoardComponent implements OnInit {
  // startTime : FormControl = new FormControl();
 

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

  public scheduleEvents: ScheduleEventModel[] = [];
  public selectedEvent: ScheduleEventModel;
  public startTimeR: any;



  public appointmentId;
  public countries: CountryModel[] = [];
  public regions: RegionModel[] = [];
  public cities: CityModel[] = [];
  public patients: PatientManageModel[] = [];
  public partners: PartnerModel[] = [];
  public dentalChairs: any[] = [];

  public selectedCountryId = 0;
  public selectedRegionId = 0;
  public selectedPatient: PatientManageModel;
  public selectedPartner: PartnerModel;
  public selectedChair: any;
  public isPatientEditMode: boolean;
  public description: string;

  public selectedDoctors: DoctorModel[] = [];
  public doctors: DoctorModel[] = [];
  public appointmentDate: Date;
  public appointmentDateLast: Date = new Date();
  public viewDate: Date;
  public genders: any[] = [];
  public ageTypes: any[] = [];
  


  @Input() public doctor: any;
  @Input() public isOpen: boolean;

  @Input() public set selectedAppointmentDate(date: Date) {
    if (date) {
      this.appointmentDate = date;
      
      this.startTimeR = `${getTimeString(new Date(date))}`;      
      
      this.viewDate = date;
      const unixTime = date.getTime();
      this.apiService.get(`api/DentalChair/GetDoctorDentalChairsByDate?doctorId=${this.doctor.doctorId}&date=${unixTime}`)
        .toPromise()
        .then(th => { 
          this.dentalChairs = th;
        }).catch(error => { })
        .finally(() => { });

        this.getAppointmentTimes(unixTime);
    }
  }





  public getAppointmentTimes(unixTime) {
    this.scheduleEvents = [];
    this.apiService.get(`api/Schedule/GetDoctorSchedule?doctorId=${this.doctor.doctorId}&date=${unixTime}`)
      .toPromise()
      .then(th => {
        th.forEach(item => {
          item.startingText = `${getTimeString(new Date(item.starting))} - ${item.name}`;
          this.scheduleEvents.push(item);
        });
      }).catch(error => { })
      .finally(() => { });    
      
  }

  public onAppointmentTimeChange(e: ScheduleEventModel) {
    this.appointmentDateLast = new Date(e.starting)
  }

  public save() {
    if (!this.selectedChair) {
      this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('DENTAL_CHAIR_NOT_SELECTED'), life: 3000 });
      return;
    }
    const appointment = { id: 0 } as AppointmentManageWithLastModel;
    appointment.appointmentDate = new Date(this.appointmentDate);
    appointment.appointmentDateLast = this.appointmentDateLast;
    appointment.appointmentStatus = AppointmentStatus.Appointed;
    appointment.description = this.description;
    appointment.partnerId = this.selectedPartner?.id;
    appointment.doctorId = this.doctor.doctorId;
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

    this.apiService.post('api/Appointment/AppointByReception', appointment)
      .toPromise().then(th => {
        this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_CREATED'), life: 3000 });
        this.selectedDoctors = [];
        console.log('wwww eee ', th);
        
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







  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public appointed: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild('patientForm') patientForm: ElementRef;

  constructor(private fb: FormBuilder,
    private apiService: ApiService,
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

 

  public cancel() {
    this.isPatientEditMode = false;
    this.selectedPatient = null;
    this.selectedDoctors = [];
    this.selectedChair = null;
    this.dentalChairs = [];
    this.editForm.reset();
    this.closed.emit();
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
