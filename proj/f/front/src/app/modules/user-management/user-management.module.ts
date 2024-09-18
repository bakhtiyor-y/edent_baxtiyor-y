import { NgModule } from '@angular/core';
import { UsersComponent } from './users/users.component';
import { RolesComponent } from './roles/roles.component';
import { UserManagementRoutingModule } from './user-management-routing.module';
import { SharedModule } from '../shared/shared.module';
import { UserEditFormComponent } from './users/user-edit-form/user-edit-form.component';
import { SetPasswordComponent } from './users/set-password/set-password.component';
import { RoleEditFormComponent } from './roles/role-edit-form/role-edit-form.component';
import { DoctorsComponent } from './doctors/doctors.component';
import { DoctorEditFormComponent } from './doctors/doctor-edit-form/doctor-edit-form.component';
import { TechnicsComponent } from './technics/technics.component';
import { TechnicEditFormComponent } from './technics/technic-edit-form/technic-edit-form.component';


@NgModule({
  declarations: [UsersComponent,
    RolesComponent,
    UserEditFormComponent,
    SetPasswordComponent,
    RoleEditFormComponent,
    DoctorsComponent,
    DoctorEditFormComponent,
    TechnicsComponent,
    TechnicEditFormComponent],
  imports: [
    SharedModule,
    UserManagementRoutingModule
  ]
})
export class UserManagementModule { }
