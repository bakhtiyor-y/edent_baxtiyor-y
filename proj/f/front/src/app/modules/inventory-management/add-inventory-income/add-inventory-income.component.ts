import { Output } from '@angular/core';
import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { InventoryIncomeModel } from 'src/app/core/models/common';
import { InventoryModel, MeasurementUnitTypeModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-add-inventory-income',
  templateUrl: './add-inventory-income.component.html',
  styleUrls: ['./add-inventory-income.component.scss']
})
export class AddInventoryIncomeComponent implements OnInit {

  @Input() public set model(item: InventoryIncomeModel) {
    if (item) {
      const fArray = this.editForm.get('inventoryItems') as FormArray;
      fArray.clear();
      if (item.inventoryItems) {
        for (const iterator of item.inventoryItems) {
          const mu = this.measurementUnitTypes.find(f => f.id === iterator.inventory.measurementUnitTypeId)?.measurementUnits;
          if (mu) {
            this.measurementUnits.push(mu);
          }
          fArray.push(this.getinventoryItemForm());
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

  public inventories: InventoryModel[] = [];

  public measurementUnitTypes: MeasurementUnitTypeModel[] = [];
  public measurementUnits = [];


  public editForm = this.fb.group({
    id: this.fb.control(0),
    createdDate: this.fb.control(new Date()),
    description: this.fb.control('', Validators.required),
    inventoryItems: this.fb.array([])
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
    if (!this.editForm.valid) {
      return;
    }
    const outcome = this.editForm.value;

    this.apiService.post('api/InventoryIncome', outcome)
      .toPromise()
      .then(th => {
        this.saved.emit(th);
      })
      .catch(error => { })
      .finally(() => { });
  }

  public close() {
    this.closed.emit();
  }

  public addInventory() {
    const fArray = this.editForm.get('inventoryItems') as FormArray;
    const itemForm = this.getinventoryItemForm();
    fArray.push(itemForm);
  }

  public getinventoryItemForm() {
    return this.fb.group({
      id: this.fb.control(0),
      quantity: this.fb.control(0, Validators.required),
      inventoryId: this.fb.control(0, Validators.required),
      measurementUnitId: this.fb.control(0),
      inventory: this.fb.control(null)
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
    const fArray = this.editForm.get('inventoryItems') as FormArray;
    fArray.controls[i].get('inventoryId').setValue(inventory.id);
  }

  public onInventoryClear(i) {
    const fArray = this.editForm.get('inventoryItems') as FormArray;
    fArray.controls[i].get('inventoryId').setValue(0);
  }

  public deleteInventory(i) {
    const fArray = this.editForm.get('inventoryItems') as FormArray;
    fArray.removeAt(i);
  }

  getFormArray(frmArrayName: string): FormArray {
    return this.editForm.get(frmArrayName) as FormArray;
  }

}
