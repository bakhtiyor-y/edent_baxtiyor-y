import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { OrganizationComponent } from './organization/organization.component';
import { ReceptInventorySettingComponent } from './recept-inventory-setting/recept-inventory-setting.component';
import { TermComponent } from './term/term.component';
import { ToothComponent } from './tooth/tooth.component';


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: '', redirectTo: 'terms' },
                    { path: 'terms', component: TermComponent },
                    { path: 'recept-inventories', component: ReceptInventorySettingComponent },
                    { path: 'teeth', component: ToothComponent },
                    { path: 'organization', component: OrganizationComponent },
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppSettingsRoutingModule { }
