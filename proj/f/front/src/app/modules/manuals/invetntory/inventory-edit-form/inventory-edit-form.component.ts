import { EventEmitter, Input } from '@angular/core';
import { Component, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { InventoryModel, InventoryPriceModel, MeasurementUnitModel, MeasurementUnitTypeModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-inventory-edit-form',
  templateUrl: './inventory-edit-form.component.html',
  styleUrls: ['./inventory-edit-form.component.scss']
})
export class InventoryEditFormComponent implements OnInit {

  public inventory: InventoryModel;

  @Input() public set model(item: any) {
    if (item && this.isOpen) {
      this.inventory = item;
      this.editForm.patchValue(item);
      this.onMeasurementUnitTypeChange({ value: item.measurementUnitTypeId });
    }
  }

  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  public selectedPrice: InventoryPriceModel;

  public measurementUnitTypes: MeasurementUnitTypeModel[] = [];
  public measurementUnits: MeasurementUnitModel[] = [];


  public editForm = this.fb.group({
    id: this.fb.control(0),
    name: this.fb.control('', Validators.required),
    stock: this.fb.control(0, Validators.required),
    currentPrice: this.fb.control({ value: 0, disabled: true }, Validators.required),
    measurementUnitId: this.fb.control(0, Validators.required),
    measurementUnitTypeId: this.fb.control(0, Validators.required)
  });

  constructor(private apiService: ApiService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private translate: TranslateService) {

  }

  ngOnInit(): void {
    this.apiService.get('api/MeasurementUnitType/GetAll')
      .toPromise()
      .then(th => { this.measurementUnitTypes = th; })
      .catch(error => { })
      .finally(() => { });
  }

  save() {
    if (!this.editForm.valid) {
      return;
    }
    const inventory = this.editForm.getRawValue();
    if (!inventory.measurementUnitId || !inventory.measurementUnitTypeId) {
      return;
    }

    const measurementUnit = this.measurementUnits.find(f => f.id === inventory.measurementUnitId);
    inventory.stock = measurementUnit.multiplicity * inventory.stock;
    if (inventory.id === 0) {
      this.apiService.post('api/Inventory', inventory)
        .toPromise()
        .then(th => {
          this.saved.emit({ item: th, isNew: true });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.put('api/Inventory', inventory)
        .toPromise()
        .then(th => {
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

  public onMeasurementUnitTypeChange(e) {
    if (e && e.value) {
      this.measurementUnits = this.measurementUnitTypes.find(f => f.id === e.value)?.measurementUnits;
    }
  }

  public onPriceChanged(e) {
    this.editForm.get('currentPrice').setValue(e);
  }
}
