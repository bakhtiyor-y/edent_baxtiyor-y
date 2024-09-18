import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { DateFilter } from 'src/app/core/models/report';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {

  filter: DateFilter;
  receptCharData: any = {};
  doctorCharData: any = {};
  partnerCharData: any = {};
  patientChartData: any = {};
  dentalServiceChartData: any = {};

  newPatientCount: number = 0;
  receptCount: number = 0;
  dentalServiceCount: number = 0;
  income: number = 0;
  profit: number = 0;
  loading: boolean;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.filter = { from: new Date(), to: new Date() };
    this.filter.from.setMonth(this.filter.to.getMonth() - 1);

    this.apiService.get('api/Widget/NewPatientCount').toPromise().then(data => {
      this.newPatientCount = data.result;
    }).catch(error => {
    }).finally(() => {
    });

    this.apiService.get('api/Widget/ReceptCount').toPromise().then(data => {
      this.receptCount = data.result;
    }).catch(error => {
    }).finally(() => {
    });

    this.apiService.get('api/Widget/DentalServiceCount').toPromise().then(data => {
      this.dentalServiceCount = data.result;
    }).catch(error => {
    }).finally(() => {
    });

    this.apiService.get('api/Widget/Incomes').toPromise().then(data => {
      this.income = data.result / 1000000;
    }).catch(error => {
    }).finally(() => {
    });

    this.apiService.get('api/Widget/Profits').toPromise().then(data => {
      this.profit = data.result / 1000000;
    }).catch(error => {
    }).finally(() => {
    });
    this.loadData();
  }

  loadDoctorChartData(filter: DateFilter) {
    this.apiService.post('api/Report/DoctorChartData', filter).toPromise().then(data => {
      this.doctorCharData = data;
    }).catch(error => {
    }).finally(() => {
    });
  }

  loadReceptChartData(filter: DateFilter) {
    this.apiService.post('api/Report/ReceptChartData', filter).toPromise().then(data => {
      this.receptCharData = data;
    }).catch(error => {
    }).finally(() => {
    });
  }

  loadPartnerChartData(filter: DateFilter) {
    this.apiService.post('api/Report/PartnerChartData', filter).toPromise().then(data => {
      this.partnerCharData = data;
    }).catch(error => {
    }).finally(() => {
    });
  }

  loadPatientChartData(filter: DateFilter) {
    this.apiService.post('api/Report/PatientChartData', filter).toPromise().then(data => {
      this.patientChartData = data;
    }).catch(error => {
    }).finally(() => {
    });
  }

  loadServiceChartData(filter: DateFilter) {
    this.apiService.post('api/Report/ServiceChartData', filter).toPromise().then(data => {
      this.dentalServiceChartData = data;
    }).catch(error => {
    }).finally(() => {
    });
  }

  public loadData() {
    this.loading = true;
    const filter = { from: this.filter.from, to: this.filter.to };
    this.loadDoctorChartData(filter);
    this.loadPartnerChartData(filter);
    this.loadPatientChartData(filter);
    this.loadReceptChartData(filter);
    this.loadServiceChartData(filter);
  }

  public onFromDateChange(from) {
    this.loadData();
  }

  public onToDateChange(to) {
    this.loadData();
  }
}
