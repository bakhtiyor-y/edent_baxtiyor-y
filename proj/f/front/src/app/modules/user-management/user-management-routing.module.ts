import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { DoctorsComponent } from './doctors/doctors.component';
import { PatientsComponent } from '../shared/components/patients/patients.component';
import { RolesComponent } from './roles/roles.component';
import { TechnicsComponent } from './technics/technics.component';
import { UsersComponent } from './users/users.component';


@NgModule({
  imports: [
    RouterModule.forChild([
      {
        path: '',
        children: [
          { path: 'users', component: UsersComponent },
          { path: 'roles', component: RolesComponent },
          { path: 'doctors', component: DoctorsComponent },
          { path: 'technics', component: TechnicsComponent },
          { path: 'patients', component: PatientsComponent }
        ]
      }
    ])
  ],
  exports: [RouterModule]
})
export class UserManagementRoutingModule { }
