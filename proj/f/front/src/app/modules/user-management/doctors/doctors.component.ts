import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DoctorManageModel, DoctorModel, SetPasswordModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { getDateFromCalendar } from 'src/app/core/util/get-date-from-calendar';

@Component({
  selector: 'app-doctors',
  templateUrl: './doctors.component.html',
  styleUrls: ['./doctors.component.scss']
})
export class DoctorsComponent implements OnInit {

  public doctorDialog: boolean;
  public passwordDialog: boolean;

  public doctors: DoctorModel[] = [];

  public selectedDoctors: DoctorModel[];
  public doctor: DoctorManageModel;

  public setPasswordModel: SetPasswordModel;

  public loading: boolean;
  public totalRecords: number;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private translate: TranslateService) {

  }

  ngOnInit(): void {
    this.loading = false;
  }

  add() {
    this.doctor = { id: 0 } as DoctorManageModel;
    this.doctor.birthDate = new Date();
    this.doctorDialog = true;
  }

  deleteSelectedUsers() {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRIES'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.post('api/Doctor/DeleteSelected', this.selectedDoctors)
          .toPromise()
          .then(th => {
            this.doctors = this.doctors.filter(val => !this.selectedDoctors.includes(val));
            this.selectedDoctors = null;
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRIES_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRIES'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  edit(doctor: DoctorModel) {
    this.apiService.get('api/Doctor/GetById?id=' + doctor.id)
      .toPromise()
      .then(th => {
        this.doctor = th;
        this.doctor.birthDate = new Date(th.birthDate);
        this.doctorDialog = true;
      });
  }

  delete(doctor: DoctorModel) {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRY'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.delete('api/Doctor?id=' + doctor.id)
          .toPromise()
          .then(th => {
            this.doctors = this.doctors.filter(val => val.id !== doctor.id);
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRY'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  onClosed() {
    this.doctor = null;
    this.doctorDialog = false;
  }

  onSaved(result) {
    if (result.isNew) {
      this.doctors.push(result.doctor);
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_CREATED'), life: 3000 });
    } else {
      this.doctors[this.findIndexById(result.doctor.id)] = result.doctor;
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
    }
    this.doctors = [...this.doctors];
    this.doctorDialog = false;
  }

  public setPassword(doctor: DoctorModel) {
    this.setPasswordModel = {} as SetPasswordModel;
    this.setPasswordModel.userId = doctor.userId;
    this.passwordDialog = true;
  }

  public setPasswordHandler() {
    this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
    this.setPasswordClosed();
  }

  public setPasswordClosed() {
    this.passwordDialog = false;
    this.setPasswordModel = null;
  }

  public loadDoctors(event) {
    this.doctors = [];
    const params = new HttpParams().set('filter', JSON.stringify(event));
    this.apiService.get('api/Doctor', params).toPromise()
      .then(th => {
        th.data.forEach(item => {
          item.birthDate = getDateFromCalendar(new Date(item.birthDate));
          this.doctors.push(item);
        });
        this.totalRecords = th.total;
        console.log('th ', th);
      }).catch(error => {
      }).finally(() => {
        this.loading = false;
      });

      console.log('this.doctors ', this.doctors);
      
  }
  public onPageChange(event) {
    this.loading = true;
  }

  findIndexById(id: number): number {
    let index = -1;
    for (let i = 0; i < this.doctors.length; i++) {
      if (this.doctors[i].id === id) {
        index = i;
        break;
      }
    }
    return index;
  }

}
