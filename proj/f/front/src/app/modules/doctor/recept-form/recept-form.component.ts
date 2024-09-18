import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { DentalServiceType } from 'src/app/core/enums';
import { ReceptInventorySettingItemModel, Teeth } from 'src/app/core/models/appsettings';
import {
  AppointmentModel,
  PatientToothModel,
  ReceptDentalServiceModel,
  ReceptInventoryModel,
  ReceptModel,
  TreatmentDentalServiceModel,
  TreatmentModel
} from 'src/app/core/models/common';
import { DiagnoseModel, MeasurementUnitTypeModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';
import { TeethControlComponent } from '../../shared/components/teeth-control/teeth-control.component';

@Component({
  selector: 'app-recept-form',
  templateUrl: './recept-form.component.html',
  styleUrls: ['./recept-form.component.scss']
})
export class ReceptFormComponent implements OnInit {

  filteredGroups: any[] = [];
  diagnoses: any[] = [];

  treatmentDescription: string;
  selectedServices: any[] = [];
  selectedReceptServices: any[] = [];

  providedReceptServices: ReceptDentalServiceModel[] = [];
  selectedDiagnose: DiagnoseModel;
  treatments: TreatmentModel[] = [];
  selectedTeeth: PatientToothModel[] = [];
  receptInventories: ReceptInventorySettingItemModel[] = [];
  receptDescription: string;

  currentTeeth: Teeth;
  measurementUnitTypes: MeasurementUnitTypeModel[] = [];
  measurementUnits = [];

  currentAppointment: AppointmentModel;
  DentalServiceType = DentalServiceType;
  //viewDialog: boolean = false;
  isSome: boolean = false;

  @ViewChild('teethCtrl') teethCtrl: TeethControlComponent;

  constructor(private route: ActivatedRoute, private apiService: ApiService,
    private messageService: MessageService,
    private router: Router,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.apiService.get('api/MeasurementUnitType/GetAll')
      .toPromise()
      .then(th => { this.measurementUnitTypes = th; })
      .catch(error => { })
      .finally(() => { });

    const appointmentId = this.route.snapshot.params.id;

    this.apiService.get('api/Appointment/GetById?id=' + appointmentId)
      .toPromise()
      .then(th => {
        this.currentAppointment = th;
        this.initTeeth(this.currentAppointment.patientId);
      })
      .catch(error => { })
      .finally(() => { });

    this.apiService.get('api/ReceptInventorySetting/GetDefaults')
      .toPromise()
      .then(th => {
        this.receptInventories = th;
        this.receptInventories.forEach(element => {
          const mu = this.measurementUnitTypes.find(f => f.id === element.selectedInventory.measurementUnitTypeId)?.measurementUnits;
          if (mu) {
            this.measurementUnits.push(mu);
          }
        });
      })
      .catch(error => { })
      .finally(() => { });

  }

  initTeeth(patientId: number) {
    this.apiService.get(`api/Patient/GetTeeth/${patientId}`)
      .toPromise()
      .then(pt => {
        this.currentTeeth = pt as Teeth;
      });
  }

  filterGroupedService(event, type: DentalServiceType) {
    this.apiService.get(`api/DentalServiceGroup/GetTypedByDentalService?name=${event.query}&type=${type}`)
      .toPromise()
      .then(th => {
        const tGroups = [];
        th.forEach(element => {
          const group = {
            name: element.name,
            items: []
          };
          element.dentalServices.forEach(service => {
            group.items.push({ name: service.name, id: service.id, currentPrice: service.currentPrice });
          });
          tGroups.push(group);
        });
        this.filteredGroups = tGroups;
      })
      .catch(error => { })
      .finally(() => { });
  }

  public filterDiagnose(event) {
    this.apiService.get('api/Diagnose/GetByName?name=' + event.query)
      .toPromise()
      .then(th => {
        this.diagnoses = th;
      })
      .catch(error => { })
      .finally(() => { });
  }

  onTeethSelected(patientTeeth: PatientToothModel[]) {
    this.selectedTeeth = patientTeeth;
  }

  getSelectedTooth(): PatientToothModel {
    if (this.selectedTeeth.length > 0) {
      return this.selectedTeeth[this.selectedTeeth.length - 1];
    }
    return null;
  }

  public addToAdditionalProvided() {
    if (this.selectedReceptServices.length === 0) {
      this.messageService.add({ severity: 'info', summary: this.translate.instant('INFORMATION'), detail: this.translate.instant('SELECT_PROVIDED_SERVICE'), life: 3000 });
      return;
    }
    this.selectedReceptServices.forEach(service => {
      const rds = {} as ReceptDentalServiceModel;
      rds.dentalServiceId = service.id;
      rds.dentalService = service;
      this.providedReceptServices.push(rds);
    });
    this.resetAdditionalProvidedServices();
  }


  public addToProvided() {
    if (this.selectedServices.length === 0) {
      this.messageService.add({ severity: 'info', summary: this.translate.instant('INFORMATION'), detail: this.translate.instant('SELECT_PROVIDED_SERVICE'), life: 3000 });
      return;
    }

    this.selectedTeeth.forEach(patientTooth => {
      const treatment = { treatmentDentalServices: [] } as TreatmentModel;
      treatment.patientToothId = patientTooth.id;
      treatment.patientTooth = patientTooth;
      treatment.diagnoseId = this.selectedDiagnose?.id;
      treatment.diagnose = this.selectedDiagnose;
      treatment.description = this.treatmentDescription;
      this.selectedServices.forEach(service => {
        const treatmentService = {} as TreatmentDentalServiceModel;
        treatmentService.dentalServiceId = service.id;
        treatmentService.dentalService = service;
        treatment.treatmentDentalServices.push(treatmentService);
      });
      this.treatments.push(treatment);
    });

    this.isSome = false;
    this.resetProvidedServices();
  }

  public finishRecept() {
    const recept = { receptDentalServices: [], receptInventories: [] } as ReceptModel;

    this.receptInventories.forEach(element => {
      const receptInventory = {} as ReceptInventoryModel;
      receptInventory.inventoryId = element.inventoryId;
      receptInventory.quantity = element.quantity;
      receptInventory.measurementUnitId = element.measurementUnitId;
      recept.receptInventories.push(receptInventory);
    });
    this.providedReceptServices.forEach(srs => {
      recept.receptDentalServices.push(srs);
    });
    recept.treatments = this.treatments;
    recept.appointmentId = this.currentAppointment.id;
    recept.doctorId = this.currentAppointment.doctorId;
    recept.patientId = this.currentAppointment.patientId;
    recept.description = this.receptDescription;

    this.apiService.post('api/Recept', recept)
      .toPromise()
      .then(th => {
        this.router.navigateByUrl('/dashboard/doctor/recept');
      })
      .catch(error => { })
      .finally(() => { });
  }

  public finishReceptAsPlan() {

  }

  public deleteService(ind: number) {
    this.treatments.splice(ind, 1);
  }

  deleteReceptService(ind: number) {
    this.providedReceptServices.splice(ind, 1);
  }

  resetProvidedServices() {
    this.teethCtrl.deselectAll();
    this.selectedServices = [];
    this.selectedTeeth = [];
    this.selectedDiagnose = null;
    this.treatmentDescription = '';
  }
  resetAdditionalProvidedServices() {
    this.selectedReceptServices = [];
  }

  public traetmentDialogClose() {
    this.resetProvidedServices();
  }

  getTotalSum(): number {
    let totalSumm = 0;
    this.treatments.forEach(t => {
      t.treatmentDentalServices.forEach(ds => {
        totalSumm += ds.dentalService.currentPrice;
      });
    });
    this.providedReceptServices.forEach(prs => {
      totalSumm += prs.dentalService.currentPrice;
    });
    return totalSumm;
  }
}
