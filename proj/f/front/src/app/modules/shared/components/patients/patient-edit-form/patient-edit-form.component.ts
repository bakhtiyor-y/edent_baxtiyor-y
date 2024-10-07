import { HttpParams } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { CityModel, CountryModel, RegionModel } from 'src/app/core/models/manuals';
import { PatientManageModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';

@Component({
  selector: 'app-patient-edit-form',
  templateUrl: './patient-edit-form.component.html',
  styleUrls: ['./patient-edit-form.component.scss']
})
export class PatientEditFormComponent implements OnInit {

  public isNew: boolean;
  public isPatientFound: boolean = false;
  @Input() public set model(item: PatientManageModel) {
    if (item) {
      
      this.isNew = !item.id || item.id === 0;
      if (this.isNew) {
        this.selectedCountryId = this.countries.find(f => f.name.toUpperCase() === 'УЗБЕКИСТАН')?.id;
      } else {
        if (item.countryId && item.regionId) {
          this.selectedCountryId = item.countryId;
          this.selectedRegionId = item.regionId;
        } else {
          this.selectedCountryId = this.countries.find(f => f.name.toUpperCase() === 'УЗБЕКИСТАН')?.id;
        }
      }
      this.editForm.patchValue(item);
      this.onCountryChange(this.selectedCountryId);
    } else {
      console.log("this.editForm.reset(); ", item);
      this.editForm.reset();
    }
  }

  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  public submitted: boolean;

  public emailRegex = '^\\w+[\\w-\\.]*\\@\\w+((-\\w+)|(\\w*))\\.[a-z]{2,3}$';

  public editForm = this.fb.group({
    id: this.fb.control(0, Validators.required),
    firstName: this.fb.control('', Validators.required),
    lastName: this.fb.control('', Validators.required),
    patronymic: this.fb.control(''),
    gender: this.fb.control(0),
    patientAgeType: this.fb.control(0),
    email: this.fb.control(''),
    phoneNumber: this.fb.control('', Validators.required),
    birthDate: this.fb.control(new Date(), Validators.required),
    cityId: this.fb.control(0, Validators.required),
    addressLine1: this.fb.control(''),
    addressLine2: this.fb.control(''),
  });

  public countries: CountryModel[] = [];
  public regions: RegionModel[] = [];
  public cities: CityModel[] = [];
  public selectedCountryId = 0;
  public selectedRegionId = 0;
  public genders: any[] = [];
  public ageTypes: any[] = [];

  public existsDialog: boolean;
  public existsItem: PatientManageModel;

  constructor(private fb: FormBuilder,
    private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService,
    private enumService: EnumService) { }

  ngOnInit(): void {
    this.genders = this.enumService.getGenders();
    this.ageTypes = this.enumService.getPatientAgeTypes();
    this.apiService.get('api/Country/GetAll')
      .toPromise()
      .then(th => {
        this.countries = th;
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

  save(par: string) {
    
    this.submitted = true;
    if (!this.editForm.valid) {
      return;
    }
    const patient = this.editForm.value;
    const fullname = patient.firstName + " " + patient.lastName;
    if (patient.id === 0 || par == "fromSave") {
      console.log("this.existsDialog == undefined || par == ", par);
      let params = new HttpParams()
      .set('fullname', fullname)
      .set('birthDate', patient.birthDate.toISOString());
      

      if(par != "fromSave"){
        this.apiService.get('api/Patient/CheckIfExists', params)
        .toPromise()
        .then(th => { 
          if(th){
            this.existsItem = th;
            patient.id = th.id;
            this.existsItem.birthDate = new Date(th.birthDate);
            document.querySelector("#editformid").classList.add("patientfound");
            this.isPatientFound = true;
            this.existsDialog = true;
          } else {
            console.log("innn", patient);
            
            this.addPatientToDb(patient);
          }
        })
        .catch(error => { })
        .finally(() => { });
      } else {
        this.addPatientToDb(patient);
      }
      
    } else {
      this.apiService.put('api/Patient', patient).toPromise()
        .then(th => {
          console.log("UPDATE ", this.existsDialog)
          
          this.saved.emit({ item: th, isNew: false });
          this.existsDialog = false;
          this.editForm.reset();
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => { });
    }
  }

  public close() {
    this.onClosedExists()
    this.regions = [];
    this.cities = [];
    this.selectedCountryId = 0;
    this.selectedRegionId = 0;
    this.editForm.reset();
    this.closed.emit();
  }

  public onClosedExists(){
    this.existsDialog = false;
    this.isPatientFound = false;
    document.querySelector("#editformid").classList.remove("patientfound");
  }

  addPatientToDb(patient){
    this.apiService.post('api/Patient', patient).toPromise()
    .then(th => {
      this.saved.emit({ item: th, isNew: true });
      console.log("POST ", th);
      
      this.editForm.reset();
    })
    .catch(error => {
      this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
    })
    .finally(() => { });
  }
}


