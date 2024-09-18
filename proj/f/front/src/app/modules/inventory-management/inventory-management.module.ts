import { NgModule } from '@angular/core';
import { InventoryOutcomeComponent } from './inventory-outcome/inventory-outcome.component';
import { InventoryIncomeComponent } from './inventory-income/inventory-income.component';
import { InventoryIncomeFormComponent } from './inventory-income-form/inventory-income-form.component';
import { InventoryOutcomeFormComponent } from './inventory-outcome-form/inventory-outcome-form.component';
import { InventoryManagementRoutingModule } from './inventory-management.routing';
import { SharedModule } from '../shared/shared.module';
import { AddInventoryIncomeComponent } from './add-inventory-income/add-inventory-income.component';
import { AddInventoryOutcomeComponent } from './add-inventory-outcome/add-inventory-outcome.component';



@NgModule({
  declarations: [
    InventoryOutcomeComponent,
    InventoryIncomeComponent,
    InventoryIncomeFormComponent,
    InventoryOutcomeFormComponent,
    AddInventoryIncomeComponent,
    AddInventoryOutcomeComponent
  ],
  imports: [
    InventoryManagementRoutingModule,
    SharedModule
  ]
})
export class InventoryManagementModule { }
