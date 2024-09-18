import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { UserManageModel, UserModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';
import { EnumService } from 'src/app/core/services/enum.service';

@Component({
  selector: 'app-user-edit-form',
  templateUrl: './user-edit-form.component.html',
  styleUrls: ['./user-edit-form.component.scss'],
})
export class UserEditFormComponent implements OnInit {

  private _model: UserManageModel;
  @Input() public set model(data: UserManageModel) {
    if (data) {
      this._model = data;
      this.editForm.patchValue(this._model);
    } else {
      this.editForm.reset();
    }
  }
  public get model() {
    return this._model;
  }

  @Input() public isOpen: boolean;
  @Input() public roles: string[];

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  public emailRegex = '^\\w+[\\w-\\.]*\\@\\w+((-\\w+)|(\\w*))\\.[a-z]{2,3}$';

  editForm: FormGroup = this.fb.group({
    id: [0],
    userName: ['', Validators.required],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    patronymic: [''],
    gender: [0],
    birthDate: ['', Validators.required],
    phoneNumber: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    isActive: [false]
  });

  public genders: any[] = [];
  constructor(private apiService: ApiService,
    private translate: TranslateService,
    private messageService: MessageService,
    private fb: FormBuilder,
    private enumService: EnumService) { }

  ngOnInit(): void {
    this.genders = this.enumService.getGenders();
  }

  save() {
    const userManageModel: UserManageModel = this.editForm.value as UserManageModel;
    userManageModel.roles = this.model.roles;
    if (this.model.id === 0) {
      this.apiService.post('api/User', userManageModel).toPromise()
        .then(th => {
          const created = th as UserModel;
          created.birthDate = new Date(th.birthDate);
          this.saved.emit({ user: created, isNew: true });
          this.editForm.reset();
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.put('api/User', userManageModel).toPromise()
        .then(th => {
          const updated = th as UserModel;
          updated.birthDate = new Date(th.birthDate);
          this.saved.emit({ user: updated, isNew: false });
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
