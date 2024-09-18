import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DentalServiceType } from 'src/app/core/enums';
import { AppointmentModel, ReceptDentalServiceModel, ReceptModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-rentgen-recept',
  templateUrl: './rentgen-recept.component.html',
  styleUrls: ['./rentgen-recept.component.scss']
})
export class RentgenReceptComponent implements OnInit {

  private _appointment: AppointmentModel;
  @Input() set appointment(item: AppointmentModel) {
    this._appointment = item;
  }
  get appointment() {
    return this._appointment;
  }

  @Input() public isOpen: boolean;
  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();
  @Output() public appointmentCancelled: EventEmitter<any> = new EventEmitter<any>();

  filteredGroups: any[] = [];
  selectedServices: any[] = [];
  providedServices: ReceptDentalServiceModel[] = [];
  receptDescription: string;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService,
    private confirmationService: ConfirmationService) { }

  ngOnInit(): void {
  }


  filterGroupedService(event) {
    this.apiService.get(`api/DentalServiceGroup/GetTypedByDentalService?name=${event.query}&type=${DentalServiceType.Additional}`)
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

  addToProvided() {
    if (this.selectedServices.length === 0) {
      this.messageService.add({ severity: 'info', summary: this.translate.instant('INFORMATION'), detail: this.translate.instant('SELECT_PROVIDED_SERVICE'), life: 3000 });
      return;
    }
    this.selectedServices.forEach(service => {
      const rds = {} as ReceptDentalServiceModel;
      rds.dentalServiceId = service.id;
      rds.dentalService = service;
      this.providedServices.push(rds);
    });
    this.resetSelectedServices();
  }

  resetSelectedServices() {
    this.selectedServices = [];
  }

  deleteReceptService(ind: number) {
    this.providedServices.splice(ind, 1);
  }

  save() {
    const recept = { receptDentalServices: [], receptInventories: [], treatments: [] } as ReceptModel;

    this.providedServices.forEach(srs => {
      recept.receptDentalServices.push(srs);
    });
    recept.appointmentId = this.appointment.id;
    recept.employeeId = this.appointment.employeeId;
    recept.patientId = this.appointment.patientId;
    recept.description = this.receptDescription;

    this.apiService.post('api/Recept', recept)
      .toPromise()
      .then(th => {
        this.saved.emit(th);
      })
      .catch(error => { })
      .finally(() => { });
  }

  close() {
    this.selectedServices = [];
    this.providedServices = [];
    this.receptDescription = '';
    this.closed.emit();
  }

  cancelAppointment() {
    this.confirmationService.confirm({
      message: 'Вы действительно хотите отменить запись на прием №' + this.appointment.id + '?',
      header: this.translate.instant('CANCEL_APPOINTMENT'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.get('api/Appointment/CancelByRentgen?id=' + this.appointment.id)
          .toPromise()
          .then(th => {
            this.appointmentCancelled.emit();
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
          })
          .finally(() => { });
      }
    });

  }
  getTotalSum(): number {
    let totalSumm = 0;
    this.providedServices.forEach(prs => {
      totalSumm += prs.dentalService.currentPrice;
    });
    return totalSumm;
  }
}
