import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { RoleModel, SetPasswordModel, UserManageModel, UserModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  public userDialog: boolean;
  public passwordDialog: boolean;

  public users: UserModel[] = [];

  public selectedUsers: UserModel[];

  public userManage: UserManageModel;

  public setPasswordModel: SetPasswordModel;

  public cols: any[];

  public roles: RoleModel[];
  public loading: boolean;
  public totalRecords: number;

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private translate: TranslateService) {

  }

  ngOnInit(): void {

    this.loading = false;
    this.apiService.get('api/Role/GetAll').toPromise().then(th => {
      this.roles = th;
    }).catch(error => {
    }).finally(() => { });

    this.cols = [
      { field: 'useName', header: 'User name' },
      { field: 'fullName', header: 'Full name' },
      { field: 'birthDate', header: 'Birth date' },
      { field: 'phoneNumber', header: 'Phone number' },
      { field: 'email', header: 'Email' },
      { field: 'isActive', header: 'Is active' },
    ];
  }

  getUserManageModel(user: UserModel): UserManageModel {
    return {
      id: user.id || 0,
      birthDate: user.birthDate,
      email: user.email,
      firstName: user.firstName,
      isActive: user.isActive,
      lastName: user.lastName,
      patronymic: user.patronymic,
      phoneNumber: user.phoneNumber,
      profileImage: user.profileImage,
      userName: user.userName,
      gender: user.gender
    } as UserManageModel;
  }

  add() {
    const user = {} as UserModel;
    user.birthDate = new Date();
    this.userManage = this.getUserManageModel(user);
    this.userDialog = true;
  }

  deleteSelectedUsers() {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRIES'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.post('api/User/DeleteSelected', this.selectedUsers)
          .toPromise()
          .then(th => {
            this.users = this.users.filter(val => !this.selectedUsers.includes(val));
            this.selectedUsers = null;
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRIES_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRIES'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  edit(user: UserModel) {
    this.apiService.get('api/Role/ByUser?userId=' + user.id).toPromise()
      .then(th => {
        this.userManage.roles = th;
      }).catch(error => {
      }).finally(() => { });

    this.userManage = this.getUserManageModel(user);
    this.userDialog = true;
  }

  delete(user: UserModel) {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRY'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.delete('api/Doctor?id=' + user.id)
          .toPromise()
          .then(th => {
            this.users = this.users.filter(val => val.id !== user.id);
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRY'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  onClosed() {
    this.userDialog = false;
  }

  onSaved(result: { user: UserModel, isNew: boolean }) {
    if (result.isNew) {
      this.users.push(result.user);
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_CREATED'), life: 3000 });
    } else {
      this.users[this.findIndexById(this.userManage.id)] = result.user;
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
    }
    this.users = [...this.users];
    this.userDialog = false;
  }

  public setPassword(user: UserModel) {
    this.setPasswordModel = {} as SetPasswordModel;
    this.setPasswordModel.userId = user.id;
    this.passwordDialog = true;
  }

  public setPasswordHandler() {
    this.passwordDialog = false;
    this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
  }

  public setPasswordClosed() {
    this.passwordDialog = false;
    this.setPasswordModel = null;
  }

  public loadUsers(event) {
    const params = new HttpParams().set('filter', JSON.stringify(event));
    this.apiService.get('api/User', params).toPromise().then(th => {
      th.data.forEach(user => {
        user.birthDate = new Date(user.birthDate);
      });
      this.users = th.data;
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
    for (let i = 0; i < this.users.length; i++) {
      if (this.users[i].id === id) {
        index = i;
        break;
      }
    }
    return index;
  }
}
