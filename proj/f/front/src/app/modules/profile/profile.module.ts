import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileComponent } from './profile.component';
import { SharedModule } from '../shared/shared.module';
import { AvatarModule } from 'primeng/avatar';



@NgModule({
  declarations: [ProfileComponent],
  imports: [
    AvatarModule,
    CommonModule,
    SharedModule
  ],
  exports:[ProfileComponent]
})
export class ProfileModule { }
