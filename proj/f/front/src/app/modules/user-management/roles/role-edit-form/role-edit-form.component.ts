import { EventEmitter, ViewChild } from '@angular/core';
import { Component, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { MessageService } from 'primeng/api';
import { RoleModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-role-edit-form',
  templateUrl: './role-edit-form.component.html',
  styleUrls: ['./role-edit-form.component.scss']
})
export class RoleEditFormComponent implements OnInit {

  @Input() public isOpen: boolean;
  @Input() public permissions: string[];

  @Output() public closed: EventEmitter<any> = new EventEmitter<any>();
  @Output() public saved: EventEmitter<any> = new EventEmitter<any>();

  editForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    id: [0, Validators.required]
  });

  private _model: RoleModel;
  @Input() public set model(data: RoleModel) {
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

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private translate: TranslateService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
  }

  save() {
    const data = this.editForm.value as RoleModel;
    data.permissions = this.model.permissions;

    if (data.id === 0) {
      this.apiService.post('api/Role', data).toPromise()
        .then(th => {
          this.saved.emit({ role: th, isNew: true });
          this.editForm.reset();
        })
        .catch(error => {
          this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_CREATE'), life: 3000 });
        })
        .finally(() => { });
    } else {
      this.apiService.put('api/Role', data).toPromise()
        .then(th => {
          this.saved.emit({ role: th, isNew: false });
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
