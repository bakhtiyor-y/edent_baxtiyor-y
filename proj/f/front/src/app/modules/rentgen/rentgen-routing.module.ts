import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ReceptionGuard, RentgenGuard } from 'src/app/core/services/guard-services';
import { RentgenDeskComponent } from './rentgen-desk/rentgen-desk.component';
import { RentgenReceptsComponent } from './rentgen-recepts/rentgen-recepts.component';


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'rentgen-desk',
                component: RentgenDeskComponent
            },
            {
                path: '',
                children: [
                    { path: '', redirectTo: 'rentgen-recepts' },
                    { path: 'rentgen-recepts', component: RentgenReceptsComponent },
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class RentgenRoutingModule { }