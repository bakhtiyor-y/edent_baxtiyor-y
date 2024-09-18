import { EventEmitter, Output } from '@angular/core';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { ReceptInventorySettingModel } from 'src/app/core/models/appsettings';
import { DentalServiceCategoryModel, DentalServiceGroupModel, DentalServiceModel, DentalServicePriceModel } from 'src/app/core/models/manuals';
import { ApiService, EnumService } from 'src/app/core/services';

@Component({
  selector: 'app-dental-service-edit-form',
  templateUrl: './dental-service-edit-form.component.html',
  styleUrls: ['./dental-service-edit-form.component.scss']
})
export class DentalServiceEditFormComponent implements OnInit {

  public dentalService: DentalServiceModel;

  @Input() public set model(item: DentalServiceModel) {
    if (item && this.isOpen) {
      this.dentalService = item;
      this.editForm.patchValue(item);
      if (item.receptInventorySettings && item.receptInventorySettings.length > 0) {
        this.apiService.post('api/ReceptInventorySetting/ByIds', item.receptInventorySettings)
          .toPromise()
          .then(th => {
            this.selectedReceptInventorySettings = th;
          })
          .catch(error => { })
          .finally(() => { });
      }
    } else {
      this.editForm.reset();
    }
  }

  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  public selectedPrice: DentalServicePriceModel;

  public dentalServiceGroups: DentalServiceGroupModel[] = [];
  public dentalServiceCategories: DentalServiceCategoryModel[] = [];

  public receptInventorySettings: ReceptInventorySettingModel[] = [];
  public selectedReceptInventorySettings: ReceptInventorySettingModel[] = [];
  public dentalServiceTypes: any[] = [];
  public toothStates: any[] = [];


  public editForm = this.fb.group({
    id: this.fb.control(0),
    name: this.fb.control('', Validators.required),
    description: this.fb.control(''),
    type: this.fb.control(0),
    toothState: this.fb.control(0),
    dentalServiceGroupId: this.fb.control(0, Validators.required),
    dentalServiceCategoryId: this.fb.control(null)
  });

  constructor(private apiService: ApiService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private translate: TranslateService,
    private enumService: EnumService) {

  }

  ngOnInit(): void {
    this.apiService.get('api/DentalServiceCategory/GetAll')
      .toPromise()
      .then(th => { this.dentalServiceCategories = th; })
      .catch(error => { })
      .finally(() => { });

    this.apiService.get('api/DentalServiceGroup/GetAll')
      .toPromise()
      .then(th => { this.dentalServiceGroups = th; })
      .catch(error => { })
      .finally(() => { });

      this.dentalServiceTypes = this.enumService.getDentalServiceTypes();
      this.toothStates = this.enumService.getToothStates();
  }

  save() {
    if (!this.editForm.valid) {
      return;
    }
    const dentalService = this.editForm.getRawValue();
    dentalService.receptInventorySettings = this.selectedReceptInventorySettings.map(m => m.id);

    if (dentalService.id === 0) {
      this.apiService.post('api/DentalService', dentalService)
        .toPromise()
        .then(th => {
          this.selectedReceptInventorySettings = [];
          this.saved.emit({ item: th, isNew: true });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.put('api/DentalService', dentalService)
        .toPromise()
        .then(th => {
          this.selectedReceptInventorySettings = [];
          this.saved.emit({ item: th, isNew: false });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => { });
    }
  }

  public close() {
    this.selectedReceptInventorySettings = [];
    this.closed.emit();
  }

  public onPriceChanged(e) {
    this.editForm.get('currentPrice').setValue(e);
  }

  public onDentalServiceGroupChange(e) {
    if (e && e.value) {
      this.dentalService.dentalServiceGroup = this.dentalServiceGroups.find(f => f.id === e.value);
      this.dentalService.groupName = this.dentalService.dentalServiceGroup.name;
    }

  }
  public onDentalServiceCategorChange(e) {
    if (e && e.value) {
      this.dentalService.dentalServiceCategory = this.dentalServiceCategories.find(f => f.id === e.value);
      this.dentalService.categoryName = this.dentalService.dentalServiceCategory.name;
    }
    else {
      this.dentalService.dentalServiceCategory = null;
      this.dentalService.categoryName = '';
    }
  }

  public onReceptInventorySettingSelected(event) {
    this.apiService.get('api/ReceptInventorySetting/GetByName?name=' + event.query)
      .toPromise()
      .then(th => { this.receptInventorySettings = th; })
      .catch(error => { })
      .finally(() => { });
  }
}
