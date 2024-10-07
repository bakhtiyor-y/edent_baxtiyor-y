import { HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { CardModule } from 'primeng/card';
import { CheckboxModule } from 'primeng/checkbox';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { FullCalendarModule } from 'primeng/fullcalendar';
import { InputMaskModule } from 'primeng/inputmask';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MenuModule } from 'primeng/menu';
import { MultiSelectModule } from 'primeng/multiselect';
import { PasswordModule } from 'primeng/password';
import { RadioButtonModule } from 'primeng/radiobutton';
import { SelectButtonModule } from 'primeng/selectbutton';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { ToolbarModule } from 'primeng/toolbar';
import { HttpTokenInterceptor } from '../../core/interceptors/http-token.interceptor';
import { ToothControlComponent } from './components/tooth-control/tooth-control.component';
import { DirectivesModule } from './directives/directives.module';
import { PipesModule } from './pipes/pipes.module';
import { ViewReceptComponent } from './components/view-recept/view-recept.component';
import { PatientViewComponent } from './components/patients/patient-view/patient-view.component';
import { PatientsComponent } from './components/patients/patients.component';
import { PatientEditFormComponent } from './components/patients/patient-edit-form/patient-edit-form.component';
import { SetTechnicComponent } from './components/set-technic/set-technic.component';
import { TeethControlComponent } from './components/teeth-control/teeth-control.component';
import { PatientQuestionnaireComponent } from './components/patient-questionnaire/patient-questionnaire.component';
import { CommonModule } from '@angular/common';
import { TooltipModule } from 'primeng/tooltip';
import { SplitButtonModule } from 'primeng/splitbutton';
import { PatientToothHistoryComponent } from './components/patient-tooth-history/patient-tooth-history.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { PatientAlreadyExistsComponent } from './components/patients/patient-edit-form/patient-already-exists/patient-already-exists.component';

export function translateFactory(http: HttpClient): any {
    return new TranslateHttpLoader(http);
}

const MODULES = [
    CommonModule,
    ToolbarModule,
    TooltipModule,
    PipesModule,
    DirectivesModule,
    ReactiveFormsModule,
    FormsModule,
    AutoCompleteModule,
    BreadcrumbModule,
    ButtonModule,
    CalendarModule,
    CardModule,
    CheckboxModule,
    ConfirmDialogModule,
    ConfirmPopupModule,
    ContextMenuModule,
    DialogModule,
    DropdownModule,
    SplitButtonModule,
    FieldsetModule,
    FullCalendarModule,
    InputNumberModule,
    InputMaskModule,
    InputSwitchModule,
    InputTextModule,
    InputTextareaModule,
    MenuModule,
    MultiSelectModule,
    PasswordModule,
    RadioButtonModule,
    SelectButtonModule,
    TableModule,
    ToastModule
];

@NgModule({
    imports: [
        ...MODULES,
        TableModule,
        TranslateModule.forChild({
            loader: {
                provide: TranslateLoader,
                useFactory: translateFactory,
                deps: [HttpClient]
            },
            defaultLanguage: 'ru'
        })
    ],
    exports: [
        ...MODULES,
        ToothControlComponent,
        PatientAlreadyExistsComponent,
        ViewReceptComponent,
        PatientsComponent,
        PatientEditFormComponent,
        SetTechnicComponent,
        PatientViewComponent,
        TeethControlComponent,
        PatientToothHistoryComponent,
        TranslateModule
    ],
    declarations: [ToothControlComponent,
        ViewReceptComponent,
        PatientViewComponent,
        PatientsComponent,
        PatientEditFormComponent,
        SetTechnicComponent,
        TeethControlComponent,
        PatientQuestionnaireComponent,
        PatientToothHistoryComponent,
        PatientAlreadyExistsComponent,
        ]
})
export class SharedModule {

    public static forRoot(): ModuleWithProviders<SharedModule> {
        return {
            ngModule: SharedModule,
            providers: [
                { provide: HTTP_INTERCEPTORS, useClass: HttpTokenInterceptor, multi: true },
                MessageService,
                ConfirmationService
            ]
        };
    }
}
