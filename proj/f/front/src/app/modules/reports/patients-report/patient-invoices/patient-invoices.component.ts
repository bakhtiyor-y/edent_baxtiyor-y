import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { InvoiceModel, ReceptModel } from 'src/app/core/models/common';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-patient-invoices',
  templateUrl: './patient-invoices.component.html',
  styleUrls: ['./patient-invoices.component.scss']
})
export class PatientInvoicesComponent implements OnInit {

  public viewReceptDialog: boolean;

  @Input() public set patientId(pId: number) {
    if (pId) {
      this.loadItems(pId);
    }
  }

  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();

  public items: InvoiceModel[];
  public editItem: InvoiceModel;

  public currentRecept: ReceptModel;

  public loading: boolean;


  constructor(private apiService: ApiService) {

  }

  ngOnInit(): void {
    this.loading = false;
  }

  public loadItems(patientId: number) {
    this.apiService.get('api/Invoice/GetByPatient?patientId=' + patientId)
      .toPromise()
      .then(th => {
        this.items = th;
      }).catch(error => {
      }).finally(() => {
        this.loading = false;
      });
  }

  public onPageChange(event) {
    this.loading = true;
  }

  public viewDetails(item: InvoiceModel) {
    this.apiService.get('api/Recept/GetById/' + item.receptId)
      .toPromise()
      .then(th => {
        this.viewReceptDialog = true;
        this.currentRecept = th;
      })
      .catch(error => {

      })
      .finally(() => { });
  }

  public closeRecept() {
    this.viewReceptDialog = false;
    this.currentRecept = null;
  }

  findIndexById(id: number): number {
    let index = -1;
    for (let i = 0; i < this.items.length; i++) {
      if (this.items[i].id === id) {
        index = i;
        break;
      }
    }
    return index;
  }

  onClose(){
    this.closed.emit();
  }

}
