import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { DoctorReport, DoctorReportFilter } from 'src/app/core/models/report';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-doctors-report',
  templateUrl: './doctors-report.component.html',
  styleUrls: ['./doctors-report.component.scss']
})
export class DoctorsReportComponent implements OnInit {

  public addDialog: boolean;
  public viewDialog: boolean;
  public filter: DoctorReportFilter;

  public model: DoctorReport;

  public loading: boolean;

  public totalRecords: number;
  public lastTableLazyLoadEvent;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.filter = {} as DoctorReportFilter;
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
    this.apiService.get('api/Report/GetDoctorsReport', params).toPromise().then(data => {
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

}
