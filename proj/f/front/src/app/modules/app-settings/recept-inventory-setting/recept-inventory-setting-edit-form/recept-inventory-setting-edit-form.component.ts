import { EventEmitter, Input, Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { ReceptInventorySettingModel } from 'src/app/core/models/appsettings';
import { InventoryModel, MeasurementUnitTypeModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-recept-inventory-setting-edit-form',
  templateUrl: './recept-inventory-setting-edit-form.component.html',
  styleUrls: ['./recept-inventory-setting-edit-form.component.scss']
})
export class ReceptInventorySettingEditFormComponent implements OnInit {

  @Input() public set model(item: ReceptInventorySettingModel) {
    if (item) {
      this.isNew = !item || item.id === 0;
      const fArray = this.editForm.get('receptInventorySettingItems') as FormArray;
      fArray.clear();
      if (item.receptInventorySettingItems) {
        for (const iterator of item.receptInventorySettingItems) {
          const mu = this.measurementUnitTypes.find(f => f.id === iterator.selectedInventory.measurementUnitTypeId)?.measurementUnits;
          if (mu) {
            this.measurementUnits.push(mu);
          }
          fArray.push(this.getReceptInventoryItemForm());
        }
      }
      this.editForm.patchValue(item);
    } else {
      this.editForm.reset();
    }
  }

  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  public submitted: boolean;
  public inventories: InventoryModel[] = [];

  public measurementUnitTypes: MeasurementUnitTypeModel[] = [];
  public measurementUnits = [];
  public isNew: boolean;


  public editForm = this.fb.group({
    id: this.fb.control(0),
    name: this.fb.control('', Validators.required),
    isActive: this.fb.control(false),
    isDefault: this.fb.control(false),
    receptInventorySettingItems: this.fb.array([])
  });

  constructor(private apiService: ApiService, private fb: FormBuilder) {

  }

  ngOnInit(): void {
    this.apiService.get('api/MeasurementUnitType/GetAll')
      .toPromise()
      .then(th => { this.measurementUnitTypes = th; })
      .catch(error => { })
      .finally(() => { });
  }

  save() {
    this.submitted = true;
    if (!this.editForm.valid) {
      return;
    }
    const receptInventorySetting = this.editForm.value;
    if (receptInventorySetting.id === 0) {
      this.apiService.post('api/ReceptInventorySetting', receptInventorySetting).toPromise()
        .then(th => {
          th.receptInventorySettingItems = receptInventorySetting.receptInventorySettingItems;
          this.saved.emit({ item: th, isNew: true });
        })
        .catch(error => { })
        .finally(() => { this.submitted = false; });
    } else {
      this.apiService.put('api/ReceptInventorySetting', receptInventorySetting).toPromise()
        .then(th => {
          this.saved.emit({ item: th, isNew: false });
        })
        .catch(error => { })
        .finally(() => { this.submitted = false; });
    }
  }

  public close() {
    this.submitted = false;
    this.closed.emit();
  }

  public addInventory() {
    const fArray = this.editForm.get('receptInventorySettingItems') as FormArray;
    const itemForm = this.getReceptInventoryItemForm();
    fArray.push(itemForm);
  }

  public getReceptInventoryItemForm() {
    return this.fb.group({
      id: this.fb.control(0),
      quantity: this.fb.control(0, Validators.required),
      inventoryId: this.fb.control(0, Validators.required),
      measurementUnitId: this.fb.control(0),
      selectedInventory: this.fb.control(null)
    });
  }

  public search(event) {
    this.apiService.get('api/Inventory/GetInventoriesByName?name=' + event.query)
      .toPromise()
      .then(data => {
        this.inventories = data;
      })
      .catch(error => { })
      .finally();
  }

  public onInventorySelect(inventory, i) {
    const mu = this.measurementUnitTypes.find(f => f.id === inventory.measurementUnitTypeId)?.measurementUnits;
    if (mu) {
      if (this.measurementUnits[i]) {
        this.measurementUnits[i] = mu;
      } else {
        this.measurementUnits.push(mu);
      }
    }
    const fArray = this.editForm.get('receptInventorySettingItems') as FormArray;
    fArray.controls[i].get('inventoryId').setValue(inventory.id);
  }

  public onInventoryClear(i) {
    const fArray = this.editForm.get('receptInventorySettingItems') as FormArray;
    fArray.controls[i].get('inventoryId').setValue(0);
  }

  public deleteInventory(i) {
    const fArray = this.editForm.get('receptInventorySettingItems') as FormArray;
    fArray.removeAt(i);
  }

  getFormArray(frmArrayName: string): FormArray {
    return this.editForm.get(frmArrayName) as FormArray;
  }
}
