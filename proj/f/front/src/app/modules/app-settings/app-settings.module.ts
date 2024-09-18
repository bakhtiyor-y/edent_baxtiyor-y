import { NgModule } from '@angular/core';
import { TermComponent } from './term/term.component';
import { ToothComponent } from './tooth/tooth.component';
import { ReceptInventorySettingComponent } from './recept-inventory-setting/recept-inventory-setting.component';
import { SharedModule } from '../shared/shared.module';
import { AppSettingsRoutingModule } from './app-settings-routing.module';
import { ReceptInventorySettingEditFormComponent } from './recept-inventory-setting/recept-inventory-setting-edit-form/recept-inventory-setting-edit-form.component';
import { OrganizationComponent } from './organization/organization.component';
import { TabViewModule } from 'primeng/tabview';

@NgModule({
  declarations: [
    TermComponent,
    ToothComponent,
    ReceptInventorySettingComponent,
    ReceptInventorySettingEditFormComponent,
    OrganizationComponent
  ],
  imports: [
    SharedModule,
    AppSettingsRoutingModule,
    TabViewModule
  ]
})
export class AppSettingsModule { }
