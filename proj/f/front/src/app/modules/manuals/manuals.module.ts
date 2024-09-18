import { NgModule } from '@angular/core';
import { CountriesComponent } from './countries/countries.component';
import { CitiesComponent } from './cities/cities.component';
import { RegionsComponent } from './regions/regions.component';
import { ManualsRoutingModule } from './manuals-routing.module';
import { CountryEditFormComponent } from './countries/country-edit-form/country-edit-form.component';
import { SharedModule } from '../shared/shared.module';
import { RegionEditFormComponent } from './regions/region-edit-form/region-edit-form.component';
import { CityEditFormComponent } from './cities/city-edit-form/city-edit-form.component';
import { InvetntoryComponent } from './invetntory/invetntory.component';
import { DiagnoseComponent } from './diagnose/diagnose.component';
import { ProfessionComponent } from './profession/profession.component';
import { SpecializationComponent } from './profession/specialization/specialization.component';
import { DentalServiceComponent } from './dental-service/dental-service.component';
import { MeasurementUnitTypeComponent } from './measurement-units/measurement-unit-type/measurement-unit-type.component';
import { MeasurementUnitComponent } from './measurement-units/measurement-unit/measurement-unit.component';
import { InventoryPriceComponent } from './invetntory/inventory-price/inventory-price.component';
import { InventoryEditFormComponent } from './invetntory/inventory-edit-form/inventory-edit-form.component';
import { DentalServicePriceComponent } from './dental-service/dental-service-price/dental-service-price.component';
import { DentalServiceEditFormComponent } from './dental-service/dental-service-edit-form/dental-service-edit-form.component';
import { PartnersComponent } from './partners/partners.component';
import { DentalServiceGroupComponent } from './dental-service-group/dental-service-group.component';
import { DentalServiceCategoryComponent } from './dental-service-category/dental-service-category.component';
import { DentalChairsComponent } from './dental-chairs/dental-chairs.component';
import { DentalChairEditFormComponent } from './dental-chairs/dental-chair-edit-form/dental-chair-edit-form.component';

@NgModule({
  declarations: [CountriesComponent,
    CitiesComponent,
    RegionsComponent,
    CountryEditFormComponent,
    RegionEditFormComponent,
    CityEditFormComponent,
    InvetntoryComponent,
    DiagnoseComponent,
    ProfessionComponent,
    SpecializationComponent,
    DentalServiceComponent,
    MeasurementUnitTypeComponent,
    MeasurementUnitComponent,
    InventoryPriceComponent,
    InventoryEditFormComponent,
    DentalServicePriceComponent,
    DentalServiceEditFormComponent,
    PartnersComponent,
    DentalServiceGroupComponent,
    DentalServiceCategoryComponent,
    DentalChairsComponent,
    DentalChairEditFormComponent],
  imports: [
    SharedModule,
    ManualsRoutingModule
  ]
})
export class ManualsModule { }
