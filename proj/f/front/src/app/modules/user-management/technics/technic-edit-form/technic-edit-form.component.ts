import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { TechnicModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';

@Component({
  selector: 'app-technic-edit-form',
  templateUrl: './technic-edit-form.component.html',
  styleUrls: ['./technic-edit-form.component.scss']
})
export class TechnicEditFormComponent implements OnInit {

  public isNew: boolean;
  @Input() public set model(item: TechnicModel) {
    if (item) {
      this.isNew = item.id === 0;
      this.editForm.patchValue(item);
    } else {
      this.editForm.reset();
    }
  }
  @Input() public isOpen: boolean;

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  public genders: any[] = [];

  public editForm = this.fb.group({
    id: this.fb.control(0),
    firstName: this.fb.control('', Validators.required),
    lastName: this.fb.control('', Validators.required),
    patronymic: this.fb.control(''),
    gender: this.fb.control(0),
    phoneNumber: this.fb.control('', Validators.required),
    birthDate: this.fb.control(new Date(), Validators.required),
  });

  constructor(private fb: FormBuilder,
    private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService,
    private enumService: EnumService) { }

  ngOnInit(): void {
    this.genders = this.enumService.getGenders();
  }

  save() {
    if (!this.editForm.valid) {
      return;
    }
    const technic = this.editForm.value;
    technic.birthDate = new Date(technic.birthDate + 'Z');
    if (technic.id === 0) {
      this.apiService.post('api/Technic', technic).toPromise()
        .then(th => {
          this.saved.emit({ item: th, isNew: true });
          this.editForm.reset();
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.put('api/Technic', technic).toPromise()
        .then(th => {
          this.saved.emit({ item: th, isNew: false });
          this.editForm.reset();
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
