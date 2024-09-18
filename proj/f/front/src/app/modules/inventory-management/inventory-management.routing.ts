import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AdminGuard } from 'src/app/core/services/guard-services';
import { InventoryIncomeComponent } from './inventory-income/inventory-income.component';
import { InventoryOutcomeComponent } from './inventory-outcome/inventory-outcome.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: '', redirectTo: 'income' },
                    {
                        path: 'income', component: InventoryIncomeComponent
                    },
                    { path: 'outcome', component: InventoryOutcomeComponent }
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class InventoryManagementRoutingModule { }
