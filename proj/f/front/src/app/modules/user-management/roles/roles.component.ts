import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { RoleModel } from 'src/app/core/models/user-management';
import { ApiService } from 'src/app/core/services';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {

  public roleDialog: boolean;
  public passwordDialog: boolean;

  public roles: RoleModel[] = [];

  public selectedRoles: RoleModel[];

  public role: RoleModel;

  public cols: any[];

  public loading: boolean;

  public totalRecords: number;
  public permissions: string[] = [];

  constructor(private apiService: ApiService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private translate: TranslateService) { }

  ngOnInit(): void {
    this.loading = false;

    this.apiService.get('api/Permission').toPromise().then(th => {
      th.forEach(element => {
        this.permissions.push(element.name);
      });
    }).catch(error => {
    }).finally(() => { });

    this.cols = [
      { field: 'name', header: 'Role name' }
    ];
  }

  add() {
    this.role = { id: 0, name: '', permissions: [] } as RoleModel;
    this.roleDialog = true;
  }

  deleteSelectedRoles() {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRIES'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.post('api/Role/DeleteSelected', this.selectedRoles)
          .toPromise()
          .then(th => {
            this.roles = this.roles.filter(val => !this.selectedRoles.includes(val));
            this.selectedRoles = null;
            this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRIES_DELETED'), life: 3000 });
          })
          .catch(error => {
            this.messageService.add({ severity: 'error', summary: this.translate.instant('ERROR'), detail: this.translate.instant('ERROR_ON_DELETE_ENTRIES'), life: 3000 });
          })
          .finally(() => { });
      }
    });
  }

  edit(role: RoleModel) {
    this.apiService.get('api/Permission/ByRole?roleId=' + role.id).toPromise()
      .then(th => {
        this.role = role;
        this.role.permissions = th;
        this.roleDialog = true;
      });
  }

  delete(role: RoleModel) {
    this.confirmationService.confirm({
      message: this.translate.instant('ARE_YOU_SURE_TO_DELETE_SELECTED_ENTRY'),
      header: this.translate.instant('CONFIRM'),
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.apiService.delete('api/Role?id=' + role.id)
          .toPromise()
          .then(th => {
            this.roles = this.roles.filter(val => val.id !== role.id);
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
    this.role = null;
    this.roleDialog = false;
  }

  onSaved(result) {
    if (result.isNew) {
      this.roles.push(result.role);
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_CREATED'), life: 3000 });
    } else {
      this.roles[this.findIndexById(this.role.id)] = result.role;
      this.messageService.add({ severity: 'success', summary: this.translate.instant('SUCCESSFUL'), detail: this.translate.instant('ENTRY_UPDATED'), life: 3000 });
    }
    this.roles = [...this.roles];
    this.roleDialog = false;
    this.role = null;
  }

  public loadRoles(event) {
    const params = new HttpParams().set('filter', JSON.stringify(event));
    this.apiService.get('api/Role', params).toPromise().then(th => {
      this.roles = th.data;
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
    for (let i = 0; i < this.roles.length; i++) {
      if (this.roles[i].id === id) {
        index = i;
        break;
      }
    }
    return index;
  }

}
