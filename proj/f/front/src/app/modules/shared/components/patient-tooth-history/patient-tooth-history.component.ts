import { Component, Input, OnInit } from '@angular/core';
import { PatientToothModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-patient-tooth-history',
  templateUrl: './patient-tooth-history.component.html',
  styleUrls: ['./patient-tooth-history.component.scss']
})
export class PatientToothHistoryComponent implements OnInit {

  toothHistories: any = [];

  _patientTooth: PatientToothModel;
  @Input() set patientTooth(tooth: PatientToothModel) {
    this._patientTooth = tooth
    if (this._patientTooth) {
      this.loadHistory(this._patientTooth.id);
    } else {
      this.toothHistories = [];
    }
  }
  get patientTooth() {
    return this._patientTooth;
  }

  @Input() maxHeight;
  @Input() height;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
  }

  loadHistory(patientToothId: number) {
    this.apiService.get(`api/Patient/GetPatientToothHistory?patientToothId=${patientToothId}`)
      .toPromise()
      .then(th => {
        this.toothHistories = [];
        th.forEach(t => {
          t.treatmentDate = new Date(t.treatmentDate);
          this.toothHistories.push(t);
        });
      });
  }

}
