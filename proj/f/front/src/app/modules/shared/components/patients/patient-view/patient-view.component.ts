import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Teeth } from 'src/app/core/models/appsettings';
import { ReceptModel } from 'src/app/core/models/common';
import { PatientModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { TeethControlComponent } from '../../teeth-control/teeth-control.component';

@Component({
  selector: 'app-patient-view',
  templateUrl: './patient-view.component.html',
  styleUrls: ['./patient-view.component.scss']
})
export class PatientViewComponent implements OnInit {

  // public recept: any;
  patient: PatientModel;
  recepts: ReceptModel[] = [];
  currentTeeth: Teeth;

  @Input() public isOpen: boolean;

  @Input() public set model(data: any) {
    if (!this.teethCtrl) {
      return;
    }
    if (data) {
      this.recepts = data.recepts;
      this.patient = data.patient;
      let selectedTeethIds = [];
      this.recepts.forEach(r => {
        r.treatments.forEach(treatment => {
          if (treatment.patientToothId) {
            const selected = [treatment.patientToothId];
            selectedTeethIds = [...selectedTeethIds, ...selected];
          }
        });
      });
      let unique = [...new Set(selectedTeethIds)];
      this.teethCtrl.setSelectedIds(unique);

      this.initTeeth(this.patient.id);
    } else {
      this.teethCtrl.deselectAll();
    }
  }

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @ViewChild('teethCtrl') teethCtrl: TeethControlComponent;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {

  }

  initTeeth(patientId: number) {
    this.apiService.get(`api/Patient/GetTeeth/${patientId}`)
      .toPromise()
      .then(pt => {
        this.currentTeeth = pt as Teeth;
      });
  }

  getDentalServicePrice(rInd, dentalService) {

    const dentalServicePrice = dentalService.dentalServicePrices.sort((a, b) => {
      return new Date(a.dateFrom).getTime() - new Date(b.dateFrom).getTime();
    }).filter(f => f.dateFrom < this.recepts[rInd].createdDate)[0];
    return dentalServicePrice?.price || 0;
  }

  getTotalPrice(rInd) {
    if (this.recepts) {
      let tprice = 0;
      this.recepts[rInd].treatments.forEach(treatment => {
        treatment.treatmentDentalServices.forEach(tds => {
          tprice += this.getDentalServicePrice(rInd, tds.dentalService);
        });
      });
      this.recepts[rInd].receptDentalServices.forEach(rds => {
        tprice += this.getDentalServicePrice(rInd, rds.dentalService);
      });
      return tprice;
    }
    else {
      return 0;
    }
  }

  close() {
    this.closed.emit();
  }
}
