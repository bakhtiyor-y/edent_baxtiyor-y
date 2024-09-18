import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { OrganizationModel } from 'src/app/core/models/appsettings/organization.model';
import { CityModel, CountryModel, RegionModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-organization',
  templateUrl: './organization.component.html',
  styleUrls: ['./organization.component.scss']
})
export class OrganizationComponent implements OnInit {

  public organization: OrganizationModel;
  public countries: CountryModel[] = [];
  public regions: RegionModel[] = [];
  public cities: CityModel[] = [];
  public selectedCountryId: number;
  public selectedRegionId: number;
  public logoImage: string = 'assets/layout/images/dentos_logo_white.png';
  @ViewChild('fileUpload', { static: false }) fileUpload: ElementRef;
  
  public editForm = this.fb.group({
    id: this.fb.control(0),
    name: this.fb.control('', Validators.required),
    inn: this.fb.control('', Validators.required),
    okonx: this.fb.control('', Validators.required),
    oked: this.fb.control('', Validators.required),
    mfo: this.fb.control('', Validators.required),
    addressId: this.fb.control(0, Validators.required),
    address: this.fb.group({
      id: this.fb.control(0, Validators.required),
      cityId: this.fb.control(0, Validators.required),
      addressLine1: this.fb.control('', Validators.required),
      addressLine2: this.fb.control('', Validators.required),
    })
  });


  constructor(private apiService: ApiService, private fb: FormBuilder,
    private messageService: MessageService, private translate: TranslateService) { }

  ngOnInit(): void {
    this.apiService.get('api/Country/GetAll')
      .toPromise()
      .then(th => {
        this.countries = th;
      }).catch(error => { })
      .finally(() => { });

    this.apiService.get('api/organization')
      .toPromise()
      .then(th => {
        this.organization = th;
        this.selectedCountryId = this.organization.address.city.region.countryId;
        this.selectedRegionId = this.organization.address.city.regionId;
        if(th.logoImage){
          this.logoImage = th.logoImage;
        }
        this.onCountryChange(this.organization.address.city.region.countryId);
        this.editForm.patchValue(this.organization);
      })
      .catch(error => {

      })
      .finally(() => { });
  }

  public onCountryChange(e) {
    if (e) {
      this.apiService.get('api/Region/GetByCountry?countryId=' + e)
        .toPromise()
        .then(th => {
          this.regions = th;
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

  public save() {
    if (!this.editForm.valid) {
      return;
    }
    const organization = this.editForm.value;
    this.apiService.put('api/Organization', organization).toPromise()
      .then(th => {
        localStorage.setItem('orgInfo', JSON.stringify(th));
        this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ORGANIZATION_UPDATED'), life: 3000 });
      })
      .catch(error => {
        this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ORGANIZATION_UPDATE_ERROR'), life: 3000 });
      })
      .finally(() => { });
  }

  getFormGroup(name: string) {
    return this.editForm.get(name) as FormGroup;
  }

  public openFileUpload() {
    this.fileUpload.nativeElement.click();
  }

  upload(files) {
    if (files.length > 0) {
      const formData = new FormData();

      for (const file of files) {
        formData.append('logoImages', file);
      }

      this.apiService.post('/api/Organization/UpdateLogo', formData)
        .toPromise()
        .then(th => {
          this.logoImage = th.logoImage;
          const orgInfo = JSON.parse(localStorage.getItem('orgInfo'));
          orgInfo.logoImage = th.logoImage;
          localStorage.setItem('orgInfo', JSON.stringify(orgInfo));
        })
        .catch(error => {

        })
        .finally(() => {

        });
    }
  }



}
