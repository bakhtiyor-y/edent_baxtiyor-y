import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { PatientReportItem } from 'src/app/core/models/report';
import { PatientModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-patients-report',
  templateUrl: './patients-report.component.html',
  styleUrls: ['./patients-report.component.scss']
})
export class PatientsReportComponent implements OnInit {

  public viewInvoiceDialog: boolean;

  public items: PatientReportItem[];

  public loading: boolean;
  public totalRecords: number;
  public isSearchClear = false;
  public lastTableLazyLoadEvent;
  public patientRecepts: any[] = [];

  public selectedPatient: PatientReportItem;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService) {

  }

  ngOnInit(): void {
    this.loading = false;
  }

  public loadItems(event, name = '') {
    this.lastTableLazyLoadEvent = event;
    let params = new HttpParams()
      .set('filter', JSON.stringify(event));

    if (name) {
      params = new HttpParams()
        .set('filter', JSON.stringify(event))
        .set('name', name);
    }

    this.apiService.get('api/Report/GetPatientsReport', params).toPromise().then(th => {
      this.items = th.data;
      this.totalRecords = th.total;
    }).catch(error => {
    }).finally(() => {
      this.loading = false;
    });
  }
  public onPageChange(event) {
    this.loading = true;
  }

  onSearch(value) {
    if (value && value.length > 3) {
      this.isSearchClear = true;
      this.loadItems(this.lastTableLazyLoadEvent, value);

    } else {
      if (this.isSearchClear) {
        this.isSearchClear = false;
        this.loadItems(this.lastTableLazyLoadEvent, '');
      }
    }
  }

  public viewInvoices(item: PatientReportItem) {
    this.selectedPatient = item;
    this.viewInvoiceDialog = true;
  }

  public onViewInvoiceClosed() {
    this.viewInvoiceDialog = false;
    this.selectedPatient = null;
  }


}
