import { EventEmitter, Input } from '@angular/core';
import { Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { TermModel } from 'src/app/core/models/appsettings';
import { CityModel, CountryModel, DentalChairModel, RegionModel, SpecializationModel } from 'src/app/core/models/manuals';
import { DoctorManageModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';
import { getDateFromCalendar } from 'src/app/core/util/get-date-from-calendar';

@Component({
  selector: 'app-doctor-edit-form',
  templateUrl: './doctor-edit-form.component.html',
  styleUrls: ['./doctor-edit-form.component.scss']
})
export class DoctorEditFormComponent implements OnInit {

  public isNew: boolean;
  @Input() public set model(item: DoctorManageModel) {
    if (item) {
      this.isNew = !item.id || item.id === 0;
      if (this.isNew) {
        this.selectedCountryId = this.countries.find(f => f.name.toUpperCase() === 'УЗБЕКИСТАН')?.id;
      }
      else {
        this.selectedCountryId = item.countryId;
        this.selectedRegionId = item.regionId;
      }
      this.editForm.patchValue(item);
      this.onCountryChange(this.selectedCountryId);
    } else {
      this.editForm.reset();
    }
  }
  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  public submitted: boolean;
  public emailRegex = '^\\w+[\\w-\\.]*\\@\\w+((-\\w+)|(\\w*))\\.[a-z]{2,3}$';

  public editForm = this.fb.group({
    id: this.fb.control(0),
    userId: this.fb.control(0),
    firstName: this.fb.control('', Validators.required),
    lastName: this.fb.control('', Validators.required),
    patronymic: this.fb.control(''),
    gender: this.fb.control(0),
    email: this.fb.control('', [Validators.required, Validators.email]),
    phoneNumber: this.fb.control('', Validators.required),
    specializationId: this.fb.control(0, [Validators.required, Validators.min(1)]),
    termId: this.fb.control(0, Validators.required),
    termValue: this.fb.control(0, Validators.required),
    birthDate: this.fb.control(new Date(), Validators.required),
    isActive: this.fb.control(false),
    cityId: this.fb.control(0, [Validators.required, Validators.min(1)]),
    dentalChairs: this.fb.control(''),
    addressLine1: this.fb.control('', Validators.required),
    addressLine2: this.fb.control(''),
  });

  public terms: TermModel[] = [];
  public specializations: SpecializationModel[] = [];

  public countries: CountryModel[] = [];
  public regions: RegionModel[] = [];
  public cities: CityModel[] = [];
  public dentalChairs: DentalChairModel[] = [];
  public selectedCountryId = 0;
  public selectedRegionId = 0;
  public genders: any[] = [];
  constructor(private fb: FormBuilder,
    private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService,
    private enumService: EnumService) { }

  ngOnInit(): void {
    this.genders = this.enumService.getGenders();
    this.apiService.get('api/Specialization/GetAll')
      .toPromise()
      .then(th => {
        this.specializations = th;
      }).catch(error => {

      }).finally(() => { });

    this.apiService.get('api/Term/GetAll')
      .toPromise()
      .then(th => {
        this.terms = th;
      }).catch(error => {

      }).finally(() => { });

    this.apiService.get('api/Country/GetAll')
      .toPromise()
      .then(th => {
        this.countries = th;
      }).catch(error => { })
      .finally(() => { });

    this.apiService.get('api/DentalChair/GetAll')
      .toPromise()
      .then(th => {
        this.dentalChairs = th;
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

  save() {
    this.submitted = true;
    if (!this.editForm.valid) {
      return;
    }
    const doctor = this.editForm.value;
    doctor.birthDate = getDateFromCalendar(doctor.birthDate).toLocaleDateString('en-US');
    if (doctor.id === 0) {
      this.apiService.post('api/Doctor', doctor).toPromise()
        .then(th => {
          this.saved.emit({ doctor: th, isNew: true });
          this.editForm.reset();
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.put('api/Doctor', doctor).toPromise()
        .then(th => {
          this.saved.emit({ doctor: th, isNew: false });
          this.editForm.reset();
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => { this.submitted = false; });
    }
  }

  public close() {
    this.submitted = false;
    this.regions = [];
    this.cities = [];
    this.selectedCountryId = 0;
    this.selectedRegionId = 0;
    this.editForm.reset();
    this.closed.emit();
  }

}
