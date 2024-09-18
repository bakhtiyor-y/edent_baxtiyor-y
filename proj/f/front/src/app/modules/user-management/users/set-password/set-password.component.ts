import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { SetPasswordModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-set-password',
  templateUrl: './set-password.component.html',
  styleUrls: ['./set-password.component.scss']
})
export class SetPasswordComponent implements OnInit {

  private _model: SetPasswordModel;
  @Input() public set model(data: SetPasswordModel){
    if(data){
      this._model = data;
      this.editForm.patchValue(this._model);
    }
  }
  public get model(){
    return this._model;
  }

  @Input() public isOpen: boolean;
  @Input() public roles: string[];

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  editForm: FormGroup;

  constructor(private apiService: ApiService,
    private translate: TranslateService,
    private messageService: MessageService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.editForm = this.fb.group({
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
      userId: ['', Validators.required]
    }, { validators: this.checkPasswords });
  }

  checkPasswords: ValidatorFn = (group: AbstractControl):  ValidationErrors | null => { 
    let pass = group.get('password').value;
    let confirmPass = group.get('confirmPassword').value
    return pass === confirmPass ? null : { notSame: true }
  }

  public save() {
    this.model = this.editForm.value as SetPasswordModel;
    if (this.model.userId !== 0) {
      this.apiService.put('api/User/SetPassword', this.model).toPromise()
        .then(th => {
          this.saved.emit();
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
