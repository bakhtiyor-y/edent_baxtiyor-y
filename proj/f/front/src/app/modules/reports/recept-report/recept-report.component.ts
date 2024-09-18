import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { ReceptModel } from 'src/app/core/models/common';
import { ReceptReport, ReceptReportFilter, ReceptReportItem } from 'src/app/core/models/report';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-recept-report',
  templateUrl: './recept-report.component.html',
  styleUrls: ['./recept-report.component.scss']
})
export class ReceptReportComponent implements OnInit {

  //public addDialog: boolean;
  public viewDialog: boolean;
  public viewReceptDialog: boolean;
  public receptItem: ReceptModel;

  public filter: ReceptReportFilter;
  public model: ReceptReport;
  public loading: boolean;
  public totalRecords: number;
  public lastTableLazyLoadEvent;


  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.filter = {} as ReceptReportFilter;
    this.filter.to = new Date();
    this.filter.from = new Date();
    this.filter.from.setMonth(this.filter.to.getMonth() - 1);
    this.loadData();
  }


  onSaved(item) {
  }

  public loadData() {
    this.loading = true;
    const filter = { from: this.filter.from.toLocaleDateString('en-US'), to: this.filter.to.toLocaleDateString('en-US') };
    const params = new HttpParams().set('filter', JSON.stringify(filter));
    this.apiService.get('api/Report/GetReceptReport', params).toPromise().then(data => {
      this.model = data;
    }).catch(error => {
    }).finally(() => {
      this.loading = false;
    });
  }

  public onFromDateChange(from) {
    this.loadData();
  }

  public onToDateChange(to) {
    this.loadData();
  }

  public calculatedWithDoctor(item: ReceptReportItem) {
    const data = { id: item.id, isDoctorCalculated: true };
    this.apiService.put('api/Recept/CalculateWithDoctor', data)
      .toPromise()
      .then(th => {
        if (th.result) {
          item.isDoctorCalculated = th.result;
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
        } else {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        }
      })
      .catch(error => {
        this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
      })
      .finally(() => { });
  }

  public calculatedWithTechnic(item: ReceptReportItem) {
    const data = { id: item.id, isTechnicCalculated: true };
    this.apiService.put('api/Recept/CalculateWithTechnic', data)
      .toPromise()
      .then(th => {
        if (th.result) {
          item.isTechnicCalculated = th.result;
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
        } else {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        }
      })
      .catch(error => {
        this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
      })
      .finally(() => { });
  }

  public calculatedWithPartner(item: ReceptReportItem) {
    const data = { id: item.id, isPartnerCalculated: true };
    this.apiService.put('api/Recept/CalculateWithPartner', data)
      .toPromise()
      .then(th => {
        if (th.result) {
          item.isPartnerCalculated = th.result;
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
        } else {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        }
      })
      .catch(error => {
        this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
      })
      .finally(() => { });
  }

  openRecept(receptId: number) {
    this.apiService.get('api/Recept/GetById/' + receptId)
      .toPromise()
      .then(data => {
        this.viewReceptDialog = true;
        this.receptItem = data;
      })
      .catch(error => { })
      .finally(() => { });
  }

  closeRecept() {
    this.viewReceptDialog = false;
    this.receptItem = null;
  }
}
