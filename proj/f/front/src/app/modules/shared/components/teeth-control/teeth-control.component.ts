import { Component, EventEmitter, Input, OnInit, Output, QueryList, ViewChildren } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { PatientAgeType } from 'src/app/core/enums';
import { Teeth } from 'src/app/core/models/appsettings';
import { PatientToothModel } from 'src/app/core/models/common';
import { ToothControlComponent } from '../tooth-control/tooth-control.component';

@Component({
  selector: 'app-teeth-control',
  templateUrl: './teeth-control.component.html',
  styleUrls: ['./teeth-control.component.scss']
})
export class TeethControlComponent implements OnInit {
  PatientAgeType = PatientAgeType;
  selectedTeeth: PatientToothModel[] = [];
  selectedTeethIds: number[] = [];
  isSome: boolean = false;
  isAllSelected: boolean = false;

  private _someSelectionLabel: string;
  get someSelectionLabel() {
    return this._someSelectionLabel;
  }
  set someSelectionLabel(data: string) {
    this._someSelectionLabel = data;
    this.setMenuLabel(0, this._someSelectionLabel);
  }
  private _allSelectionLabel: string;
  get allSelectionLabel() {
    return this._allSelectionLabel;
  }
  set allSelectionLabel(data: string) {
    this._allSelectionLabel = data;
    this.setMenuLabel(1, this._allSelectionLabel);
  }

  menuItems: any[] = [
    {
      label: this.translate.instant('SELECT_SOME'),
      icon: 'pi pi-refresh',
      command: () => {
        this.isSomeToggle();
      }
    },
    {
      label: this.translate.instant('SELECT_ALL'),
      icon: 'pi pi-times',
      command: () => {
        this.selectAllToggle();
      }
    },
  ];

  @Input() editMode: boolean;

  @Output() public teethSelected: EventEmitter<PatientToothModel[]> = new EventEmitter<PatientToothModel[]>();
  @Output() public teethDeselected: EventEmitter<PatientToothModel[]> = new EventEmitter<PatientToothModel[]>();

  @ViewChildren(ToothControlComponent) teethCtrls: QueryList<ToothControlComponent>;


  @Input() teeth: Teeth;

  constructor(private translate: TranslateService, private messageService: MessageService) { }

  ngOnInit(): void {
  }

  setMenuLabel(index: number, label: string): void {
    if (this.menuItems && this.menuItems.length > index) {
      this.menuItems[index].label = label;
    }
  }

  selectAllToggle() {
    if(this.isSome){
      this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('FIRST_CANCEL_SOME_SELECTION'), life: 3000 });
      return;
    }

    if (this.isAllSelected) {
      const deselectTeeth = this.deselectAll();
      this.teethDeselected.emit(deselectTeeth);
    } else {
      this.selectedTeeth = this.selectAll();
      this.teethSelected.emit(this.selectedTeeth);
    }
    this.isAllSelected = !this.isAllSelected;
    this.allSelectionLabel = this.isAllSelected ? this.translate.instant('DESELECT_ALL') : this.translate.instant('SELECT_ALL');
  }

  public deselectAll(): PatientToothModel[] {
    this.selectedTeeth = [];
    const deselectTeeth = [];
    this.teethCtrls.forEach(tooth => {
      deselectTeeth.push(tooth.model);
    });
    this.selectedTeethIds = [];
    return deselectTeeth;
  }

  public selectAll(): PatientToothModel[] {
    const selectTeeth = [];
    this.teethCtrls.forEach(tooth => {
      selectTeeth.push(tooth.model);
      this.selectedTeethIds.push(tooth.model.id);
    });
    return selectTeeth;
  }

  public setSelectedIds(patientToothIds: number[]): void {
    this.selectedTeethIds = patientToothIds;
  }

  isSelected(pt: PatientToothModel): boolean {
    const selected = this.selectedTeethIds.some(s => s === pt.id);
    if (selected && !this.selectedTeeth.some(s => s.id == pt.id)) {
      this.selectedTeeth.push(pt);
    }
    return selected;
  }

  isSomeToggle() {
    if(this.isAllSelected){
      this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('FIRST_CANCEL_ALL_SELECTION'), life: 3000 });
      return;
    }

    this.isSome = !this.isSome;
    if (!this.isSome) {
      this.deselectAll();
    }
    this.someSelectionLabel = this.isSome ? this.translate.instant('CANCEL_SOME') : this.translate.instant('SELECT_SOME');
  }

  // onSetTreatment() {
  //   this.teethSelected.emit(this.selectedTeeth);
  // }

  onToothSelected(patientTooth: PatientToothModel) {

    if (!this.isSome) {
      this.deselectAll();
    }
    this.selectedTeeth.push(patientTooth);
    this.selectedTeethIds.push(patientTooth.id);
    this.teethSelected.emit(this.selectedTeeth);
  }

  onToothDeselected(patientTooth: PatientToothModel) {
    const index = this.selectedTeeth.findIndex(f => f.id === patientTooth.id);
    if (index >= 0) {
      this.selectedTeeth.splice(index, 1);
    }
    const ind = this.selectedTeethIds.findIndex(f => f === patientTooth.id);
    if (ind >= 0) {
      this.selectedTeethIds.splice(ind, 1);
    }
  }

}
