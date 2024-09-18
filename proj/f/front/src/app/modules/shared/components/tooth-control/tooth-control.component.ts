import { ElementRef, Input, Output, ViewChild } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ToothDirection, ToothState, ToothType } from 'src/app/core/enums';
import { PatientToothModel } from 'src/app/core/models/common';

@Component({
  selector: 'app-tooth-control',
  templateUrl: './tooth-control.component.html',
  styleUrls: ['./tooth-control.component.scss']
})
export class ToothControlComponent implements OnInit {

  @Input() public model: PatientToothModel;

  @Input() public editMode;

  @Input() selected: boolean;

  @Output() public toothSelected: EventEmitter<PatientToothModel> = new EventEmitter<PatientToothModel>();

  @Output() public toothDeselected: EventEmitter<PatientToothModel> = new EventEmitter<PatientToothModel>();

  @ViewChild('toothText') toothHtml: ElementRef;

  ToothDirection = ToothDirection;

  constructor() { }

  ngOnInit(): void {
  }

  public getToothImage(patientTooth: PatientToothModel) {
    if (!patientTooth || !patientTooth.tooth) {
      return '';
    }

    let imageNamePart = '';
    let imageNamePartPrefix = patientTooth.toothState == ToothState.Implanted ? 'imp_' : '';

    let imageNamePartSuffix = patientTooth.tooth.toothType == ToothType.Adult ? '' : 'ch_';
    if (patientTooth.tooth.direction === ToothDirection.BottomLeft) {
      imageNamePart = `${imageNamePartPrefix}btl_${imageNamePartSuffix}${patientTooth.tooth.position}`;
    } else if (patientTooth.tooth.direction === ToothDirection.BottomRight) {
      imageNamePart = `${imageNamePartPrefix}btr_${imageNamePartSuffix}${patientTooth.tooth.position}`;
    } else if (patientTooth.tooth.direction === ToothDirection.TopLeft) {
      imageNamePart = `${imageNamePartPrefix}upl_${imageNamePartSuffix}${patientTooth.tooth.position}`;
    } else {
      imageNamePart = `${imageNamePartPrefix}upr_${imageNamePartSuffix}${patientTooth.tooth.position}`;
    }
    let imageUrlPart = patientTooth.tooth.toothType == ToothType.Adult ? `adult/${imageNamePart}.png` : `child/${imageNamePart}.png`;
    return `assets/layout/images/teeth/${imageUrlPart}`;

  }

  onToothSelect(e) {
    if (!this.editMode) {
      return;
    }
    this.toothSelected.emit(this.model);
  }

  onToothDeselect(e) {
    if (!this.editMode) {
      return;
    }
    this.toothDeselected.emit(this.model);
    return false;
  }

}
