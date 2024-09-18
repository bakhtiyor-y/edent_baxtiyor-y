import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CitiesComponent } from './cities/cities.component';
import { CountriesComponent } from './countries/countries.component';
import { DentalChairsComponent } from './dental-chairs/dental-chairs.component';
import { DentalServiceCategoryComponent } from './dental-service-category/dental-service-category.component';
import { DentalServiceGroupComponent } from './dental-service-group/dental-service-group.component';
import { DentalServiceComponent } from './dental-service/dental-service.component';
import { DiagnoseComponent } from './diagnose/diagnose.component';
import { InvetntoryComponent } from './invetntory/invetntory.component';
import { MeasurementUnitTypeComponent } from './measurement-units/measurement-unit-type/measurement-unit-type.component';
import { PartnersComponent } from './partners/partners.component';
import { ProfessionComponent } from './profession/profession.component';
import { RegionsComponent } from './regions/regions.component';


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    { path: '', redirectTo: 'countries' },
                    { path: 'countries', component: CountriesComponent },
                    { path: 'regions', component: RegionsComponent },
                    { path: 'cities', component: CitiesComponent },
                    { path: 'diagnoses', component: DiagnoseComponent },
                    { path: 'professions', component: ProfessionComponent },
                    { path: 'measurement-unit', component: MeasurementUnitTypeComponent },
                    { path: 'inventories', component: InvetntoryComponent },
                    { path: 'dental-service-group', component: DentalServiceGroupComponent },
                    { path: 'dental-service-category', component: DentalServiceCategoryComponent },
                    { path: 'dental-service', component: DentalServiceComponent },
                    { path: 'dental-chairs', component: DentalChairsComponent },
                    { path: 'partners', component: PartnersComponent }
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class ManualsRoutingModule { }
