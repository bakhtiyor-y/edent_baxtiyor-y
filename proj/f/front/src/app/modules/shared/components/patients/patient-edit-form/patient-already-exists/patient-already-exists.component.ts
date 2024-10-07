import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PatientManageModel } from 'src/app/core/models/user-management';
import { FormBuilder, Validators } from '@angular/forms';
import { CityModel, CountryModel, RegionModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-patient-already-exists',
  templateUrl: './patient-already-exists.component.html',
  styleUrls: ['./patient-already-exists.component.scss']
})
export class PatientAlreadyExistsComponent implements OnInit {
  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService,
    private enumService: EnumService
  ) { }  
  
  
  public countries: CountryModel[] = [];
  public regions: RegionModel[] = [];
  public cities: CityModel[] = [];
  public selectedCountryId = 0;
  public selectedRegionId = 0;
  public genders: any[] = [];
  public ageTypes: any[] = [];
  public isDisabled: boolean



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

  @Output() public closedExists: EventEmitter<any> = new EventEmitter<any>();

  @Input() isOpen1: boolean;

  @Input() public set modelExists(item: PatientManageModel) {
    console.log("tttttttt--->> ", item);
    
    if (item){
      if (item.countryId && item.regionId) {
        this.selectedCountryId = item.countryId;
        this.selectedRegionId = item.regionId;
      }
      this.editFormExists.patchValue(item);
      this.onCountryChange(this.selectedCountryId);
      this.editFormExists.disable();
      this.isDisabled = true;
    }
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




  public editFormExists = this.fb.group({
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
    selectedRegionId: [],
    addressLine1: this.fb.control(''),
    addressLine2: this.fb.control(''),
  });

  public closeExists() {
    this.closedExists.emit()
  }

}
