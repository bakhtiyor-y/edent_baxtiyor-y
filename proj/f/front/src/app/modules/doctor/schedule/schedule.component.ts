import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { DayOfWeek } from 'src/app/core/enums';
import { ApiService } from 'src/app/core/services';
import { clone } from 'src/app/core/util/clone';
import { getDateTime, getTimeString } from 'src/app/core/util/get-date-from-calendar';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {

  public weekDays = [{ name: this.translate.instant('MONDAY'), value: DayOfWeek.Monday },
  { name: this.translate.instant('TUESDAY'), value: DayOfWeek.Tuesday },
  { name: this.translate.instant('WEDNESDAY'), value: DayOfWeek.Wednesday },
  { name: this.translate.instant('THURSDAY'), value: DayOfWeek.Thursday },
  { name: this.translate.instant('FRIDAY'), value: DayOfWeek.Friday },
  { name: this.translate.instant('SATURDAY'), value: DayOfWeek.Saturday },
  { name: this.translate.instant('SUNDAY'), value: DayOfWeek.Sunday }];

  public editForm = this.fb.group({
    id: this.fb.control(0),
    doctorId: this.fb.control(0, Validators.required),
    fromDate: this.fb.control(new Date(), Validators.required),
    toDate: this.fb.control(new Date(), Validators.required),
    admissionDuration: this.fb.control(30, Validators.required),
    scheduleSettings: this.fb.array([])
  });

  constructor(private apiService: ApiService, private fb: FormBuilder,
    private messageService: MessageService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.apiService.get('api/Schedule/GetSchedule')
      .toPromise()
      .then(th => {
        if (th) {
          th.fromDate = new Date(th.fromDate);
          th.toDate = new Date(th.toDate);
          const fArray = this.editForm.get('scheduleSettings') as FormArray;
          fArray.clear();
          for (const iterator of th.scheduleSettings) {
            iterator.fromTime = getDateTime(iterator.fromTime);
            iterator.toTime = getDateTime(iterator.toTime);
            fArray.push(this.getScheduleSettingForm());
          }
          this.editForm.patchValue(th);
        } else {
          this.editForm.reset();
        }
      })
      .catch(error => { })
      .finally(() => { });
  }

  public getScheduleSettingForm() {
    return this.fb.group({
      id: this.fb.control(0),
      fromTime: this.fb.control(new Date(), Validators.required),
      toTime: this.fb.control(new Date(), Validators.required),
      settingDayOfWeeks: this.fb.control(0)
    });
  }

  public save() {
    if (!this.editForm.valid) {
      return;
    }
    const scheduleSetting = clone(this.editForm.value);
    scheduleSetting.fromDate = new Date(scheduleSetting.fromDate);
    scheduleSetting.toDate = new Date(scheduleSetting.toDate);
    for (const iterator of scheduleSetting.scheduleSettings) {
      iterator.fromTime = getTimeString(new Date(iterator.fromTime));
      iterator.toTime = getTimeString(new Date(iterator.toTime));
    }
    if (scheduleSetting.id > 0) {
      this.apiService.put('api/Schedule', scheduleSetting).toPromise()
        .then(th => {
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => { });
    }
    else {
      this.apiService.post('api/Schedule', scheduleSetting).toPromise()
        .then(th => {
          this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => { });
    }

  }

  public addSetting() {
    const fArray = this.editForm.get('scheduleSettings') as FormArray;
    const itemForm = this.getScheduleSettingForm();
    fArray.push(itemForm);
  }

  public deleteSetting(i) {
    const fArray = this.editForm.get('scheduleSettings') as FormArray;
    fArray.removeAt(i);
  }

  public onWeekDayChange(event) {

  }

  getFormArray(name: string): FormArray {
    return this.editForm.get(name) as FormArray;
  }

}
