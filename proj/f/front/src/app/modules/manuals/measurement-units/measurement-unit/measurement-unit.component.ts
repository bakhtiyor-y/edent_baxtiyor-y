import { HttpParams } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { MeasurementUnitModel, MeasurementUnitTypeModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-measurement-unit',
  templateUrl: './measurement-unit.component.html',
  styleUrls: ['./measurement-unit.component.scss']
})
export class MeasurementUnitComponent implements OnInit {

  @Input() public measurementUnitType;

  public items: MeasurementUnitModel[];

  public selectedItems: MeasurementUnitModel[];

  public loading: boolean;

  public clonedItems: { [s: string]: MeasurementUnitModel; } = {};

  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();

  public totalRecords: number;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private translate: TranslateService) { }

  ngOnInit(): void {
  }

  add() {
    const muom = { id: 0 } as MeasurementUnitModel;
    muom.measurementUnitTypeId = this.measurementUnitType.id;
    return muom;
  }

  edit(item: MeasurementUnitModel) {
    this.clonedItems[item.id] = { ...item };
  }

  delete(item: MeasurementUnitModel) {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRY'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.delete('api/MeasurementUnit?id=' + item.id)
          .toPromise()
          .then(th => {
            this.items = this.items.filter(val => val.id !== item.id);
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRY'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  deleteSelected() {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRIES'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.post('api/MeasurementUnit/DeleteSelected', this.selectedItems)
          .toPromise()
          .then(th => {
            this.items = this.items.filter(val => !this.selectedItems.includes(val));
            this.selectedItems = null;
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRIES_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRIES'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  onClosed() {

  }

  onEdit(item: MeasurementUnitModel) {
    if (!item) {
      return;
    }
    if (item.id > 0) {
      delete this.clonedItems[item.id];
      this.apiService.put('api/MeasurementUnit', item).toPromise()
        .then(th => {
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.post('api/MeasurementUnit', item).toPromise()
        .then(th => {
          this.items[this.findIndexById(0)] = th;
          this.items = [...this.items];
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_CREATED'), life: 3000 });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    }
  }

  onEditCancel(item: MeasurementUnitModel, index: number) {
    if (item.id === 0) {
      this.items.splice(index, 1);
    } else {
      this.items[index] = this.clonedItems[item.id];
      delete this.clonedItems[item.id];
    }
  }

  public loadData(event) {

    Object.assign(event.filters,
      {
        measurementUnitTypeId:
        {
          value: this.measurementUnitType.id,
          matchMode: 'equals'
        }
      });


    const params = new HttpParams().set('filter', JSON.stringify(event));
    this.apiService.get('api/MeasurementUnit', params).toPromise().then(th => {
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

  public close() {
    this.closed.emit();
  }
}
