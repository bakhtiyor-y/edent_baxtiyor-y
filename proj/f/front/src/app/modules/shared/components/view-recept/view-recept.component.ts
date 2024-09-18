import { Component, Input, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ContextMenu } from 'primeng/contextmenu';
import { ToothDirection } from 'src/app/core/enums';
import { Teeth, ToothModel } from 'src/app/core/models/appsettings';
import { ReceptModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';
import { TeethControlComponent } from '../teeth-control/teeth-control.component';

@Component({
  selector: 'app-view-recept',
  templateUrl: './view-recept.component.html',
  styleUrls: ['./view-recept.component.scss']
})
export class ViewReceptComponent implements OnInit {

  public topLeftTeeth: ToothModel[] = [];
  public topRightTeeth: ToothModel[] = [];
  public bottomLeftTeeth: ToothModel[] = [];
  public bottomRightTeeth: ToothModel[] = [];

  public recept: ReceptModel;
  public currentTeeth: Teeth;

  @Input() public set model(data: ReceptModel) {
    if (!this.teethCtrl) {
      return;
    }
    // this.teethCtrl.deselectAll();
    if (data) {
      this.recept = data;
      let selectedTeethIds = [];
      this.recept.treatments.forEach(treatment => {
        if (treatment.patientToothId) {
          const selected = [treatment.patientToothId];
          selectedTeethIds = [...selectedTeethIds, ...selected];
        }
      });
      let unique = [...new Set(selectedTeethIds)];
      this.teethCtrl.setSelectedIds(unique);

      this.initTeeth(this.recept.patientId);
    } else {
      this.teethCtrl.deselectAll();
    }
  }

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

  public getDentalServicePrice(dentalService) {
    const dentalServicePrice = dentalService.dentalServicePrices.filter(f => new Date(f.dateFrom) < new Date(this.recept.createdDate))[0];
    return dentalServicePrice?.price || 0;
  }

  public getTotalPrice() {
    if (this.recept) {
      let tprice = 0;
      this.recept.treatments.forEach(treatment => {
        treatment.treatmentDentalServices.forEach(tds => {
          tprice += this.getDentalServicePrice(tds.dentalService);
        });
      });
      this.recept.receptDentalServices.forEach(rds => {
        tprice += this.getDentalServicePrice(rds.dentalService);
      });
      return tprice;
    }
    else {
      return 0;
    }
  }

  public onToothClick() {
    return false;
  }
}
