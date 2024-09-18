import { ElementRef } from '@angular/core';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { PatientAgeType, ToothDirection, ToothType } from 'src/app/core/enums';
import { Teeth, ToothModel } from 'src/app/core/models/appsettings';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-tooth',
  templateUrl: './tooth.component.html',
  styleUrls: ['./tooth.component.scss']
})
export class ToothComponent implements OnInit {

  public topLeftTeeth: ToothModel[] = [];
  public topRightTeeth: ToothModel[] = [];
  public bottomLeftTeeth: ToothModel[] = [];
  public bottomRightTeeth: ToothModel[] = [];

  @ViewChild('fileUpload', { static: false }) fileUpload: ElementRef;

  public selectedTooth: ToothModel;
  public selectedDirection: ToothDirection;

  public editForm = this.fb.group({
    id: this.fb.control(0, Validators.required),
    name: this.fb.control('', Validators.required),
  });

  constructor(private apiService: ApiService, private fb: FormBuilder,
    private messageService: MessageService,
    private translate: TranslateService) {

  }

  public childTeeth: Teeth;
  public adultTeeth: Teeth;

  ngOnInit(): void {

    this.apiService.get('api/Tooth/GetAll')
      .toPromise()
      .then(th => {
        const teeth = th as ToothModel[];
        this.childTeeth = this.getTeeth(teeth.filter(f => f.toothType === ToothType.Child), PatientAgeType.Child);
        this.adultTeeth = this.getTeeth(teeth.filter(f => f.toothType === ToothType.Adult), PatientAgeType.Adult);
      })
      .catch(error => { })
      .finally(() => { });
  }

  getTeeth(data: ToothModel[], patientAgeType: PatientAgeType): Teeth {
    const teeth = {
      patientId: 0,
      patientAgeType: patientAgeType,
      bottomLeft: [],
      bottomRight: [],
      topLeft: [],
      topRight: []
    };

    teeth.topLeft = data.filter(f => f.direction === ToothDirection.TopLeft)
      .sort((n1, n2) => {
        if (n1.position > n2.position) {
          return 1;
        }
        if (n1.position < n2.position) {
          return -1;
        }
        return 0;
      });

    teeth.topRight = data.filter(f => f.direction === ToothDirection.TopRight)
      .sort((n1, n2) => {
        if (n1.position > n2.position) {
          return 1;
        }
        if (n1.position < n2.position) {
          return -1;
        }
        return 0;
      });

    teeth.bottomLeft = data.filter(f => f.direction === ToothDirection.BottomLeft)
      .sort((n1, n2) => {
        if (n1.position > n2.position) {
          return 1;
        }
        if (n1.position < n2.position) {
          return -1;
        }
        return 0;
      });

    teeth.bottomRight = data.filter(f => f.direction === ToothDirection.BottomRight)
      .sort((n1, n2) => {
        if (n1.position > n2.position) {
          return 1;
        }
        if (n1.position < n2.position) {
          return -1;
        }
        return 0;
      });

    return teeth;
  }


  public selectTooth(tooth: ToothModel) {
    if (tooth) {
      this.selectedTooth = tooth;
      this.selectedDirection = tooth.direction;
      this.editForm.patchValue(this.selectedTooth);
    } else {
      this.selectedTooth = undefined;
      this.selectedDirection = ToothDirection.Default;
    }
  }

  public openFileUpload() {
    this.fileUpload.nativeElement.click();
  }

  upload(files) {
    if (files.length > 0) {
      const formData = new FormData();

      for (const file of files) {
        formData.append('toothImages', file);
      }

      this.apiService.post('/api/Tooth/UploadImage/' + this.selectedTooth.id, formData)
        .toPromise()
        .then(th => {
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('TOOTH_IMAGE_UPDATED'), life: 3000 });
          // this.selectedTooth.image = th.imageUrl;
          // if (this.selectedDirection === ToothDirection.BottomLeft) {
          //   this.bottomLeftTeeth.find(f => f.id === this.selectedTooth.id).image = th.imageUrl;
          // }
          // else if (this.selectedDirection === ToothDirection.BottomRight) {
          //   this.bottomRightTeeth.find(f => f.id === this.selectedTooth.id).image = th.imageUrl;
          // }
          // else if (this.selectedDirection === ToothDirection.TopLeft) {
          //   this.topLeftTeeth.find(f => f.id === this.selectedTooth.id).image = th.imageUrl;
          // }
          // else if (this.selectedDirection === ToothDirection.TopRight) {
          //   this.topRightTeeth.find(f => f.id === this.selectedTooth.id).image = th.imageUrl;
          // }
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => {
        });
    }
  }

  save() {
    const t = this.editForm.value;
    this.apiService.post('api/Tooth', t)
      .toPromise()
      .then(th => {
        this.selectedTooth.name = t.name;
        this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('TOOTH_UPDATED'), life: 3000 });
      })
      .catch(error => {
        this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('TOOTH_UPDATE_ERROR'), life: 3000 });
      })
      .finally(() => { });
  }

}
