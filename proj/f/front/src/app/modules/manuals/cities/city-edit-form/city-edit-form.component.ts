import { EventEmitter } from '@angular/core';
import { Component, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { CityModel, CountryModel, RegionModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-city-edit-form',
  templateUrl: './city-edit-form.component.html',
  styleUrls: ['./city-edit-form.component.scss']
})
export class CityEditFormComponent implements OnInit {

  public city: CityModel;
  @Input() public set model(item: CityModel) {
    if (item) {
      this.city = item;
      if (item.region) {
        this.selectedCountryId = item.region.countryId;
        this.onCountryChange(this.selectedCountryId);
      }
      this.editForm.patchValue(item);
    } else {
      this.editForm.reset();
    }
  }

  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  public selectedCountryId: number;
  public countries: CountryModel[];
  public regions: RegionModel[];

  public editForm = this.fb.group({
    id: this.fb.control(0),
    name: this.fb.control('', Validators.required),
    regionId: this.fb.control(0, Validators.required),
    code: this.fb.control('', Validators.required)
  });

  constructor(private apiService: ApiService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.apiService.get('api/Country/GetAll')
      .toPromise()
      .then(th => {
        this.countries = th;
      }).catch(error => { })
      .finally(() => { });
  }

  save() {
    if (!this.editForm.valid) {
      return;
    }
    const city = this.editForm.value;
    const region = this.regions.find(f => f.id === city.regionId);
    if (city.id === 0) {
      this.apiService.post('api/City', city)
        .toPromise()
        .then(th => {
          th.region = region;
          this.saved.emit({ item: th, isNew: true });
          this.selectedCountryId = 0;
          this.regions = [];
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.put('api/City', city)
        .toPromise()
        .then(th => {
          th.region = region;
          this.saved.emit({ item: th, isNew: false });
          this.selectedCountryId = 0;
          this.regions = [];
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => { });
    }
  }

  public close() {
    this.selectedCountryId = 0;
    this.regions = [];
    this.closed.emit();
  }

  public onCountryChange(e) {
    if (e) {
      this.apiService.get('api/Region/GetByCountry?countryId=' + e)
        .toPromise()
        .then(th => {
          this.regions = th;
        })
        .catch(error => { })
        .finally(() => { });
    }
  }
}
