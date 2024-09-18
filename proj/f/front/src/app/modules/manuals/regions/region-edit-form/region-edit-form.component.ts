import { EventEmitter, Input, Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { CountryModel, RegionModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-region-edit-form',
  templateUrl: './region-edit-form.component.html',
  styleUrls: ['./region-edit-form.component.scss']
})
export class RegionEditFormComponent implements OnInit {

  public region: RegionModel;
  @Input() public set model(item: any) {
    if (item) {
      this.region = item;
      this.editForm.patchValue(item);
    } else {
      this.editForm.reset();
    }
  }

  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  public submitted: boolean;

  public selectedCountry: CountryModel;

  public countries: CountryModel[];

  public editForm = this.fb.group({
    id: this.fb.control(0),
    name: this.fb.control('', Validators.required),
    countryId: this.fb.control('0', Validators.required),
    code: this.fb.control('', Validators.required)
  });

  constructor(private apiService: ApiService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private translate: TranslateService) {

  }

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
    const region = this.editForm.value;
    const country = this.countries.find(f => f.id === region.countryId);
    if (region.id === 0) {
      this.apiService.post('api/Region', region)
        .toPromise()
        .then(th => {
          th.country = country;
          this.saved.emit({ item: th, isNew: true });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.put('api/Region', region)
        .toPromise()
        .then(th => {
          th.country = country;
          this.saved.emit({ item: th, isNew: false });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => { });
    }
  }

  public close() {
    this.closed.emit();
  }

  public onRegionChange(e) {
  }

}
