import { NgModule } from '@angular/core';
import { RentgenDeskComponent } from './rentgen-desk/rentgen-desk.component';
import { RentgenRoutingModule } from './rentgen-routing.module';
import { SharedModule } from '../shared/shared.module';
import { AppointPatientToRentgenComponent } from './appoint-patient-to-rentgen/appoint-patient-to-rentgen.component';
import { RentgenReceptsComponent } from './rentgen-recepts/rentgen-recepts.component';
import { RentgenReceptComponent } from './rentgen-recept/rentgen-recept.component';



@NgModule({
  declarations: [
    RentgenDeskComponent,
    AppointPatientToRentgenComponent,
    RentgenReceptsComponent,
    RentgenReceptComponent
  ],
  imports: [
    RentgenRoutingModule, SharedModule
  ]
})
export class RentgenModule { }
