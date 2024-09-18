import { EventEmitter, Output } from '@angular/core';
import { Input } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { CountryModel } from 'src/app/core/models/manuals';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-country-edit-form',
  templateUrl: './country-edit-form.component.html',
  styleUrls: ['./country-edit-form.component.scss']
})
export class CountryEditFormComponent implements OnInit {

  public country: CountryModel;

  @Input() public set model(item: CountryModel) {
    if (item) {
      this.country = item;
      this.editForm.patchValue(item);
    } else {
      this.editForm.reset();
    }
  }

  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  public editForm = this.fb.group({
    id: this.fb.control(0),
    name: this.fb.control('', Validators.required),
    code: this.fb.control('', Validators.required)
  });

  constructor(private apiService: ApiService,
    private fb: FormBuilder,
    private messageService: MessageService,
    private translate: TranslateService) {

  }

  ngOnInit(): void {

  }

  save() {
    if (!this.editForm.valid) {
      return;
    }
    const country = this.editForm.value;
    if (country.id === 0) {
      this.apiService.post('api/Country', country).toPromise()
        .then(th => {
          this.saved.emit({ item: th, isNew: true });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.put('api/Country', country).toPromise()
        .then(th => {
          this.saved.emit({ item: th, isNew: false });
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_UPDATE'), life: 3000 });
        })
        .finally(() => { });
    }
  }

  public close() {
    this.closed.emit();
  }

}
